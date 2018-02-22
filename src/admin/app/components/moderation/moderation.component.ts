import { Component, OnInit } from '@angular/core';
import { ModerationService } from '../../services/moderation/moderation.service';
import { ModerationItem } from '../../models/moderation-item';
import { ChangeEvent } from 'angular2-virtual-scroll';
import { MatDialog } from '@angular/material';
import { PartModerationComponent } from './part-moderation/part-moderation.component';
import { EventService } from '../../../../common/services/event-service/event.service';
import { ModerationActions } from '../../enums/moderation-actions.enum';
import { SeoAdminBackendService } from '../../services/seo/seo-admin-backend.service';
import { ModerationFilter } from './../../models/moderation-filter';
import { FilterVirtualScrollComponent } from '../../../../common/components/virtual-scroll/filter-virtual-scroll.component';
import { BaseFilter } from '../../../../common/models/base-filter';

@Component({
  selector: 'app-moderation',
  templateUrl: './moderation.component.html',
  styleUrls: ['./moderation.component.scss']
})
export class ModerationComponent extends FilterVirtualScrollComponent<
  ModerationFilter,
  ModerationItem
> implements OnInit {
  constructor(
    public moderation: ModerationService,
    public dialog: MatDialog,
    public events: EventService,
    public seoService: SeoAdminBackendService
  ) {
    super(moderation);
  }

  ngOnInit() {
    this.load();

    this.events.subscribe(
      ModerationActions.RemovePart,
      (item: ModerationItem) => {
        if (confirm('Уверены? Точно?')) {
          this.moderation.delete(item).subscribe(() => {
            this.loadItem(item.id);
          });
        }
      }
    );

    this.events.subscribe(
      ModerationActions.RestorePart,
      (item: ModerationItem) => {
        this.moderation.restore(item).subscribe(() => {
          this.loadItem(item.id);
        });
      }
    );
  }

  public clone(item: ModerationItem): ModerationItem {
    return JSON.parse(JSON.stringify(item));
  }

  public show(item: ModerationItem) {
    const data = this.clone(item);
    const dialogRef = this.dialog.open(PartModerationComponent, {
      data: data,
      width: '1400px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (data.catalogItemId && data.seo) {
        this.seoService.updateForPart(data.seo, data.catalogItemId).subscribe();
      }

      this.moderation.update(data).subscribe(() => {
        this.loadItem(data.id);
      });
    });
  }

  public loadItem(id: number): void {
    this.moderation
      .needModerationItem(id)
      .map(data => {
        this.replaceIn(data, this.buffer);
        this.replaceIn(data, this.scrollItems);
      })
      .subscribe();
  }

  public replaceIn(data: ModerationItem, buffer: ModerationItem[]) {
    for (let i = 0; i < buffer.length; i++) {
      if (+buffer[i].id === +data.id) {
        buffer[i] = data;
        break;
      }
    }

    this.buffer.push(data);
  }

  public delete(item: ModerationItem): void {
    this.events.broadcast(ModerationActions.RemovePart, item);
  }
}
