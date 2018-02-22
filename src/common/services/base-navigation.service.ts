import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

export class BaseNavigationService {
  public virtualDirectoryUrl = '';

  constructor(public router: Router) {}

  public goToLogin(): void {
    this.router.navigate(['/' + this.virtualDirectoryUrl + '/login']);
  }

  public goToMain(): void {
    this.router.navigate(['/' + this.virtualDirectoryUrl]);
  }
}
