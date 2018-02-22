import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule, Http, BaseRequestOptions } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient } from '@angular/common/http/src/client';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';

import { routing } from './app.routing';
import { LoginComponent } from './components/login/login.component';
import { StatisticComponent } from './components/statistic/statistic.component';
import { AuthGuard } from './guards/auth.guard';
import { AuthenticationService } from './../../common/services/authentication.service';
import { UserService } from './services/user/user.service';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { StatisticService } from './services/statistic/statistic.service';
import { EventService } from './../../common/services/event-service/event.service';
import { AdminNavigationService } from './services/navigation/admin-navigation.service';
import { PagesComponent } from './components/pages/pages.component';
import { SuppliersComponent } from './components/suppliers/suppliers.component';
import { CatalogsComponent } from './components/catalogs/catalogs.component';
import { ProducersComponent } from './components/producers/producers.component';
import { ModerationComponent } from './components/moderation/moderation.component';
import { PartModerationComponent } from './components/moderation/part-moderation/part-moderation.component';

import { SeoComponent } from './components/seo/seo.component';
import { UsersComponent } from './components/users/users.component';
import { ModerationService } from './services/moderation/moderation.service';
import { CatalogItemStatisticService } from './services/statistic/catalog-item-statistic.service';
import { SeoAdminBackendService } from './services/seo/seo-admin-backend.service';
import { AuthenticatedHttpService } from './services/authenticated-http.service';
import { VirtualScrollModule } from 'angular2-virtual-scroll';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FileUploadModule } from 'ng2-file-upload';

import {
  MatButtonModule,
  MatCheckboxModule,
  MatInputModule,
  MatDialogModule,
  MatCardModule,
  MatTooltipModule,
  MatToolbarModule,
  MatDialogTitle,
  MatDialogContent,
  MatSelectModule,
  MatOptionModule,
  MatGridList,
  MatGridTile,
  MatGridListModule,
  MatCardImage,
  MatRadioGroup,
  MatRadioButton,
  MatRadioModule,
  MatIconModule
} from '@angular/material';
import { CatalogItemStatisticComponent } from './components/statistic/catalog-item-statistic/catalog-item-statistic.component';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    HttpClientModule,
    routing,
    VirtualScrollModule,
    BrowserAnimationsModule,

    MatTooltipModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatCardModule,

    MatDialogModule,

    MatSelectModule,
    MatOptionModule,
    MatGridListModule,

    MatRadioModule,
    MatIconModule,

    MatToolbarModule,
    FileUploadModule
  ],
  declarations: [
    AppComponent,
    LoginComponent,
    StatisticComponent,
    HeaderComponent,
    FooterComponent,
    PagesComponent,
    SuppliersComponent,
    CatalogsComponent,
    ProducersComponent,
    ModerationComponent,
    SeoComponent,
    UsersComponent,

    PartModerationComponent,
    CatalogItemStatisticComponent
  ],
  entryComponents: [PartModerationComponent],
  providers: [
    AuthGuard,
    AuthenticationService,
    UserService,
    StatisticService,
    ModerationService,
    CatalogItemStatisticService,
    SeoAdminBackendService,
    { provide: Http, useClass: AuthenticatedHttpService },
    EventService,
    AdminNavigationService,

    // MatDialogTitle,
    MatGridTile,
    MatCardImage,
    MatRadioGroup,
    MatRadioButton,

    BaseRequestOptions
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
