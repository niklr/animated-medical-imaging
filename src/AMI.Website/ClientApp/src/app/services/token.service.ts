import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { NotificationService } from './notification.service';
import { LoggerService } from './logger.service';
import { TokenContainerModel } from '../clients/ami-api-client';
import { TokenProxy } from '../proxies/token.proxy';
import { GarbageCollector } from '../utils';

@Injectable()
export class TokenService extends BaseService {

  private container: TokenContainerModel;

  constructor(gc: GarbageCollector, notificationService: NotificationService, logger: LoggerService,
              private tokenProxy: TokenProxy) {
    super(gc, notificationService, logger);

    gc.attach(() => {
      this.clear();
    });
  }

  private clear(): void {
    this.logger.info('TokenService.clear() called');
    this.clearLocalStorage();
  }

  public getAccessToken(): string {
    if (this.container) {
      return this.container.accessTokenEncoded;
    } else {
      return undefined;
    }
  }

  public getIdentityClaims(): object {
    if (this.container) {
      return this.container.idToken;
    } else {
      return undefined;
    }
  }

  public getRefreshToken(): string {
    if (this.container) {
      return this.container.refreshTokenEncoded;
    } else {
      return undefined;
    }
  }

  public getAccessTokenExpiration(): number {
    if (this.container && this.container.accessToken) {
      return this.container.accessToken.exp;
    } else {
      return 0;
    }
  }

  private setTokenContainer(container: TokenContainerModel): void {
    this.logger.info(container);
    if (container) {
      this.container = container;
      this.setLocalStorage(container);
    } else {
      throw new Error('TokenService.setTokenContainer failed.');
    }
  }

  public fetchTokenUsingAnonymousFlow(): Promise<void> {
    return new Promise<void>(async (resolve, reject) => {
      try {
        this.container = this.readLocalStorage();
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
        this.container = this.readLocalStorage();
        if (this.container && this.container.idToken && !this.container.idToken.isAnon) {
          // Use existing token that is NOT an anonymous user otherwise fetch new token
          resolve();
        } else {
          this.tokenProxy.create(username, password).subscribe(result => {
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

  public refreshToken(): Promise<void> {
    return new Promise<void>(async (resolve, reject) => {
      try {
        this.container = this.readLocalStorage();
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
    });
  }

  public loadUserProfile(): Promise<void> {
    return new Promise<void>(async (resolve, reject) => {
      try {
        this.container = this.readLocalStorage();
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
