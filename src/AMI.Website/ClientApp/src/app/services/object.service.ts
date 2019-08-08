import { Injectable } from '@angular/core';
import { ObjectModelExtended } from '../models/object-extended.model';
import { TaskProxy } from '../proxies/task.proxy';
import { ObjectProxy } from '../proxies/object.proxy';
import { NotificationService } from './notification.service';
import { ObjectStore } from '../stores/object.store';
import { CallbackWrapper } from '../wrappers/callback.wrapper';

@Injectable()
export class ObjectService {

  constructor(private notificationService: NotificationService, private objectStore: ObjectStore,
              private objectProxy: ObjectProxy, private taskProxy: TaskProxy) {

  }

  public processSelectedObjects = (callbackFn) => {
    const callbackWrapper = new CallbackWrapper(callbackFn);

    const items = this.objectStore.getItems();
    if (items) {
      for (const item of items) {
        if (item.isChecked) {
          callbackWrapper.counter++;
          this.processObject(item.id, callbackWrapper);
        }
      }
    }

    if (callbackWrapper.counter <= 0) {
      callbackWrapper.invokeCallbackFn();
    }
  }

  public processObject(id: string, callbackWrapper: CallbackWrapper): void {
    const settings = this.objectStore.settings;
    this.taskProxy.create(id, settings).then(result => {
    }, error => {
      this.notificationService.handleError(error);
    }).finally(() => {
      if (callbackWrapper) {
        callbackWrapper.invokeCallbackFn();
      }
    });
  }

  public downloadSelectedObjects = (callbackFn) => {
    const callbackWrapper = new CallbackWrapper(callbackFn);

    const items = this.objectStore.getItems();
    if (items) {
      for (const item of items) {
        if (item.isChecked) {
          callbackWrapper.counter++;
          this.downloadObject(item, callbackWrapper);
        }
      }
    }

    if (callbackWrapper.counter <= 0) {
      callbackWrapper.invokeCallbackFn();
    }
  }

  public downloadObject(object: ObjectModelExtended, callbackWrapper: CallbackWrapper): void {
    this.objectProxy.downloadObject(object);
    if (callbackWrapper) {
      callbackWrapper.invokeCallbackFn();
    }
  }

  public deleteSelectedObjects = (callbackFn) => {
    const callbackWrapper = new CallbackWrapper(callbackFn);

    const items = this.objectStore.getItems();
    for (const item of items) {
      if (item.isChecked) {
        callbackWrapper.counter++;
        this.deleteObject(item.id, callbackWrapper);
      }
    }

    if (callbackWrapper.counter <= 0) {
      callbackWrapper.invokeCallbackFn();
    }
  }

  public deleteObject(id: string, callbackWrapper: CallbackWrapper): void {
    this.objectProxy.deleteObject(id).then(result => {
      this.objectStore.deleteById(id);
    }, error => {
      this.notificationService.handleError(error);
    }).finally(() => {
      if (callbackWrapper) {
        callbackWrapper.invokeCallbackFn();
      }
    });
  }
}
