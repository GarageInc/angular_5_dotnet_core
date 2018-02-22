import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductSearchItemsComponent } from './product-search-items.component';

describe('ProductSearchItemComponent', () => {
  let component: ProductSearchItemsComponent;
  let fixture: ComponentFixture<ProductSearchItemsComponent>;

  beforeEach(
    async(() => {
      TestBed.configureTestingModule({
        declarations: [ProductSearchItemsComponent]
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductSearchItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
