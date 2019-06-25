import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  CreateTaskCommand,
  ProcessObjectCommand,
  TaskModel,
  TasksAmiApiClient
} from '../clients/ami-api-client';
import { BaseProxy } from './base.proxy';

@Injectable()
export class TaskProxy extends BaseProxy {

  constructor(private tasksClient: TasksAmiApiClient) {
    super();
  }

  public create(id: string, settings: ProcessObjectCommand): Observable<TaskModel> {
    var commandCopy = new ProcessObjectCommand(settings);
    commandCopy.id = id;
    var command = new CreateTaskCommand({
      command: commandCopy
    });
    return this.tasksClient.create(command);
  }
}
