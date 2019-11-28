import { Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { ConfigService } from '../services/config.service';

@Injectable()
export class BaseProxy {

  protected readonly baseUrl: string;

  constructor(protected authService: AuthService) {
    this.baseUrl = ConfigService.options.apiEndpoint;
  }

  public preflight(): Promise<void> {
    return new Promise<void>(async (resolve, reject) => {
      try {
        // console.log('authService.isAuthenticated: ' + this.authService.isAuthenticated);
        // console.log('authService.isExpired: ' + this.authService.isExpired);
        if (this.authService.isAuthenticated && this.authService.isExpired) {
          this.authService.refresh(false).then(() => {
            resolve();
          }, error => {
            reject(error);
          });
        } else {
          resolve();
        }
      } catch (error) {
        reject(error);
      }
    });
  }
}
