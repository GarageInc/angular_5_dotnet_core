import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ModerationItem } from '../../../models/moderation-item';
import { SeoParameter } from '../../../../../search/app/models/api/seo-parameter';
import { ModerationService } from '../../../services/moderation/moderation.service';
import { PartProducer } from '../../../models/part-producer';
import { EventService } from '../../../../../common/services/event-service/event.service';
import { ModerationActions } from '../../../enums/moderation-actions.enum';
import { PartSupplier } from '../../../models/part-supplier';
import { debounce } from 'rxjs/operators/debounce';
import { FileUploader } from 'ng2-file-upload';
import { AuthenticationService } from '../../../../../common/services/authentication.service';
import { FilterVirtualScrollComponent } from '../../../../../common/components/virtual-scroll/filter-virtual-scroll.component';

import { SeoAdminBackendService } from '../../../services/seo/seo-admin-backend.service';
import { OnDestroy } from '@angular/core';
import { SupplierMatchModel } from '../../../models/supplier-match-model';
import { ModerationFilter } from '../../../models/moderation-filter';

@Component({
  selector: 'app-part-moderation',
  templateUrl: './part-moderation.component.html',
  styleUrls: ['./part-moderation.component.scss']
})
export class PartModerationComponent implements OnInit {
  public item: ModerationItem = new ModerationItem();
  public producers: PartProducer[] = [];
  public suppliers: PartSupplier[] = [];
  public uploader: FileUploader;
  public suppliersMatch: SupplierMatchModel[] = [];
  public canShowMatches = false;
  public imageUrl = '';

  constructor(
    public thisDialogRef: MatDialogRef<PartModerationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ModerationItem,
    private authService: AuthenticationService,
    private events: EventService,
    public moderationService: ModerationService,
    public seoService: SeoAdminBackendService
  ) {
    this.setItem(data);
  }

  public setItem(data: ModerationItem) {
    this.setSeo(data);
    this.item = data;
    this.setImagePath();
  }

  public get uploadUrl(): string {
    return (
      '/api/v1/moderation/upload-catalog-item-image?producerCode=' +
      this.item.producerCode +
      '&producerId=' +
      this.item.producerId
    );
  }

  public showSupplierPrices(): void {
    this.moderationService
      .getSuppliesMatchFor(this.item.producerCode)
      .subscribe(data => {
        this.suppliersMatch = data;
        this.canShowMatches = true;
      });
  }

  public loadSeo(): void {
    if (this.item.seoParameterId) {
      this.seoService
        .getSeo(this.item.seoParameterId)
        .map((data: SeoParameter) => (this.item.seo = data))
        .subscribe();
    }
  }

  public setImagePath() {
    this.imageUrl =
      'files/producers/items/' +
      this.item.producerId +
      '/' +
      this.item.producerCode.trim() +
      '.jpg' +
      '?random+=' +
      Math.random();
  }

  public upload(): void {
    if (this.item.producerId) {
      this.uploader.uploadAll();
    }

    setTimeout(() => {
      this.setImagePath();
    }, 3000);
  }

  public loadSuppliers(): void {
    this.moderationService
      .suppliersFor(this.item.id)
      .map(data => {
        this.suppliers = data;
        this.setSupplierData();
      })
      .subscribe();
  }

  public setSupplierData(): void {
    const supplier = this.suppliers.find(x => +x.id === +this.item.supplierId);

    if (supplier) {
      this.item.ruName = supplier.partName;
    }
  }

  public loadProducers(): void {
    this.moderationService
      .producers()
      .map(data => (this.producers = data))
      .subscribe();
  }

  public setSeo(data: ModerationItem): void {
    if (!data.seo) {
      data.seo = new SeoParameter();
    }
  }

  public remove(): void {
    this.item.isDeleted = true;
    this.events.broadcast(ModerationActions.RemovePart, this.item);
  }

  public restore(): void {
    this.item.isDeleted = false;
    this.events.broadcast(ModerationActions.RestorePart, this.item);
  }

  public createCatalogItem(): void {
    this.moderationService.createCatalogItem(this.item).subscribe(() => {
      this.moderationService
        .needModerationItem(this.item.id)
        .subscribe(data => {
          this.setItem(data);
        });
    });
  }

  public selectSupplierMatch(match: SupplierMatchModel): void {
    this.item.ruName = match.name;
    this.item.supplierId = match.supplierId;
  }

  public closeMatches(): void {
    this.canShowMatches = false;
  }

  ngOnInit() {
    this.uploader = new FileUploader({
      url: this.uploadUrl,
      authToken: this.authService.bearerToken
    });

    this.loadSeo();
    this.loadSuppliers();
    this.loadProducers();
  }
}
