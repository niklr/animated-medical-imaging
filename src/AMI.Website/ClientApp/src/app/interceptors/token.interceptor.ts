import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';

import { Observable } from 'rxjs';
import { ConfigService } from '../services/config.service';
import { TokenStore } from '../stores';


@Injectable()
export class TokenInterceptor {

constructor(public tokenStore: TokenStore) {}

/**
 * Intercept all HTTP requests to add JWT token to Headers
 */
intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    // Exclude interceptor on startup or if token is expired
    // to prevent expired token exception
    if (ConfigService.isInitialized && !this.tokenStore.isExpired) {
        const token = this.tokenStore.getAccessToken();
        if (token) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${token}`
                }
             });
        }
    }

    return next.handle(request);
  }
}
