import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  ObjectsAmiApiClient,
  PaginationResultModelOfObjectModel,
  ProcessObjectAsyncCommand,
  TaskModel
} from '../clients/ami-api-client';
import { BaseProxy } from './base.proxy';

@Injectable()
export class ObjectProxy extends BaseProxy {

  constructor(private objectsClient: ObjectsAmiApiClient) {
    super();
  }

  public getObjects(page: number, limit: number): Observable<PaginationResultModelOfObjectModel> {
    return this.objectsClient.getPaginated(page, limit);
  }

  public processObject(id: string, command: ProcessObjectAsyncCommand): Observable<TaskModel> {
    return this.objectsClient.process(id, command);
  }
}
