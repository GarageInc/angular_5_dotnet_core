import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegionHelperComponent } from './region-helper.component';

describe('RegionHelperComponent', () => {
  let component: RegionHelperComponent;
  let fixture: ComponentFixture<RegionHelperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegionHelperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegionHelperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
