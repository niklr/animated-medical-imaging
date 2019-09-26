import { Injectable } from '@angular/core';
import {
  PaginationResultModelOfWebhookModel,
  WebhooksAmiApiClient
} from '../clients/ami-api-client';
import { BaseProxy } from './base.proxy';
import { AuthService } from '../services/auth.service';

@Injectable()
export class WebhookProxy extends BaseProxy {

  constructor(authService: AuthService, private webhooksClient: WebhooksAmiApiClient) {
    super(authService);
  }

  public getPaginated(page: number, limit: number): Promise<PaginationResultModelOfWebhookModel> {
    return new Promise<PaginationResultModelOfWebhookModel>((resolve, reject) => {
      super.preflight().then(() => {
        return this.webhooksClient.getPaginated(page, limit).subscribe(result => {
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
