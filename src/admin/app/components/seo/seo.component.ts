import { Component, OnInit } from '@angular/core';
import { SeoAdminBackendService } from '../../services/seo/seo-admin-backend.service';
import { SearchSeoParams } from '../../../../common/models/seo/search-seo-params';
import { RobotsTxt } from './../../models/robots-txt';

@Component({
  selector: 'app-seo',
  templateUrl: './seo.component.html',
  styleUrls: ['./seo.component.scss']
})
export class SeoComponent implements OnInit {
  public searchSeoParams: SearchSeoParams = new SearchSeoParams();
  public robotsTxt: RobotsTxt = new RobotsTxt();

  constructor(private seoService: SeoAdminBackendService) {}

  ngOnInit() {
    this.reload();
  }

  public update(): void {
    this.seoService.update(this.searchSeoParams).subscribe();
    this.seoService.updateRobots(this.robotsTxt).subscribe();
  }

  public reload(): void {
    this.seoService
      .get()
      .map(x => (this.searchSeoParams = x))
      .subscribe();

    this.seoService.getRobots().subscribe(data => (this.robotsTxt = data));
  }
}
