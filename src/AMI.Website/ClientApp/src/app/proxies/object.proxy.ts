import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  ObjectsAmiApiClient,
  ObjectModel,
  PaginationResultModelOfObjectModel
} from '../clients/ami-api-client';
import { BaseProxy } from './base.proxy';
import { ConfigService } from '../services/config.service';

@Injectable()
export class ObjectProxy extends BaseProxy {

  constructor(private objectsClient: ObjectsAmiApiClient) {
    super();
  }

  public getObjects(page: number, limit: number): Observable<PaginationResultModelOfObjectModel> {
    return this.objectsClient.getPaginated(page, limit);
  }

  public downloadObject(object: ObjectModel): void {
    if (object && object.latestTask && object.latestTask.result) {
      var result = object.latestTask.result;
      var eventDetail = {
        'filename': object.originalFilename + '.zip',
        'url': ConfigService.settings.apiEndpoint + '/results/' + result.id + '/download',
        'size': 0
      };
      var event = new CustomEvent('github:niklr/angular-material-datatransfer.download-item', { 'detail': eventDetail });
      document.dispatchEvent(event);
    }
  }

  public deleteObject(id: string): Observable<ObjectModel> {
    return this.objectsClient.deleteById(id);
  }
}
