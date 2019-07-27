import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  TokensAmiApiClient,
  CredentialsModel,
  TokenContainerModel
} from '../clients/ami-api-client';

@Injectable()
export class TokenProxy {

  constructor(private tokensClient: TokensAmiApiClient) {
  }

  public create(username: string, password: string): Observable<TokenContainerModel> {
    var credentials = new CredentialsModel({
      username: username,
      password: password
    });
    return this.tokensClient.create(credentials);
  }
  
  public createAnon(): Observable<TokenContainerModel> {
    return this.tokensClient.createAnonymous();
  }
}
