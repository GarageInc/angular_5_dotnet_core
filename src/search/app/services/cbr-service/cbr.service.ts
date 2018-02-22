import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class CbrService {
  public constructor(private httpClient: HttpClient) {}

  public courses: Array<any>;
  public parsed: any = {};

  private USD_NAME = 'USD';
  private EUR_NAME = 'EUR';

  public loadCourses(): void {
    this.httpClient.get('/files/cbr/XML_daily.json').subscribe((data: any) => {
      this.courses = data.ValCurs.Valute;
      this.parseValute(this.USD_NAME);
      this.parseValute(this.EUR_NAME);
    });
  }

  public parseValute(name: string): void {
    this.parsed[name] = this.courses.find(
      x => x.CharCode['#text'] === name
    ).Value['#text'];
  }

  public get usdCourse(): number {
    return Number.parseFloat(this.parsed[this.USD_NAME]);
  }

  public get euroCourse(): number {
    return Number.parseFloat(this.parsed[this.EUR_NAME]);
  }
}
