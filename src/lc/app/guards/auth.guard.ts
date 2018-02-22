import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { BaseAuthGuard } from '../../../common/guards/base-auth.guard';
import { LcNavigationService } from './../services/navigation/lc-navigation.service';

@Injectable()
export class AuthGuard extends BaseAuthGuard {
  constructor(public navigation: LcNavigationService) {
    super(navigation);
  }
}
