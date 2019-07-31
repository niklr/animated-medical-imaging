import { Injectable } from '@angular/core';
import { AppInfoAmiApiClient, AppInfo } from '../clients/ami-api-client';
import { BaseProxy } from './base.proxy';
import { AuthService } from '../services/auth.service';
import { ConfigService } from '../services/config.service';
import { LoggerService } from '../services/logger.service';

@Injectable()
export class AppProxy extends BaseProxy {

  constructor(authService: AuthService, private appInfoClient: AppInfoAmiApiClient, private logger: LoggerService) {
    super(authService);
  }

  public init(retryCounter: number): void {
    if (!retryCounter || retryCounter < 0) {
      retryCounter = 0;
    }
    super.preflight().then(() => {
      this.appInfoClient.get().subscribe((result: AppInfo) => {
        ConfigService.options.version = result.appVersion;
      }, error => {
        if (retryCounter <= 3) {
          setTimeout(() => {
            this.logger.info('AppProxy.init retryCounter: ' + retryCounter);
            this.init(++retryCounter);
          }, 5000);
        }
      });
    });

  }
}
