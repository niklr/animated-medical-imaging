import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  ApiOptionsAmiApiClient,
  IApiOptions
} from '../clients/ami-api-client';
import { BaseProxy } from './base.proxy';
import { AuthService } from '../services/auth.service';

@Injectable()
export class OptionProxy extends BaseProxy {

  constructor(authService: AuthService, private apiOptionsClient: ApiOptionsAmiApiClient) {
    super(authService);
  }

  public getApiOptions(): Promise<IApiOptions> {
    return new Promise<IApiOptions>((resolve, reject) => {
      super.preflight().then(() => {
        return this.apiOptionsClient.get().subscribe(result => {
          resolve(result);
        }, error => {
          reject(error);
        });
      }, error => {
        reject(error);
      });
    });
  }
}
