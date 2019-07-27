import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { GarbageCollector, MomentUtil } from '../utils';
import { NotificationService } from './notification.service';
import { LoggerService } from './logger.service';
import { TokenService } from './token.service';
import { IdentityModel } from '../models/identity.model';

@Injectable()
export class AuthService extends BaseService {

  private _isInitialized: boolean;
  private _refreshRetryCount = 0;

  public user: IdentityModel;

  constructor(gc: GarbageCollector, notificationService: NotificationService, logger: LoggerService,
    private tokenService: TokenService, private momentUtil: MomentUtil) {
    super(gc, notificationService, logger);

    this._isInitialized = false;

    gc.attach(function () {
      const that = this as AuthService;
      that.clear();
    }.bind(this));
  }

  private clear(): void {
    this._isInitialized = false;
  }

  private setUser(): boolean {
    return true;
  }

  public get isAuthenticated(): boolean {
    return false;
  }

  public get isExpired(): boolean {
    try {
      const exp = this.tokenService.getAccessTokenExpiration();
      if (!!exp && exp > 0) {
        const diff = this.momentUtil.getDiffInMinutes(exp);
        return diff >= -2;
      }
      return true;
    } catch (error) {
      return true;
    }
  }

  public logout(): void {
    this.gc.notify();
  }

  public async refresh(): Promise<boolean> {
    const defaultMessage = 'Refreshing authentication failed. Try to logout and login again.';
    return this.tokenService.refreshToken().then(
      (s) => {
        // Loading data about the user
        return this.tokenService.loadUserProfile();
      },
      (e) => {
        throw e;
      }).then(
        (s) => {
          if (this.setUser()) {
            this._refreshRetryCount = 0;
            return true;
          } else {
            throw new Error(defaultMessage);
          }
        },
        (e) => {
          this.handleAuthError(e, defaultMessage);
          // 400: stale refresh token / invalid_grant
          if (!!e.status && e.status === 400) {
            if (this._refreshRetryCount >= 3) {
              this._refreshRetryCount = 0;
              this.logout();
            } else {
              this._refreshRetryCount += 1;
            }
          }
          throw e;
        });
  }

  private handleAuthError(e: any, defaultMessage: string): void {
    let message = defaultMessage;
    if (!!e && !!e.error && !!e.error.error_description) {
      message = e.error.error_description;
    }
    this.notificationService.raiseError(message, e);
    this.logger.error(e);
  }
}
