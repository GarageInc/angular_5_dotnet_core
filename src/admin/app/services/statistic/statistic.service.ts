import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Statistic } from '../../models/statistic';
import { Observable } from 'rxjs/Observable';
import { AuthenticationService } from './../../../../common/services/authentication.service';
import { BaseAuthService } from './../../../../common/services/base-auth.service';

@Injectable()
export class StatisticService extends BaseAuthService {
  constructor(
    protected http: Http,
    protected authenticationService: AuthenticationService
  ) {
    super(http, authenticationService);
  }

  public getFull(): Observable<Statistic> {
    return this.httpGet('api/v1/statistic/full');
  }
}
