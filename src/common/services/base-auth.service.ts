import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { AuthenticationService } from './authentication.service';
import { Observable } from 'rxjs/Observable';
import { BaseApiService } from './base-api.service';

@Injectable()
export class BaseAuthService extends BaseApiService {
  constructor(
    protected http: Http,
    protected authenticationService: AuthenticationService
  ) {
    super();
  }

  private get options(): RequestOptions {
    // add authorization header with jwt token
    const headers = new Headers({
      Authorization: this.authenticationService.bearerToken
    });
    const options = new RequestOptions({ headers: headers });

    return options;
  }

  public httpGet(url: string): Observable<any> {
    return this.http.get(url, this.options).map(this.map);
  }

  public httpPost(url: string, body: any): Observable<any> {
    return this.http.post(url, body, this.options).map(this.map);
  }
}
