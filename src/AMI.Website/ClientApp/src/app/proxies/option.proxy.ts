import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  ApiOptionsAmiApiClient,
  IApiOptions
} from '../clients/ami-api-client';

@Injectable()
export class OptionProxy {

  constructor(private apiOptionsClient: ApiOptionsAmiApiClient) {
  }

  public getApiOptions(): Observable<IApiOptions> {
    return this.apiOptionsClient.get();
  }
}
