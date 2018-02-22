import {
  NgModule,
  PLATFORM_ID,
  APP_ID,
  Inject,
  ErrorHandler
} from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import {
  BrowserModule,
  BrowserTransferStateModule
} from '@angular/platform-browser';
import { HttpClient } from '@angular/common/http/src/client';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AppHeaderComponent } from './app/app-header/app-header.component';
import { AppFooterComponent } from './app/app-footer/app-footer.component';
import { AppSearchComponent } from './app/app-search/app-search.component';
import { SearchResultComponent } from './components/search/search-result/search-result.component';
import { DistributorDetailsComponent } from './components/distributor/distributor-details/distributor-details.component';
import { SearchResultsComponent } from './components/search/search-results/search-results.component';
import { ProductSearchItemComponent } from './components/search/product-search-item/product-search-item.component';
import { ProductSearchItemsComponent } from './components/search/product-search-items/product-search-items.component';
import { NotFoundPageComponent } from './components/common/not-found-page.component';
import { EmptyPageComponent } from './components/common/empty-page.component';

import { ClarifySearchComponent } from './components/search/clarify-search/clarify-search.component';
import { EventService } from './../../common/services/event-service/event.service';
import { SearchDataService } from './services/search-data-service/search-data.service';

import { SearchBackendService } from './services/backend-service/search-backend.service';
import { SeoBackendService } from './services/seo-service/seo-backend.service';
import { CurrencyHelperComponent } from './app/desctop/currency-helper/currency-helper.component';
import { RegionHelperComponent } from './app/desctop/region-helper/region-helper.component';
import { LoginButtonComponent } from './app/desctop/login-button/login-button.component';
import { DesctopHelperComponent } from './app/desctop/desctop-helper/desctop-helper.component';
import { MobileHelperComponent } from './app/mobile/mobile-helper/mobile-helper.component';

import { AsyncPipe } from '@angular/common/src/pipes/async_pipe';
import { CommonModule } from '@angular/common/src/common_module';
import { Routes, RouterModule } from '@angular/router';
import { JoinPipe } from './../../common/pipes/join.pipe';
import { DeclinePipe } from './pipes/decline.pipe';
import { TimeAgoPipe } from './pipes/time-ago.pipe';
import { SeoAdapterService } from './services/seo-adapter/seo-adapter.service';
import { SeoService } from './services/seo-service/seo.service';
import { NavigatorService } from './services/navigator-service/navigator.service';
import { CbrService } from './services/cbr-service/cbr.service';
import { ProducerCodeService } from './services/producer-code/producer-code.service';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { PhonePipe } from './../../common/pipes/phone.pipe';
import { AboutPageComponent } from './components/common/about/about-page.component';
import { MyErrorHandler } from './../../common/services/error-handler';

import { LocalStorageService } from './services/local-storage/local-storage.service';
import { SearchTripAndTricksService } from './services/search-data-service/search-trip-and-tricks.service';
import { NguiAutoCompleteModule } from '@ngui/auto-complete';
import { isPlatformBrowser } from '@angular/common';

// определение маршрутов
const appRoutes: Routes = [
  {
    path: '',
    component: EmptyPageComponent,
    data: { title: 'Печкин — поиск запчастей котельного оборудования' }
  },
  {
    path: 'search',
    component: AppSearchComponent
  },
  {
    path: 'parts/:producer/:art',
    component: SearchResultsComponent
  },
  {
    path: 'firms/:company',
    component: DistributorDetailsComponent,
    data: { title: 'Печкин — поиск запчастей котельного оборудования' }
  },
  { path: 'about', component: AboutPageComponent },
  { path: '**', redirectTo: '/about' } // name: '404',
  // { path: '**', redirectTo: '404' }
];

@NgModule({
  declarations: [
    AppComponent,
    AppHeaderComponent,
    AppFooterComponent,
    AppSearchComponent,
    SearchResultComponent,
    DistributorDetailsComponent,
    SearchResultsComponent,
    ProductSearchItemsComponent,
    ProductSearchItemComponent,
    ClarifySearchComponent,
    CurrencyHelperComponent,
    RegionHelperComponent,
    LoginButtonComponent,
    DesctopHelperComponent,
    MobileHelperComponent,
    NotFoundPageComponent,
    EmptyPageComponent,
    AboutPageComponent,

    JoinPipe,
    TimeAgoPipe,
    DeclinePipe,
    PhonePipe
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'pechk' }),
    // CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes),
    InfiniteScrollModule,
    NguiAutoCompleteModule,
    BrowserTransferStateModule
  ],
  providers: [
    EventService,
    SearchBackendService,
    SeoBackendService,
    SeoService,
    SeoAdapterService,
    NavigatorService,
    CbrService,
    ProducerCodeService,
    SearchDataService,
    LocalStorageService,
    SearchTripAndTricksService,
    { provide: ErrorHandler, useClass: MyErrorHandler },
    { provide: 'LOCALSTORAGE', useFactory: getLocalStorage }
  ],
  bootstrap: [AppComponent],
  exports: [JoinPipe, TimeAgoPipe, DeclinePipe]
})
export class AppModule {
  constructor(
    @Inject(PLATFORM_ID) private platformId: Object,
    @Inject(APP_ID) private appId: string
  ) {
    const platform = isPlatformBrowser(platformId)
      ? 'in the browser'
      : 'on the server';
    console.log(`Running ${platform} with appId=${appId}`);
  }
}

export function getLocalStorage() {
  return typeof window !== 'undefined' ? window.localStorage : null;
}
