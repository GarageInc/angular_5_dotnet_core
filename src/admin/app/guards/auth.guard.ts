import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AdminNavigationService } from './../services/navigation/admin-navigation.service';
import { BaseAuthGuard } from './../../../common/guards/base-auth.guard';

@Injectable()
export class AuthGuard extends BaseAuthGuard {
  constructor(public navigation: AdminNavigationService) {
    super(navigation);
  }
}
