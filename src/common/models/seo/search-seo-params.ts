import { SeoModel } from './seo-model';

export class SearchSeoParams {
  main: SeoModel;
  suppliers: SeoModel;
  parts: SeoModel;
  clarify: SeoModel;
  about: SeoModel;

  constructor() {
    this.about = new SeoModel();
    this.main = new SeoModel();
    this.suppliers = new SeoModel();
    this.parts = new SeoModel();
    this.clarify = new SeoModel();
  }
}
