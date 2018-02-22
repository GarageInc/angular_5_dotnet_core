import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { ActivatedRoute } from '@angular/router';
import { SearchBackendService } from '../../../services/backend-service/search-backend.service';
import { SupplierModel } from '../../../models/api/supplier-model';
import { Title } from '@angular/platform-browser';
import { SeoAdapterService } from '../../../services/seo-adapter/seo-adapter.service';
import { SeoBackendService } from '../../../services/seo-service/seo-backend.service';

import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/concatMap';
import { Observable } from 'rxjs/Observable';
import { SeoParameter } from '../../../models/api/seo-parameter';
import { Contact } from '../../../models/api/contact';
import { SalePoint } from '../../../models/api/sale-point';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'distributor-details',
  templateUrl: './distributor-details.component.html',
  styleUrls: ['./distributor-details.component.scss']
})
export class DistributorDetailsComponent implements OnInit {
  constructor(
    private activateRoute: ActivatedRoute,
    private seoAdapter: SeoAdapterService,
    private backend: SearchBackendService,
    private seoBackend: SeoBackendService
  ) {}

  public subscription: Subscription;

  public supplier: SupplierModel;
  private anotherSupplierContacts: Contact[];
  private showingSupplierContacts: Contact[];

  ngOnInit() {
    this.subscription = this.activateRoute.params.subscribe(params => {
      const companyName = params['company'];

      if (companyName) {
        this.loadSupplierDetails(companyName);
      }
    });
  }

  public loadSupplierDetails(name: string): void {
    const backend = this.backend.searchSupplier(name).map(data => {
      this.supplier = data;

      if (this.supplier.salePoints.length === 1) {
        this.supplier.salePoints[0].name = 'Офис';
      }

      this.showingSupplierContacts = this.supplier.contacts.slice(0, 2);
      this.anotherSupplierContacts = this.supplier.contacts.slice(
        2,
        this.supplier.contacts.length
      );
      this.seoAdapter.setSupplierSeo(this.supplier);
      return data;
    });

    const chain = backend
      .filter((data: SupplierModel) => !!data.seoParameterId)
      .concatMap((data: SupplierModel) => {
        return this.seoBackend
          .getSeo(data.seoParameterId)
          .map((seoParam: SeoParameter) =>
            this.seoAdapter.setSeoParameter(seoParam)
          );
      });

    chain.subscribe();
  }

  public getSupplierContacts(): Contact[] {
    return this.showingSupplierContacts;
  }

  public getSalePointContacts(salePoint: SalePoint): string[] {
    let phones = [salePoint.phone];

    this.anotherSupplierContacts.forEach(x => {
      phones.push(x.phone);
    });

    const supplierContacts = this.getSupplierContacts();
    phones = phones.filter(x => !supplierContacts.some(y => y.phone === x));

    return phones;
  }
}
