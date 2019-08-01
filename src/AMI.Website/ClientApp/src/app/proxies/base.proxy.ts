import { Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Injectable()
export class BaseProxy {

  constructor(protected authService: AuthService) {
  }

  public preflight(): Promise<void> {
    return new Promise<void>(async (resolve, reject) => {
      try {
        // console.log('authService.isAuthenticated: ' + this.authService.isAuthenticated);
        // console.log('authService.isExpired: ' + this.authService.isExpired);
        if (this.authService.isAuthenticated && this.authService.isExpired) {
          this.authService.refresh().then(() => {
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
