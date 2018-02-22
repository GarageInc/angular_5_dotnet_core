import { TestBed, inject } from '@angular/core/testing';

import { SearchBackendService } from './search-backend.service';

describe('SearchBackendService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SearchBackendService]
    });
  });

  it('should be created', inject([SearchBackendService], (service: SearchBackendService) => {
    expect(service).toBeTruthy();
  }));
});
