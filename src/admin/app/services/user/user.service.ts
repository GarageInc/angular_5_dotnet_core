import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { AuthenticationService } from './../../../../common/services/authentication.service';
import { User } from './../../../../common/models/user';
import { BaseAuthService } from './../../../../common/services/base-auth.service';

@Injectable()
export class UserService extends BaseAuthService {
  constructor(
    protected http: Http,
    protected authenticationService: AuthenticationService
  ) {
    super(http, authenticationService);
  }

  getUsers(): Observable<User[]> {
    return this.httpGet('/api/vi/user/all');
  }
}
