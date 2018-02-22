import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule, Http, BaseRequestOptions } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient } from '@angular/common/http/src/client';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';

import { JoinPipe } from './../../common/pipes/join.pipe';

import { routing } from './app.routing';
import { LoginComponent } from './components/login/login.component';
import { AuthGuard } from './guards/auth.guard';
import { AuthenticationService } from './../../common/services/authentication.service';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { EventService } from './../../common/services/event-service/event.service';
import { OffersComponent } from './components/offers/offers.component';

import { AuthenticatedHttpService } from './services/authenticated-http.service';
import { VirtualScrollModule } from 'angular2-virtual-scroll';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FileUploadModule } from 'ng2-file-upload';
import { SupplierOffersService } from './services/supplier-offers/supplier-offers.service';
import { LcNavigationService } from './services/navigation/lc-navigation.service';

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
import { OffersListComponent } from './components/offers-list/offers-list.component';

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
    HeaderComponent,
    FooterComponent,

    OffersComponent,
    OffersListComponent,

    JoinPipe
  ],
  entryComponents: [],
  providers: [
    AuthGuard,
    AuthenticationService,
    { provide: Http, useClass: AuthenticatedHttpService },
    EventService,
    LcNavigationService,
    SupplierOffersService,

    // MatDialogTitle,
    MatGridTile,
    MatCardImage,
    MatRadioGroup,
    MatRadioButton,

    BaseRequestOptions
  ],
  bootstrap: [AppComponent],
  exports: [JoinPipe]
})
export class AppModule {}
