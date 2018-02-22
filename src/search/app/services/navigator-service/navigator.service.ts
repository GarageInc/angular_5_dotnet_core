import { Injectable } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import { Title } from '@angular/platform-browser/src/browser/title';
import { ProducerItemSearchModel } from '../../models/api/producer-item-search-model';
import { SupplierModel } from '../../models/api/supplier-model';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { promise } from 'selenium-webdriver';

@Injectable()
export class NavigatorService {
  constructor(private router: Router) {}

  public goToSearch(searchString: string): Promise<boolean> {
    return this.router.navigate(['/search'], {
      queryParams: { art: searchString }
    });
  }

  public goToMain(): Promise<boolean> {
    return this.router.navigate(['/']);
  }

  public goToPart(
    producerName: string,
    producerCode: string
  ): Promise<boolean> {
    return this.router.navigate([
      '/parts/' + producerName.toLowerCase() + '/' + producerCode
    ]);
  }
}
