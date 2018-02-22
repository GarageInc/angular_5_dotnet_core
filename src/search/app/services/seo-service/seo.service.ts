import { Injectable, Inject } from '@angular/core';
import { Title, DOCUMENT, Meta } from '@angular/platform-browser';

@Injectable()
export class SeoService {
  /**
   * Inject the Angular 2 Title Service
   * @param titleService
   */
  constructor(protected titleService: Title, protected meta: Meta) {}

  public getTitle(): string {
    return this.titleService.getTitle();
  }

  public setTitle(newTitle: string) {
    this.titleService.setTitle(newTitle);
  }

  public setMetaDescription(content: string) {
    const tag = this.meta.getTag('description');
    const value = {
      name: 'description',
      content: content
    };

    if (!!tag) {
      this.meta.updateTag(value);
    } else {
      this.meta.addTag(value);
    }
  }
}
