import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { HttpParams } from '@angular/common/http/src/params';
import { debounce } from 'rxjs/operators/debounce';
import { BaseAuthService } from '../../../../common/services/base-auth.service';
import { SearchSeoParams } from '../../../../common/models/seo/search-seo-params';
import { AuthenticationService } from '../../../../common/services/authentication.service';
import { Http } from '@angular/http';
import { SeoParameter } from './../../models/seo-parameter';
import { RobotsTxt } from '../../models/robots-txt';

@Injectable()
export class SeoAdminBackendService extends BaseAuthService {
  constructor(
    protected http: Http,
    protected authenticationService: AuthenticationService
  ) {
    super(http, authenticationService);
  }

  public get(): Observable<SearchSeoParams> {
    return this.httpGet('/files/ceo/search.json');
  }

  public getRobots(): Observable<RobotsTxt> {
    return this.httpGet('/api/v1/seo/get-robots');
  }

  public updateRobots(model: RobotsTxt): Observable<boolean> {
    return this.httpPost('api/v1/seo/update-robots', model);
  }

  public getSeo(id: number): Observable<SeoParameter> {
    return this.httpGet(
      this.buildGetOptions('api/v1/seo/get', {
        id: id
      })
    );
  }

  public update(item: SearchSeoParams): Observable<boolean> {
    return this.httpPost('api/v1/seo/update-search-seo', item);
  }

  public updateForPart(
    item: SeoParameter,
    catalogItemId: number
  ): Observable<boolean> {
    return this.httpPost('api/v1/seo/save-part-seo', {
      model: item,
      catalogItemId: catalogItemId
    });
  }
}
