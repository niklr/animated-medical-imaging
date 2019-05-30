import { Injectable } from '@angular/core';
import { AppInfoAmiApiClient, AppInfo } from '../clients/ami-api-client';
import { BaseProxy } from './base.proxy';
import { ConfigService } from '../services/config.service';

@Injectable()
export class AppProxy extends BaseProxy {

  constructor(private appInfoClient: AppInfoAmiApiClient) {
    super();
  }

  public init(retryCounter: number): void {
    if (!retryCounter || retryCounter < 0) {
      retryCounter = 0;
    }
    this.appInfoClient.get().subscribe((result: AppInfo) => {
      ConfigService.settings.version = result.appVersion;
    }, error => {
      if (retryCounter <= 3) {
        setTimeout(() => {
          console.log('AppProxy.init retryCounter: ' + retryCounter);
          this.init(++retryCounter);
        }, 5000);
      }
    });
  }
}
