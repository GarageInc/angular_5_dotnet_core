import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from '../../../../common/services/authentication.service';
import { HeaderEnums } from '../../enums/header.enum';
import { EventService } from '../../../../common/services/event-service/event.service';
import { FileUploader } from 'ng2-file-upload';
import { SupplierOfferItem } from './../../models/supplier-offer-item';
import { SupplierOffersService } from '../../services/supplier-offers/supplier-offers.service';

import { OfferEnums } from '../../enums/offers.enum';

@Component({
  selector: 'app-offers-list',
  templateUrl: 'offers-list.component.html',
  styleUrls: ['offers-list.component.scss']
})
export class OffersListComponent implements OnInit {
  public offers: Array<SupplierOfferItem> = [];

  constructor(
    private supplierOffersService: SupplierOffersService,
    private events: EventService
  ) {}

  public ngOnInit() {
    this.load();

    this.events.subscribe(OfferEnums.Reload, () => {
      this.load();
    });
  }

  public load(): void {
    this.supplierOffersService
      .loadList()
      .subscribe(data => (this.offers = data));
  }

  public remove(item: SupplierOfferItem): void {
    this.supplierOffersService.remove(item).subscribe(() => {
      this.load();
    });
  }
}
