import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { HttpParams } from '@angular/common/http/src/params';
import { debounce } from 'rxjs/operators/debounce';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { BaseAuthService } from './base-auth.service';
import { AuthenticationService } from './authentication.service';
import { BaseFilter } from './../models/base-filter';

@Injectable()
export class FilterListBackendService<
  F extends BaseFilter,
  I
> extends BaseAuthService {
  constructor(
    protected http: Http,
    protected authenticationService: AuthenticationService
  ) {
    super(http, authenticationService);
  }

  public chunkUrl = '';

  public next(filter: BaseFilter): Observable<Array<I>> {
    return this.httpPost(this.chunkUrl, filter);
  }
}
