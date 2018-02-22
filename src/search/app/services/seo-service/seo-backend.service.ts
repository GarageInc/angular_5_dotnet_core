import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { HttpParams } from '@angular/common/http/src/params';
import { ProducerItemSearchModel } from './../../models/api/producer-item-search-model';
import { SupplierItemSearchModel } from './../../models/api/supplier-item-search-model';
import { SupplierModel } from './../../models/api/supplier-model';
import { SeoParameter } from '../../models/api/seo-parameter';
import { BaseApiService } from '../../../../common/services/base-api.service';

@Injectable()
export class SeoBackendService extends BaseApiService {
  constructor(protected httpClient: HttpClient) {
    super();
  }

  public getSeo(id: number): Observable<SeoParameter> {
    return this.httpClient.get<SeoParameter>(
      this.buildGetOptions('api/v1/seo/get', {
        id: id
      })
    );
  }
}
