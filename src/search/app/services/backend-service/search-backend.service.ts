import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { HttpParams } from '@angular/common/http/src/params';
import { ProducerItemSearchModel } from './../../models/api/producer-item-search-model';
import { SupplierItemSearchModel } from './../../models/api/supplier-item-search-model';
import { SupplierModel } from './../../models/api/supplier-model';
import { debounce } from 'rxjs/operators/debounce';
import { BaseApiService } from '../../../../common/services/base-api.service';

@Injectable()
export class SearchBackendService extends BaseApiService {
  constructor(protected httpClient: HttpClient) {
    super();
  }

  public searchSupplier(searchName: string): Observable<SupplierModel> {
    return this.httpClient.get<SupplierModel>(
      this.buildGetOptions('api/v1/search/supplier', {
        name: searchName
      })
    );
  }

  public searchInProducers(
    code: string,
    producerName: string | null = null
  ): Observable<ProducerItemSearchModel[]> {
    return this.httpClient.get<ProducerItemSearchModel[]>(
      this.buildGetOptions('api/v1/search/in-producers', {
        code: code,
        producerName: producerName
      })
    );
  }

  public searchInSuppliers(
    code: string,
    producerName: string | null = null
  ): Observable<SupplierItemSearchModel[]> {
    return this.httpClient.get<SupplierItemSearchModel[]>(
      this.buildGetOptions('api/v1/search/in-suppliers', {
        code: code,
        producerName: producerName
      })
    );
  }

  protected buildGetOptions(where: string, model: any): any {
    let url = where + '?';

    if (model) {
      for (const key in model) {
        if (key && model[key] !== undefined && model[key] !== null) {
          url += '&' + key + '=' + model[key];
        }
      }
    }

    return url;
  }
}
