import { Component, OnInit, Input } from '@angular/core';
import { SupplierItemSearchModel } from '../../../models/api/supplier-item-search-model';
import { NgModule, LOCALE_ID } from '@angular/core';
import { registerLocaleData } from '@angular/common';
// import lcoaleDECH from '@angular/common/locales/ru';
import { CbrService } from '../../../services/cbr-service/cbr.service';

// registerLocaleData(lcoaleDECH);

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.scss']
  // viewProviders: [{ provide: LOCALE_ID, useValue: 'ru' }]
})
export class SearchResultComponent implements OnInit {
  @Input() items: SupplierItemSearchModel[];

  constructor(private cbr: CbrService) {}

  ngOnInit() {}

  public getPrice(item: SupplierItemSearchModel): string {
    let price = item.supplier.prices.priceRu;

    if (price > 0) {
      return this.formatPrice(price);
    }

    price = item.supplier.prices.priceUsd;
    if (price > 0) {
      return this.formatPrice(price * this.cbr.usdCourse);
    }

    price = item.supplier.prices.priceEu;

    return this.formatPrice(price * this.cbr.euroCourse);
  }

  private formatPrice(value: number): string {
    return value + ' ₽';
  }

  public hasPrice(item: SupplierItemSearchModel): boolean {
    return (
      !!+item.supplier.prices.priceRu ||
      !!+item.supplier.prices.priceEu ||
      !!+item.supplier.prices.priceUsd
    );
  }
}
