import { Injectable } from '@angular/core';
import { ConfigService } from '../services/config.service';

@Injectable()
export class BaseHub {

  protected readonly baseUrl: string;

  constructor() {
    this.baseUrl = ConfigService.options.apiEndpoint;
  }

}
