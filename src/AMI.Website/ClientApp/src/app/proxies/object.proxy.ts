import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  ObjectsAmiApiClient,
  ObjectModel,
  PaginationResultModelOfObjectModel
} from '../clients/ami-api-client';
import { BaseProxy } from './base.proxy';
import { AuthService } from '../services/auth.service';
import { ConfigService } from '../services/config.service';

@Injectable()
export class ObjectProxy extends BaseProxy {

  constructor(authService: AuthService, private objectsClient: ObjectsAmiApiClient) {
    super(authService);
  }

  public getObjects(page: number, limit: number): Promise<PaginationResultModelOfObjectModel> {
    return new Promise<PaginationResultModelOfObjectModel>((resolve, reject) => {
      super.preflight().then(() => {
        return this.objectsClient.getPaginated(page, limit).subscribe(result => {
          resolve(result);
        }, error => {
          reject(error);
        });
      }, error => {
        reject(error);
      });
    });
  }

  public downloadObject(object: ObjectModel): void {
    if (object && object.latestTask && object.latestTask.result) {
      const result = object.latestTask.result;
      const eventDetail = {
        filename: object.originalFilename + '.zip',
        url: ConfigService.options.apiEndpoint + '/results/' + result.id + '/download',
        size: 0
      };
      const event = new CustomEvent('github:niklr/angular-material-datatransfer.download-item', { detail: eventDetail });
      document.dispatchEvent(event);
    }
  }

  public deleteObject(id: string): Promise<ObjectModel> {
    return new Promise<ObjectModel>((resolve, reject) => {
      super.preflight().then(() => {
        return this.objectsClient.deleteById(id).subscribe(result => {
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
