import { Pipe, ChangeDetectorRef, PipeTransform } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Pipe({
  name: 'timeAgo',
  pure: false
})
export class TimeAgoPipe implements PipeTransform {
  timer: Observable<string>;
  private months = [
    'января',
    'февраля',
    'марта',
    'апреля',
    'мая',
    'июня',
    'июля',
    'августа',
    'сентября',
    'октября',
    'ноября',
    'декабря'
  ];

  constructor() {}

  transform(time: number): any {
    let result: string;
    // current time
    const now = new Date();
    now.setHours(0);
    now.setMinutes(0);
    now.setSeconds(0);

    const nowTrimmedTime = now.getTime() / 1000;
    // time since message was sent in seconds
    const delta = Math.floor(nowTrimmedTime - time);

    // format string
    if (delta < 0) {
      // sent more than one day ago
      result = 'Сегодня';
    } else if (delta <= 86400) {
      result = 'Вчера';
    } else {
      const date = new Date(time * 1000);
      result = date.getDate() + ' ' + this.getMonthLabel(date.getMonth());
    }

    return result;
  }

  public getMonthLabel(monthNumber): string {
    return this.months[monthNumber];
  }
}
