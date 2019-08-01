import { Injectable } from '@angular/core';
import { TokenContainerModel } from '../clients/ami-api-client';
import { MomentUtil } from '../utils';

@Injectable()
export class TokenStore {

    private container: TokenContainerModel;

    constructor(private momentUtil: MomentUtil) {
    }

    public clear(): void {
        this.container = undefined;
        localStorage.removeItem('tokenContainer');
    }

    public get isExpired(): boolean {
        try {
          const exp = this.getAccessTokenExpiration();
          if (!!exp && exp > 0) {
            const diff = this.momentUtil.getDiffInSeconds(this.momentUtil.getFromUnix(exp));
            return diff >= 0;
          }
          return true;
        } catch (error) {
          return true;
        }
      }

    public getAccessToken(): string {
        this.loadTokenContainer();
        if (this.container) {
            return this.container.accessTokenEncoded;
        } else {
            return undefined;
        }
    }

    public getIdentityClaims(): object {
        this.loadTokenContainer();
        if (this.container) {
            return this.container.idToken;
        } else {
            return undefined;
        }
    }

    public getRefreshToken(): string {
        this.loadTokenContainer();
        if (this.container) {
            return this.container.refreshTokenEncoded;
        } else {
            return undefined;
        }
    }

    public getAccessTokenExpiration(): number {
        this.loadTokenContainer();
        if (this.container && this.container.accessToken) {
            return this.container.accessToken.exp;
        } else {
            return 0;
        }
    }

    public loadTokenContainer(): TokenContainerModel {
        if (!this.container) {
            this.container = this.readLocalStorage();
        }
        return this.container;
    }

    public setTokenContainer(container: TokenContainerModel) {
        if (container) {
            this.container = container;
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
}
