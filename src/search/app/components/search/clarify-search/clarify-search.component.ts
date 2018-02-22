import { Component, OnInit, Input } from '@angular/core';
import { ProducerItemSearchModel } from '../../../models/api/producer-item-search-model';
import { ChangeEvent } from 'angular2-virtual-scroll';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import { OnChanges, SimpleChanges } from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'clarify-search',
  templateUrl: './clarify-search.component.html',
  styleUrls: ['./clarify-search.component.scss']
})
export class ClarifySearchComponent implements OnInit, OnChanges {
  @Input() items: ProducerItemSearchModel[];

  public array: ProducerItemSearchModel[] = [];

  public sum = 100;
  public throttle = 300;
  public scrollDistance = 1;
  public scrollUpDistance = 2;
  private countOnPage = 10;

  constructor() {}

  ngOnInit() {
    this.onScrollDown();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.items) {
      this.array = [];
      this.onScrollDown();
    }
  }

  getImageUrl(item: ProducerItemSearchModel): string {
    return (
      'files/producers/items/' +
      item.producerId +
      '/' +
      item.producerCode +
      '.jpg'
    );
  }

  onScrollDown() {
    const sliceResult = this.items.slice(
      this.array.length,
      this.array.length + this.countOnPage
    );
    this.array = this.array.concat(sliceResult);
  }
}
