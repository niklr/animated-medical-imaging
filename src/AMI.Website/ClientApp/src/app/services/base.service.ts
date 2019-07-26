import { Injectable } from '@angular/core';
import { GarbageCollector } from '../utils';
import { NotificationService } from './notification.service';
import { LoggerService } from './logger.service';
import { Observable } from 'rxjs';

@Injectable()
export class BaseService {

  protected readonly baseUrl: string;

  constructor(protected gc: GarbageCollector, protected notification: NotificationService, protected logger: LoggerService) {

  }

  public handleError(response: any) {
    const errorMessage = this.notification.handleError(response);
    return Observable.throw(errorMessage);
  }
}
