import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';

import { Observable } from 'rxjs';
import { TokenService } from '../services/token.service';


@Injectable()
export class TokenInterceptor {

constructor(public tokenService: TokenService) {}

/**
 * Intercept all HTTP requests to add JWT token to Headers
 */
intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const token = this.tokenService.getAccessToken();

    if (token) {
        request = request.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`
            }
         });
    }

    return next.handle(request);
  }
}