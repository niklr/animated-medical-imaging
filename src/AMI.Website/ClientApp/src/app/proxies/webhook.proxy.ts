import { Injectable } from '@angular/core';
import {
  PaginationResultModelOfWebhookModel,
  WebhooksAmiApiClient,
  WebhookModel,
  CreateWebhookCommand,
  UpdateWebhookCommand
} from '../clients/ami-api-client';
import { BaseProxy } from './base.proxy';
import { AuthService } from '../services/auth.service';
import { NotificationService } from '../services/notification.service';
import { WebhookModelExtended } from '../models/webhook-extended.model';

@Injectable()
export class WebhookProxy extends BaseProxy {

  constructor(authService: AuthService, private notificationService: NotificationService, private webhooksClient: WebhooksAmiApiClient) {
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

  public create(webhook: WebhookModelExtended): Promise<WebhookModel> {
    const command = new CreateWebhookCommand({
      url: webhook.url,
      apiVersion: webhook.apiVersion,
      secret: webhook.secret,
      enabledEvents: webhook.enabledEvents
    });
    return new Promise<WebhookModel>((resolve, reject) => {
      const handleError = (error: any) => {
        this.notificationService.handleError(error);
        reject(error);
      };
      super.preflight().then(() => {
        this.webhooksClient.create(command).subscribe(result => {
          resolve(result);
        }, error => {
          handleError(error);
        });
      }, error => {
        handleError(error);
      });
    });
  }

  public update(webhook: WebhookModelExtended): Promise<WebhookModel> {
    const command = new UpdateWebhookCommand({
      id: webhook.id,
      url: webhook.url,
      apiVersion: webhook.apiVersion,
      secret: webhook.secret,
      enabledEvents: webhook.enabledEvents
    });
    return new Promise<WebhookModel>((resolve, reject) => {
      const handleError = (error: any) => {
        this.notificationService.handleError(error);
        reject(error);
      };
      super.preflight().then(() => {
        this.webhooksClient.update(command.id, command).subscribe(result => {
          resolve(result);
        }, error => {
          handleError(error);
        });
      }, error => {
        handleError(error);
      });
    });
  }

  public delete(id: string): Promise<WebhookModel> {
    return new Promise<WebhookModel>((resolve, reject) => {
      super.preflight().then(() => {
        return this.webhooksClient.deleteById(id).subscribe(result => {
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
