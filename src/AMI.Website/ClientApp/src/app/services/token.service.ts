import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { NotificationService } from './notification.service';
import { LoggerService } from './logger.service';
import { TokenContainerModel } from '../clients/ami-api-client';
import { TokenProxy } from '../proxies/token.proxy';
import { TokenStore } from '../stores';
import { GarbageCollector } from '../utils';

@Injectable()
export class TokenService extends BaseService {

  private container: TokenContainerModel;
  private refreshingPromise: Promise<void>;

  constructor(gc: GarbageCollector, notificationService: NotificationService, logger: LoggerService,
              private tokenStore: TokenStore, private tokenProxy: TokenProxy) {
    super(gc, notificationService, logger);

    gc.attach(() => {
      this.clear();
    });
  }

  private clear(): void {
    this.logger.info('TokenService.clear() called');
    this.container = undefined;
    this.tokenStore.clear();
  }

  public get isExpired(): boolean {
    return this.tokenStore.isExpired;
  }

  public getAccessToken(): string {
    return this.tokenStore.getAccessToken();
  }

  public getIdentityClaims(): object {
    return this.tokenStore.getIdentityClaims();
  }

  public getRefreshToken(): string {
    return this.tokenStore.getRefreshToken();
  }

  public getAccessTokenExpiration(): number {
    this.loadTokenContainer();
    if (this.container && this.container.accessToken) {
      return this.container.accessToken.exp;
    } else {
      return 0;
    }
  }

  private loadTokenContainer(): void {
    if (!this.container) {
      this.container = this.tokenStore.loadTokenContainer();
    }
  }

  private setTokenContainer(container: TokenContainerModel): void {
    this.logger.info(container);
    if (container) {
      this.container = container;
      this.tokenStore.setTokenContainer(container);
    } else {
      throw new Error('TokenService.setTokenContainer failed.');
    }
  }

  public fetchTokenUsingAnonymousFlow(): Promise<void> {
    return new Promise<void>(async (resolve, reject) => {
      try {
        this.loadTokenContainer();
        if (this.container && this.container.idToken && this.container.idToken.isAnon) {
          // Use existing token that is an anonymous user otherwise fetch new token
          resolve();
        } else {
          this.tokenProxy.createAnon().subscribe(result => {
            this.setTokenContainer(result);
            resolve();
          }, error => {
            reject(error);
          });
        }
      } catch (error) {
        reject(error);
      }
    });
  }

  public fetchTokenUsingPasswordFlow(username: string, password: string): Promise<void> {
    return new Promise<void>(async (resolve, reject) => {
      try {
        this.tokenProxy.create(username, password).subscribe(result => {
          this.setTokenContainer(result);
          resolve();
        }, error => {
          reject(error);
        });
      } catch (error) {
        reject(error);
      }
    });
  }

  public refreshToken(): Promise<void> {
    this.logger.info('TokenService.refreshToken isRefreshing: ' + !!this.refreshingPromise);
    if (!!this.refreshingPromise) {
      return this.refreshingPromise;
    } else {
      this.refreshingPromise = new Promise<void>(async (resolve, reject) => {
        try {
          this.loadTokenContainer();
          if (this.container) {
            this.tokenProxy.update(this.container).subscribe(result => {
              this.setTokenContainer(result);
              resolve();
            }, error => {
              reject(error);
            });
          } else {
            throw new Error('TokenService.refreshToken failed.');
          }
        } catch (error) {
          reject(error);
        }
      }).finally(() => {
        this.refreshingPromise = undefined;
      });
      return this.refreshingPromise;
    }
  }

  public loadUserProfile(): Promise<void> {
    return new Promise<void>(async (resolve, reject) => {
      try {
        this.container = this.tokenStore.loadTokenContainer();
        resolve();
      } catch (error) {
        reject(error);
      }
    });
  }
}
