import { Injectable } from '@angular/core';

@Injectable()
export class ProducerCodeService {
  constructor() {}

  public compare(one: string, second: string): boolean {
    const identical = one === second;
    const isStartWithZero = one[0] === '0' || second[0] === '0';
    return (
      identical ||
      (isStartWithZero && this.trimStart(second, '0') === one) ||
      (isStartWithZero && this.trimStart(one, '0') === second)
    );
  }

  trimStart(string, c) {
    if (string.length === 0) {
      return string;
    }
    c = c ? c : ' ';
    let i = 0;
    const val = 0;
    for (; string.charAt(i) === c && i < string.length; i++) {}
    return string.substring(i);
  }

  trimEnd(string, c) {
    c = c ? c : ' ';
    let i = string.length - 1;
    for (; i >= 0 && string.charAt(i) === c; i--) {}
    return string.substring(0, i + 1);
  }
  trim(string, c) {
    return string.trimStart(c).trimEnd(c);
  }
  leadingUpper(string) {
    let result = '';
    if (string.length > 0) {
      result += this[0].toUpperCase();
      if (string.length > 1) {
        result += string.substring(1, string.length);
      }
    }
    return result;
  }
}
