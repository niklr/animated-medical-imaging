import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { ConfigService } from './config.service';
import { NotificationService } from './notification.service';
import { LoggerService } from './logger.service';
import { TokenService } from './token.service';
import { GatewayHub } from '../hubs';
import { IdentityModel } from '../models/identity.model';
import { GarbageCollector, MomentUtil } from '../utils';

@Injectable()
export class AuthService extends BaseService {

  private isInitialized: boolean;
  private refreshRetryCount = 0;

  public user: IdentityModel;

  constructor(gc: GarbageCollector, notificationService: NotificationService, logger: LoggerService,
              private gateway: GatewayHub, private tokenService: TokenService, private momentUtil: MomentUtil) {
    super(gc, notificationService, logger);

    this.isInitialized = false;

    gc.attach(() => {
      this.clear();
    });
  }

  private clear(): void {
    this.isInitialized = false;
    this.user = undefined;
  }

  private setUser(): void {
    // Using the loaded user data
    const claims: any = this.tokenService.getIdentityClaims();
    this.logger.info(claims);

    if (claims) {
      // TODO: read claims based on names defined in the API options
      this.user = new IdentityModel();
      this.user.sub = claims.sub;
      this.user.username = claims.username;
      this.user.isAnon = claims.isAnon;
      this.user.roles = claims.roles;
      this.gateway.start(this.tokenService.getAccessToken());
    } else {
      throw new Error('Identity claims could not be loaded.');
    }
  }

  public get isAuthenticated(): boolean {
    return !!this.user;
  }

  public get isExpired(): boolean {
    try {
      const exp = this.tokenService.getAccessTokenExpiration();
      if (!!exp && exp > 0) {
        const diff = this.momentUtil.getDiffInMinutes(this.momentUtil.getFromUnix(exp));
        return diff >= -2;
      }
      return true;
    } catch (error) {
      return true;
    }
  }

  public async login(username: string, password: string): Promise<void> {
    this.gc.notify();
    const message = 'Login failed.';
    return this.init().then(async () => {
      return this.tokenService.fetchTokenUsingPasswordFlow(username, password).then(
        (s) => {
          // Loading profile of the user
          return this.tokenService.loadUserProfile();
        },
        (e) => {
          this.handleAuthError(e, message);
          throw e;
        }).then(
          (s) => {
            this.setUser();
          },
          (e) => {
            this.handleAuthError(e, message);
            throw e;
          });
    });
  }

  public logout(): void {
    this.gc.notify();
  }

  public async init(): Promise<void> {
    return new Promise<void>(async (resolve, reject) => {
      const extendedReject = (error: any) => {
        this.notificationService.handleError(error);
        reject();
      };
      try {
        if (this.isInitialized) {
          resolve();
        } else {
          const extendedResolve = () => {
            this.isInitialized = true;
            resolve();
          };
          // Check if refresh token is existing
          if (this.tokenService.getRefreshToken()) {
            return this.refresh().then(() => {
              extendedResolve();
            }, (e) => {
              extendedReject(e);
            });
          } else {
            // Check if anonymous users are allowed
            if (ConfigService.apiOptions && ConfigService.apiOptions.authOptions && ConfigService.apiOptions.authOptions.allowAnonymous) {
              // Login as anonymous user
              return this.tokenService.fetchTokenUsingAnonymousFlow().then(() => {
                this.setUser();
                extendedResolve();
              }, (e) => {
                extendedReject(e);
              });
            } else {
              extendedResolve();
            }
          }
        }
      } catch (error) {
        extendedReject(error);
      }
    });
  }

  public async refresh(): Promise<void> {
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
          this.setUser();
          this.refreshRetryCount = 0;
        },
        (e) => {
          this.handleAuthError(e, defaultMessage);
          // 400: stale refresh token / invalid_grant
          if (!!e.status && e.status === 400) {
            if (this.refreshRetryCount >= 3) {
              this.refreshRetryCount = 0;
              this.logout();
            } else {
              this.refreshRetryCount += 1;
            }
          }
          throw e;
        });
  }

  private handleAuthError(e: any, defaultMessage: string): void {
    let message = defaultMessage;
    if (e && e.error) {
      if (e.error.error_description) {
        message = e.error.error_description;
      } else {
        message = e.error;
      }
    }
    this.notificationService.raiseError(message, e);
    this.logger.error(e);
  }
}
