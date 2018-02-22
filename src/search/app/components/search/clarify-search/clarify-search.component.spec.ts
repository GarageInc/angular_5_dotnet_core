import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClarifySearchComponent } from './clarify-search.component';

describe('ClarifySearchComponent', () => {
  let component: ClarifySearchComponent;
  let fixture: ComponentFixture<ClarifySearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClarifySearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClarifySearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
