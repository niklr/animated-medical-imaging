import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  ObjectsAmiApiClient,
  ObjectModel,
  PaginationResultModelOfObjectModel,
  ProcessObjectAsyncCommand,
  ProcessObjectCommand,
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

  public deleteObject(id: string): Observable<ObjectModel> {
    return this.objectsClient.deleteById(id);
  }

  public processObject(id: string, settings: ProcessObjectCommand): Observable<TaskModel> {
    var command = new ProcessObjectAsyncCommand({
      amountPerAxis: settings.amountPerAxis,
      axisTypes: settings.axisTypes,
      bezierEasingTypeCombined: settings.bezierEasingTypeCombined,
      bezierEasingTypePerAxis: settings.bezierEasingTypePerAxis,
      desiredSize: settings.desiredSize,
      grayscale: settings.grayscale,
      id: id,
      imageFormat: settings.imageFormat
    });
    return this.objectsClient.process(id, command);
  }
}
