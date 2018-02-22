import { Injectable, Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'joinBy'
})
@Injectable()
export class JoinPipe implements PipeTransform {
  transform(items: Array<any>, field: string, key: string): any {
    const values = [] as Array<any>;

    for (let i = 0; i < items.length; i++) {
      if (key) {
        const subObject = items[i][key];
        if (subObject) {
          values.push(subObject[field]);
        }
      } else {
        values.push(items[i][field]);
      }
    }
    return values.join(', ');
  }
}
