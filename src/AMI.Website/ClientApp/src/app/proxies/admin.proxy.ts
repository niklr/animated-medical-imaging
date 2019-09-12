import { Injectable } from '@angular/core';
import {
  AppLogsAmiApiClient,
  PaginationResultModelOfAppLogModel,
  PaginationResultModelOfAuditEventModel,
  PaginationResultModelOfBaseWorkerModel,
  AuditEventsAmiApiClient,
  WorkersAmiApiClient
} from '../clients/ami-api-client';
import { BaseProxy } from './base.proxy';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AdminProxy extends BaseProxy {

  constructor(authService: AuthService, private appLogsClient: AppLogsAmiApiClient,
    private auditEventsClient: AuditEventsAmiApiClient, private workersClient: WorkersAmiApiClient) {
    super(authService);
  }

  public getLogs(page: number, limit: number): Promise<PaginationResultModelOfAppLogModel> {
    return new Promise<PaginationResultModelOfAppLogModel>((resolve, reject) => {
      super.preflight().then(() => {
        return this.appLogsClient.getPaginated(page, limit).subscribe(result => {
          resolve(result);
        }, error => {
          reject(error);
        });
      }, error => {
        reject(error);
      });
    });
  }

  public getAuditEvents(page: number, limit: number): Promise<PaginationResultModelOfAuditEventModel> {
    return new Promise<PaginationResultModelOfAuditEventModel>((resolve, reject) => {
      super.preflight().then(() => {
        return this.auditEventsClient.getPaginated(page, limit).subscribe(result => {
          resolve(result);
        }, error => {
          reject(error);
        });
      }, error => {
        reject(error);
      });
    });
  }

  public getWorkers(page: number, limit: number): Promise<PaginationResultModelOfBaseWorkerModel> {
    return new Promise<PaginationResultModelOfBaseWorkerModel>((resolve, reject) => {
      super.preflight().then(() => {
        return this.workersClient.getPaginated(page, limit).subscribe(result => {
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
