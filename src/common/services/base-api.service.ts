import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import { HttpParams } from '@angular/common/http/src/params';
import { debounce } from 'rxjs/operators/debounce';
import { Http, Headers, RequestOptions, Response } from '@angular/http';

@Injectable()
export class BaseApiService {
  constructor() {}

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

  protected map(response: Response): any {
    return response.json();
  }
}
