import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser, isPlatformServer } from '@angular/common';

@Injectable()
export class LocalStorageService {
  constructor(
    @Inject(PLATFORM_ID) private platformId: Object,
    @Inject('LOCALSTORAGE') private localStorage: any
  ) {
    if (isPlatformServer(this.platformId)) {
      // Server only code.
    }
  }

  public setItem(key: string, value: any): boolean {
    if (isPlatformBrowser(this.platformId)) {
      this.localStorage.setItem(key, JSON.stringify(value));
      return true;
    }

    return false;
  }

  public getItem(key: string): any {
    if (isPlatformBrowser(this.platformId)) {
      return JSON.parse(this.localStorage.getItem(key));
    }

    return {};
  }
}
