import { ProducerItemSearchModel } from '../../models/api/producer-item-search-model';
import { SupplierItemSearchModel } from '../../models/api/supplier-item-search-model';
import { Injectable } from '@angular/core';

@Injectable()
export class SearchDataService {
  public producerItems: ProducerItemSearchModel[] = [];
  public supplierItems: SupplierItemSearchModel[] = [];

  constructor() {}
}
