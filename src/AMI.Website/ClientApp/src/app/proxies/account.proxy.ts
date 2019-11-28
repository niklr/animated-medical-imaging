import { Injectable } from '@angular/core';
import { BaseProxy } from './base.proxy';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AccountProxy extends BaseProxy {

  constructor(authService: AuthService) {
    super(authService);
  }

  public login(token: string, redirectUrl: string): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      super.preflight().then(() => {
        resolve();
        const url = this.baseUrl + '/account/login?token=' + token + '&redirectUrl=' + redirectUrl;
        const win = window.open(url, '_blank');
        win.focus();
      }, error => {
        reject(error);
      });
    });
  }

}
