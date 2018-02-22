import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './app-header.component.html',
  styleUrls: ['./app-header.component.scss']
})
export class AppHeaderComponent implements OnInit {
  constructor() {}

  @ViewChild('headerElement') el: ElementRef;

  ngOnInit() {}

  public onMobButtonClick(): void {
    this.el.nativeElement.classList.toggle('active');
    const params = document.querySelector('.header__params') || {
      classList: {
        toggle: (value: string) => {}
      }
    };

    params.classList.toggle('show');
  }
}
