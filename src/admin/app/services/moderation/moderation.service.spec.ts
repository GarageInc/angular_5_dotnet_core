import { TestBed, inject } from '@angular/core/testing';

import { ModerationService } from './moderation.service';

describe('ModerationService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ModerationService]
    });
  });

  it('should be created', inject([ModerationService], (service: ModerationService) => {
    expect(service).toBeTruthy();
  }));
});
