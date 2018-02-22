import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MobileHelperComponent } from './mobile-helper.component';

describe('MobileHelperComponent', () => {
  let component: MobileHelperComponent;
  let fixture: ComponentFixture<MobileHelperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MobileHelperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MobileHelperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
