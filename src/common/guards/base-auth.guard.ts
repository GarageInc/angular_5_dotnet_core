import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { BaseNavigationService } from '../services/base-navigation.service';

export class BaseAuthGuard implements CanActivate {
  constructor(public navigation: BaseNavigationService) {}

  canActivate() {
    if (localStorage.getItem('currentUser')) {
      // logged in so return true
      return true;
    }

    // not logged in so redirect to login page
    this.navigation.goToLogin();
    return false;
  }
}
