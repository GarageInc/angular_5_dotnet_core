import { Injectable } from '@angular/core';
import { AuthenticationService } from './../../../../common/services/authentication.service';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ModerationItem } from '../../models/moderation-item';
import { PartProducer } from '../../models/part-producer';
import { PartSupplier } from '../../models/part-supplier';
import { FileUploader } from 'ng2-file-upload';
import { ModerationFilter } from '../../models/moderation-filter';
import { SupplierMatchModel } from '../../models/supplier-match-model';
import { FilterListBackendService } from './../../../../common/services/filter-list-backend.service';
import { BaseFilter } from '../../../../common/models/base-filter';

@Injectable()
export class ModerationService extends FilterListBackendService<
  ModerationFilter,
  ModerationItem
> {
  constructor(
    protected http: Http,
    protected authenticationService: AuthenticationService
  ) {
    super(http, authenticationService);

    this.chunkUrl = '/api/v1/moderation/need-moderation';
  }

  public getSuppliesMatchFor(
    producerCode: string
  ): Observable<SupplierMatchModel[]> {
    return this.httpGet(
      this.buildGetOptions('/api/v1/moderation/suppliers-match-for', {
        producerCode: producerCode
      })
    );
  }

  public producers(): Observable<PartProducer[]> {
    return this.httpGet('/api/v1/producers/list');
  }

  public suppliersFor(partId: number): Observable<PartSupplier[]> {
    return this.httpGet(
      this.buildGetOptions('/api/v1/suppliers/list-for', {
        partId: partId
      })
    );
  }

  public needModeration(
    offset: number,
    count: number,
    filter: ModerationFilter
  ): Observable<Array<ModerationItem>> {
    filter.count = count;
    filter.offset = offset;
    return this.next(filter); // this.httpPost('/api/v1/moderation/need-moderation', filter);
  }

  public needModerationCount(): Observable<number> {
    return this.httpGet('/api/v1/moderation/need-moderation-count');
  }

  public needModerationItem(id): Observable<ModerationItem> {
    return this.httpGet(
      this.buildGetOptions('/api/v1/moderation/need-moderation-item', {
        id: id
      })
    );
  }

  public delete(item: ModerationItem): Observable<void> {
    return this.httpGet(
      this.buildGetOptions('/api/v1/moderation/delete', {
        id: item.id
      })
    );
  }

  public restore(item: ModerationItem): Observable<void> {
    return this.httpGet(
      this.buildGetOptions('/api/v1/moderation/restore', {
        id: item.id
      })
    );
  }

  public update(item: ModerationItem): Observable<void> {
    return this.httpPost('/api/v1/moderation/update', item);
  }

  public createCatalogItem(item: ModerationItem): Observable<void> {
    return this.httpPost('/api/v1/moderation/create-catalog-item', item);
  }
}
