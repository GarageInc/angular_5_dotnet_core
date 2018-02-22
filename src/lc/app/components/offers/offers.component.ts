import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from '../../../../common/services/authentication.service';
import { HeaderEnums } from '../../enums/header.enum';
import { EventService } from '../../../../common/services/event-service/event.service';
import { Guid } from './../../../../common/models/guid';
import { FileUploader } from 'ng2-file-upload';

import { OfferEnums } from '../../enums/offers.enum';

@Component({
  templateUrl: 'offers.component.html',
  styleUrls: ['offers.component.scss']
})
export class OffersComponent implements OnInit {
  public uploader: FileUploader;

  public hasBaseDropZoneOver = false;

  constructor(
    private router: Router,
    private eventsService: EventService,
    private authService: AuthenticationService
  ) {}

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public get uploadUrl(): string {
    let guid = Guid.newGuid();
    guid = Math.floor(new Date().getTime() / 1000) + '_' + guid;
    return '/api/v1/lc/offers/upload?identifier=' + guid;
  }

  public upload(): void {
    this.uploader.setOptions(this.uploaderOptions);
    this.uploader.uploadAll();
  }

  public reinitUploader(): void {
    this.uploader.setOptions(this.uploaderOptions);
  }

  public get uploaderOptions(): any {
    return {
      url: this.uploadUrl,
      authToken: this.authService.bearerToken,
      removeAfterUpload: true
    };
  }

  public ngOnInit() {
    this.uploader = new FileUploader(this.uploaderOptions);
    this.uploader.onCompleteAll = () => {
      this.eventsService.broadcast(OfferEnums.Reload);
    };
  }
}
