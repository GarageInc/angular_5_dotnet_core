import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrencyHelperComponent } from './currency-helper.component';

describe('CurrencyHelperComponent', () => {
  let component: CurrencyHelperComponent;
  let fixture: ComponentFixture<CurrencyHelperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CurrencyHelperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CurrencyHelperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
