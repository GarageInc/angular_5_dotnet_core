import { ProducerItemSearchModel } from '../../models/api/producer-item-search-model';
import { SupplierItemSearchModel } from '../../models/api/supplier-item-search-model';
import { Injectable } from '@angular/core';
import { LocalStorageService } from '../local-storage/local-storage.service';
import { SearchTripAndTrick } from './../../models/search-trip-and-trick';

@Injectable()
export class SearchTripAndTricksService {
  public key = 'SEARCH_TRIPS_AND_TRICKS';

  public maxCount = 100;

  constructor(public localStorage: LocalStorageService) {}

  public list(): Array<SearchTripAndTrick> {
    const items = this.localStorage.getItem(this.key);
    return items || [];
  }

  public save(list: Array<SearchTripAndTrick>): void {
    this.localStorage.setItem(this.key, list);
  }

  public add(producerCode: string) {
    let items = this.list();

    const item = items.find(x => x.producerCode === producerCode);

    if (!!item) {
      item.searchCount = +item.searchCount || 0;
      item.searchCount++;
    } else {
      const newItem = new SearchTripAndTrick();
      newItem.producerCode = producerCode;

      items.push(newItem);

      if (items.length > this.maxCount) {
        items = items.slice(1);
      }
    }

    items = items.sort(function(
      a: SearchTripAndTrick,
      b: SearchTripAndTrick
    ): number {
      const keyA = +a.searchCount || 0,
        keyB = +b.searchCount || 0;
      if (keyA < keyB) {
        return 1;
      }
      if (keyA > keyB) {
        return -1;
      }
      return 0;
    });

    this.save(items);
  }
}
