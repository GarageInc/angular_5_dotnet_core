import {
  Component,
  OnInit,
  ChangeDetectorRef,
  Inject,
  PLATFORM_ID
} from '@angular/core';
import { Http } from '@angular/http';
import { EventService } from './../../common/services/event-service/event.service';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { OnChanges, SimpleChanges, OnDestroy } from '@angular/core';
import { Title, DomSanitizer, SafeHtml } from '@angular/platform-browser';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/mergeMap';
import { SeoAdapterService } from './services/seo-adapter/seo-adapter.service';
import { NavigatorService } from './services/navigator-service/navigator.service';
import { Subscription } from 'rxjs/Subscription';
import { SearchEvents } from './enums/search-events';
import { CbrService } from './services/cbr-service/cbr.service';
import { SearchTripAndTrick } from './models/search-trip-and-trick';
import { SearchTripAndTricksService } from './services/search-data-service/search-trip-and-tricks.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnChanges, OnDestroy {
  public myForm: FormGroup;
  public isServer = true;

  constructor(
    @Inject(PLATFORM_ID) private platformId: Object,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private seoAdapter: SeoAdapterService,
    private eventsService: EventService,
    private navigator: NavigatorService,
    private cbr: CbrService,
    private cdr: ChangeDetectorRef,
    private tripAndTricks: SearchTripAndTricksService,
    private builder: FormBuilder,
    private _sanitizer: DomSanitizer
  ) {
    this.isServer = !isPlatformBrowser(platformId);
  }

  public get isMainPage(): boolean {
    return this.router.url === '/' || this.router.url === '/admin';
  }

  private subscription: Subscription;

  public get tripsAndTricks(): Array<string> {
    const data = this.tripAndTricks.list();
    return data && data.map ? data.map(x => x.producerCode) : [];
  }

  public handleKeyDown(event: any): void {
    if (+event.keyCode === 13) {
      this.search();
    }
  }

  public autocompleSeachFormatter = (data: string): SafeHtml => {
    // const html = `<span style='color:red'>${data}</span>`;
    const htmlResult = data.replace(
      new RegExp(this.searchString, 'g'),
      `<span style='color:red'>${this.searchString}</span>`
    );
    return this._sanitizer.bypassSecurityTrustHtml(htmlResult);
    // tslint:disable-next-line:semicolon
  };

  ngOnInit() {
    this.myForm = this.builder.group({
      searchString: ''
    });

    this.seoAdapter.setMainSeo();

    this.subscription = this.activatedRoute.queryParams.subscribe(params => {
      this.searchString = params['art'];

      if (this.searchString) {
        this.seoAdapter.setClarifySeo(this.searchString);
      }
    });

    this.checkFlags();

    this.router.events
      .filter(event => event instanceof NavigationEnd)
      .map(() => this.activatedRoute)
      .map(route => {
        // tslint:disable-next-line:curly
        while (route.firstChild) route = route.firstChild;
        return route;
      })
      .filter(route => route.outlet === 'primary')
      .mergeMap(route => route.data)
      .subscribe(event => {
        const title = event['title'];
        if (title) {
          this.seoAdapter.setTitle(title);
        }
      });

    this.eventsService.subscribe(
      SearchEvents.SetSearchString,
      (data: string) => {
        if (this.searchString !== data) {
          this.searchString = data;
          this.cdr.detectChanges();
        }
      }
    );

    this.cbr.loadCourses();
  }

  public get searchString(): string {
    return this.myForm.value.searchString;
  }

  public set searchString(value: string) {
    this.myForm.value.searchString = value;
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    this.eventsService.unsubscribeFromGroup(SearchEvents.SetSearchString);
  }

  ngOnChanges(chages: SimpleChanges) {
    this.checkFlags();
  }

  private checkFlags(): void {}

  public search(): void {
    if (this.isValidSearchString()) {
      this.navigator.goToSearch(this.searchString).then(() => {
        this.checkFlags();
      });
    } else {
      //      this.navigator.goToMain().then(() => {
      //      this.checkFlags();
      //  });
    }
  }

  public isValidSearchString(): boolean {
    return !!this.searchString && this.searchString.length >= 4;
  }
}
