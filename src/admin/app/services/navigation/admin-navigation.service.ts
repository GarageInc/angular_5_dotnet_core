import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BaseNavigationService } from './../../../../common/services/base-navigation.service';

@Injectable()
export class AdminNavigationService extends BaseNavigationService {
  constructor(public router: Router) {
    super(router);

    this.virtualDirectoryUrl = 'admin';
  }
}
