import { TestBed, inject } from '@angular/core/testing';

import { AdminNavigationService } from './admin-navigation.service';

describe('AdminNavigationService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AdminNavigationService]
    });
  });

  it('should be created', inject([AdminNavigationService], (service: AdminNavigationService) => {
    expect(service).toBeTruthy();
  }));
});
