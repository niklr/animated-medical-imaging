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
    const credentials = new CredentialsModel();
    credentials.username = username;
    credentials.password = password;
    return this.tokensClient.create(credentials);
  }

  public createAnon(): Observable<TokenContainerModel> {
    return this.tokensClient.createAnonymous();
  }

  public update(container: TokenContainerModel): Observable<TokenContainerModel> {
    return this.tokensClient.update(container);
  }
}
