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
import { CatalogItemStatisticFilter } from '../../models/catalog-item-statistic-filter';
import { CatalogItemStatistic } from '../../models/catalog-item-statistic';

@Injectable()
export class CatalogItemStatisticService extends FilterListBackendService<
  CatalogItemStatisticFilter,
  CatalogItemStatistic
> {
  constructor(
    protected http: Http,
    protected authenticationService: AuthenticationService
  ) {
    super(http, authenticationService);

    this.chunkUrl = '/api/v1/statistic/get-for-catalog';
  }
}
