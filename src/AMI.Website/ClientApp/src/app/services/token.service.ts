import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { NotificationService } from './notification.service';
import { LoggerService } from './logger.service';
import { TokenContainerModel } from '../clients/ami-api-client';
import { TokenProxy } from '../proxies/token.proxy';
import { GarbageCollector } from '../utils';

@Injectable()
export class TokenService extends BaseService {

  private _isInitialized: boolean;
  private _refreshRetryCount = 0;

  constructor(gc: GarbageCollector, notificationService: NotificationService, logger: LoggerService,
    private tokenProxy: TokenProxy) {
    super(gc, notificationService, logger);

    this._isInitialized = false;

    gc.attach(function () {
      const that = this as TokenService;
      that.clear();
    }.bind(this));
  }

  private clear(): void {
    this._isInitialized = false;
    this.clearLocalStorage();
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

  private setLocalStorage(container: TokenContainerModel) {
    if (container) {
      localStorage.setItem('tokenContainer', JSON.stringify(container));
    }
  }

  private readLocalStorage(): TokenContainerModel {
    try {
      return JSON.parse(localStorage.getItem('tokenContainer'));
    } catch (e) {
      return undefined;
    }
  }

  private clearLocalStorage(): void {
    localStorage.removeItem('tokenContainer');
  }
}
