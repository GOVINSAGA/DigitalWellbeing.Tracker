import { TestBed } from '@angular/core/testing';

import { Usage } from './usage';

describe('Usage', () => {
  let service: Usage;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Usage);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
