import { Component, OnInit, Input } from '@angular/core';
import { ProducerItemSearchModel } from '../../../models/api/producer-item-search-model';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'product-search-items',
  templateUrl: './product-search-items.component.html',
  styleUrls: ['./product-search-items.component.scss']
})
export class ProductSearchItemsComponent implements OnInit {
  @Input() items: ProducerItemSearchModel[];

  constructor() {}

  ngOnInit() {}
}
