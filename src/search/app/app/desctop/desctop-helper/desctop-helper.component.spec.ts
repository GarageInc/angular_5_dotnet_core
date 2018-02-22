import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DesctopHelperComponent } from './desctop-helper.component';

describe('DesctopHelperComponent', () => {
  let component: DesctopHelperComponent;
  let fixture: ComponentFixture<DesctopHelperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DesctopHelperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DesctopHelperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
