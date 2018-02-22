import { Injectable } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';

import { AuthenticationService } from './../../../../common/services/authentication.service';
import { BaseAuthService } from './../../../../common/services/base-auth.service';
import { Http } from '@angular/http';
import { SupplierOfferItem } from '../../models/supplier-offer-item';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class SupplierOffersService extends BaseAuthService {
  constructor(
    protected http: Http,
    protected authenticationService: AuthenticationService
  ) {
    super(http, authenticationService);
  }

  public loadList(): Observable<Array<SupplierOfferItem>> {
    return this.httpGet('api/v1/lc/offers/list');
  }

  public remove(model: SupplierOfferItem): Observable<boolean> {
    return this.httpGet(
      this.buildGetOptions('api/v1/lc/offers/remove', {
        groupIdentifier: model.groupIdentifier
      })
    );
  }
}
