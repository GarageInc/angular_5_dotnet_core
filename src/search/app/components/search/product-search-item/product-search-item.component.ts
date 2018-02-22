import { Component, OnInit, Input } from '@angular/core';
import { ProducerItemSearchModel } from '../../../models/api/producer-item-search-model';
import { NavigatorService } from '../../../services/navigator-service/navigator.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'product-search-item',
  templateUrl: './product-search-item.component.html',
  styleUrls: ['./product-search-item.component.scss']
})
export class ProductSearchItemComponent implements OnInit {
  @Input() item: ProducerItemSearchModel;

  constructor(private navigator: NavigatorService) {}

  ngOnInit() {}

  public goToSearch(item: ProducerItemSearchModel) {
    this.navigator.goToSearch(item.producerCode);
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
}
