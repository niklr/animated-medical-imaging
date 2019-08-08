import { Injectable } from '@angular/core';
import {
    AppLogsAmiApiClient,
    PaginationResultModelOfAppLogModel,
} from '../clients/ami-api-client';
import { BaseProxy } from './base.proxy';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AdminProxy extends BaseProxy {

    constructor(authService: AuthService, private appLogsClient: AppLogsAmiApiClient) {
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
}
