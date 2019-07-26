import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { GarbageCollector } from '../utils';
import { NotificationService } from './notification.service';
import { LoggerService } from './logger.service';

@Injectable()
export class TokenService extends BaseService {

  private _isInitialized: boolean;
  private _refreshRetryCount = 0;

  constructor(gc: GarbageCollector, notification: NotificationService, logger: LoggerService) {
    super(gc, notification, logger);

    this._isInitialized = false;

    gc.attach(function () {
      const that = this as TokenService;
      that.clear();
    }.bind(this));
  }

  private clear(): void {
    this._isInitialized = false;
  }

  public getAccessTokenExpiration(): number {
    return 0;
  }

  public refreshToken(): Promise<void> {
    return new Promise<void>(async (resolve, reject) => {
      try {
        resolve();
      } catch (error) {
        reject(error);
      }
    });
  }

  public loadUserProfile(): Promise<void> {
    return new Promise<void>(async (resolve, reject) => {
      try {
        resolve();
      } catch (error) {
        reject(error);
      }
    });
  }
}
