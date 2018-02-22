import { Injectable, Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'decline'
})
@Injectable()
export class DeclinePipe implements PipeTransform {
  transform(number: number, one: string, two: string, many: string): any {
    const titles = [one, two, many];
    const cases = [2, 0, 1, 1, 1, 2];
    return titles[
      number % 100 > 4 && number % 100 < 20
        ? 2
        : cases[number % 10 < 5 ? number % 10 : 5]
    ];
  }
}
