import { Injectable } from '@angular/core';
import {
  Request,
  XHRBackend,
  RequestOptions,
  Response,
  Http,
  RequestOptionsArgs,
  Headers
} from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { Router } from '@angular/router';
import { BaseNavigationService } from './base-navigation.service';

export class BaseAuthenticatedHttpService extends Http {
  constructor(
    public backend: XHRBackend,
    public defaultOptions: RequestOptions,
    public navigation: BaseNavigationService
  ) {
    super(backend, defaultOptions);
  }

  request(
    url: string | Request,
    options?: RequestOptionsArgs
  ): Observable<Response> {
    return super.request(url, options).catch((error: Response) => {
      if (
        (error.status === 401 || error.status === 403) &&
        (window.location.href.match(/\?/g) || []).length < 2
      ) {
        this.navigation.goToLogin();
      }
      return Observable.throw(error);
    });
  }
}
