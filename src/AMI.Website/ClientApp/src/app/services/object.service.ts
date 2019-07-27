import { Injectable } from '@angular/core';
import { ObjectModelExtended } from '../models/object-extended.model';
import { TaskProxy, ObjectProxy } from '../proxies';
import { NotificationService } from './notification.service';
import { ObjectStore } from '../stores/object.store';
import { CallbackWrapper } from '../wrappers/callback.wrapper';

@Injectable()
export class ObjectService {

  constructor(private notificationService: NotificationService, private objectStore: ObjectStore,
    private objectProxy: ObjectProxy, private taskProxy: TaskProxy) {

  }

  public processSelectedObjects = (callbackFn) => {
    var callbackWrapper = new CallbackWrapper(callbackFn);

    var items = this.objectStore.getItems();
    if (items) {
      for (var i = 0; i < items.length; i++) {
        var item = items[i];
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
    var settings = this.objectStore.settings;
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
    var callbackWrapper = new CallbackWrapper(callbackFn);

    var items = this.objectStore.getItems();
    if (items) {
      for (var i = 0; i < items.length; i++) {
        var item = items[i];
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
    var callbackWrapper = new CallbackWrapper(callbackFn);

    var items = this.objectStore.getItems();
    for (var i = 0; i < items.length; i++) {
      var item = items[i];
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
    this.objectProxy.deleteObject(id).subscribe(result => {
      this.objectStore.deleteById(id);
    }, error => {
      this.notificationService.handleError(error);
    }).add(() => {
      // finally block
      if (callbackWrapper) {
        callbackWrapper.invokeCallbackFn();
      }
    });
  }
}
