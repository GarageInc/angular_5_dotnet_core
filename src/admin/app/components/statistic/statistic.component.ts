import { Component, OnInit } from '@angular/core';

import { User } from './../../../../common/models/user';
import { StatisticService } from './../../services/statistic/statistic.service';
import { Observable } from 'rxjs/Observable';
import { Statistic } from '../../models/statistic';

@Component({
  templateUrl: 'statistic.component.html',
  styleUrls: ['statistic.component.scss']
})
export class StatisticComponent implements OnInit {
  constructor(private statisticService: StatisticService) {}

  public statistic: Statistic = new Statistic();

  ngOnInit() {
    this.statisticService
      .getFull()
      .map(data => {
        this.statistic = data;
      })
      .subscribe();
  }
}
