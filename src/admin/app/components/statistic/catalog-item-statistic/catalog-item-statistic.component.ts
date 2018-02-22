import { Component, OnInit } from '@angular/core';
import { FilterVirtualScrollComponent } from '../../../../../common/components/virtual-scroll/filter-virtual-scroll.component';
import { CatalogItemStatistic } from './../../../models/catalog-item-statistic';
import { CatalogItemStatisticFilter } from '../../../models/catalog-item-statistic-filter';
import { CatalogItemStatisticService } from '../../../services/statistic/catalog-item-statistic.service';

@Component({
  selector: 'app-catalog-item-statistic',
  templateUrl: './catalog-item-statistic.component.html',
  styleUrls: ['./catalog-item-statistic.component.scss']
})
export class CatalogItemStatisticComponent extends FilterVirtualScrollComponent<
  CatalogItemStatisticFilter,
  CatalogItemStatistic
> implements OnInit {
  public TYPE_CURRENT_WEEK = '0';
  public TYPE_CURRENT_MONTH = '1';
  public TYPE_CURRENT_YEAR = '2';

  public timeType = this.TYPE_CURRENT_MONTH;

  constructor(public catalogItemStatisticService: CatalogItemStatisticService) {
    super(catalogItemStatisticService);
  }

  public loadForTime(): void {
    let fromDate = new Date();

    switch (+this.timeType) {
      case +this.TYPE_CURRENT_WEEK: {
        fromDate = this.getMonday(new Date());
        break;
      }
      case +this.TYPE_CURRENT_MONTH: {
        fromDate = new Date(fromDate.getFullYear(), fromDate.getMonth(), 1);
        break;
      }
      case +this.TYPE_CURRENT_YEAR: {
        fromDate = new Date(fromDate.getFullYear(), 0, 1);
        break;
      }
    }

    this.filter.from = this.getTimestamp(fromDate);
    this.filter.to = this.getTimestamp(new Date());

    this.load();
  }

  private getTimestamp(from: Date): number {
    return Math.floor(from.getTime() / 1000);
  }

  private getMonday(d) {
    d = new Date(d);
    const day = d.getDay(),
      diff = d.getDate() - day + (+day === 0 ? -6 : 1); // adjust when day is sunday
    return new Date(d.setDate(diff));
  }

  public ngOnInit(): void {
    this.loadForTime();
  }
}
