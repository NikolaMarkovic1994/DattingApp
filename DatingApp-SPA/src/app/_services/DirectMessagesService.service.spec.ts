/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { DirectMessagesServiceService } from './DirectMessagesService.service';

describe('Service: DirectMessagesService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DirectMessagesServiceService]
    });
  });

  it('should ...', inject([DirectMessagesServiceService], (service: DirectMessagesServiceService) => {
    expect(service).toBeTruthy();
  }));
});
