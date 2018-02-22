import { Injectable, Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'phone'
})
@Injectable()
export class PhonePipe implements PipeTransform {
  transform(val: string, args) {
    let newStr = '';

    if (val.length > 6) {
      const isMobile = val[1] === '9';
      if (isMobile) {
        return this.parseAsMobilePhone(val);
      } else {
        return this.parseAsCityPhone(val);
      }
    } else {
      let i = 0;
      for (i = 0; i < Math.floor(val.length / 2); i++) {
        newStr = newStr + val.substr(i * 2, 2) + '–';
      }
      return newStr + val.substr(i * 2);
    }
  }

  private parseAsCityPhone(val: string): string {
    let newStr = '';
    newStr += '+' + val.slice(0, 1);
    newStr += ' ';
    newStr += val.slice(1, 6);
    newStr += ' ';
    newStr += val.slice(6, 7);
    newStr += '–';
    newStr += val.slice(7, 9);
    newStr += '–';
    newStr += val.slice(9, 11);
    return newStr;
  }

  private parseAsMobilePhone(val: string): string {
    let newStr = '';
    newStr += '+' + val.slice(0, 1);
    newStr += ' ';
    newStr += val.slice(1, 4);
    newStr += ' ';
    newStr += val.slice(4, 7);
    newStr += '–';
    newStr += val.slice(7, 9);
    newStr += '–';
    newStr += val.slice(9, 11);
    return newStr;
  }
}
