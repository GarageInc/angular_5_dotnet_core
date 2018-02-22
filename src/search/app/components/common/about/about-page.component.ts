import { Component, OnInit } from '@angular/core';
import { SeoAdapterService } from './../../../services/seo-adapter/seo-adapter.service';

@Component({
  selector: 'app-about-page',
  templateUrl: './about-page.component.html'
})
export class AboutPageComponent implements OnInit {
  public constructor(private seoAdapter: SeoAdapterService) {}

  public ngOnInit() {
    this.seoAdapter.setAboutPageSeo();
  }
}
