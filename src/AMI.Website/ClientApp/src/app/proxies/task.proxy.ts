import { Injectable } from '@angular/core';
import {
  CreateTaskCommand,
  ProcessObjectCommand,
  TaskModel,
  TasksAmiApiClient
} from '../clients/ami-api-client';
import { BaseProxy } from './base.proxy';
import { AuthService } from '../services/auth.service';
import { LoggerService } from '../services/logger.service';
import { NotificationService } from '../services/notification.service';

@Injectable()
export class TaskProxy extends BaseProxy {

  constructor(authService: AuthService, private logger: LoggerService, private notificationService: NotificationService,
              private tasksClient: TasksAmiApiClient) {
    super(authService);
  }

  public create(id: string, settings: ProcessObjectCommand): Promise<TaskModel> {
    const commandCopy = new ProcessObjectCommand(settings);
    commandCopy.id = id;
    const command = new CreateTaskCommand({
      command: commandCopy
    });
    return new Promise<TaskModel>((resolve, reject) => {
      const handleError = (error: any) => {
        this.notificationService.handleError(error);
        reject(error);
      };
      super.preflight().then(() => {
        this.tasksClient.create(command).subscribe(result => {
          resolve(result);
        }, error => {
          handleError(error);
        });
      }, error => {
        handleError(error);
      });
    });
  }
}
