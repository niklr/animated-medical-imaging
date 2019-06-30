import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { PubSubTopic } from '../../../enums/pub-sub-topic.enum';
import { PageEvent } from '../../../events/page.event';
import { ObjectModelExtended } from '../../../models/object-extended.model';
import { ObjectProxy } from '../../../proxies';
import { ObjectStore } from '../../../stores/object.store';
import { NotificationService } from '../../../services/notification.service';
import { PubSubService } from '../../../services/pubsub.service';
import { CallbackWrapper } from '../../../wrappers/callback.wrapper';
import {
  AxisContainerModelOfString,
  AxisType,
  ObjectModel,
  PositionAxisContainerModelOfString,
  ProcessResultModel,
  TaskModel,
  TaskStatus
} from '../../../clients/ami-api-client';

@Component({
  selector: 'app-processing-objects',
  templateUrl: './objects.component.html'
})
export class ObjectsComponent implements OnInit, AfterViewInit, OnDestroy {

  private _subs: string[] = [];

  public isChecked: boolean = true;
  public pageEvent: PageEvent;

  constructor(private notificationService: NotificationService, private pubSubService: PubSubService,
    private objectProxy: ObjectProxy, public objectStore: ObjectStore) {
    this.pageEvent = this.objectStore.pageEvent;
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    this.init();
  }

  ngOnDestroy(): void {
    this._subs.forEach(sub => {
      this.pubSubService.unsubscribe(sub);
    });
    this._subs = [];
  }

  private initDropdown(): void {
    setTimeout(() => {
      var options = {};
      var elem = document.querySelector('#objectActionsDropdownButton');
      var instance = M.Dropdown.init(elem, options);
    });
  }

  private init(): void {
    const sub1 = this.pubSubService.subscribe(PubSubTopic.OBJECTS_INIT_TOPIC, function (msg, data) {
      const that = this as ObjectsComponent;
      that.refresh();
    }.bind(this));
    this._subs.push(sub1);

    // this.initDemoObjects();
    this.setPage(this.pageEvent);
  }

  private afterInit(): void {
    this.initDropdown();
    // Mark all objects as checked by default
    this.isChecked = true;
    var items = this.objectStore.getItems();
    if (items) {
      for (var i = 0; i < items.length; i++) {
        var item = items[i];
        item.isChecked = true;
      }
    }
  }

  private initDemoObjects(): void {
    var object1 = new ObjectModel({
      id: '19f06aa9-1856-4cc9-ba30-1ea0483fc154',
      originalFilename: 'SMIR.Brain.XX.O.MR_Flair.36620.mha',
      modifiedDate: new Date('2019-05-21T14:05:25.3100000Z'),
      latestTask: new TaskModel({
        id: '87619062-ec17-4e4a-ab08-4f87aeea4249',
        createdDate: new Date('2019-05-21T14:05:26.1100000Z'),
        modifiedDate: new Date('2019-05-21T14:05:27.1900000Z'),
        status: TaskStatus.Finished,
        position: 0,
        result: this.createDemoResult()
      })
    });

    var object2 = new ObjectModel({
      id: 'a8bfea94-614b-466c-9400-3ace2d5e4f06',
      originalFilename: 'SMIR.Brain.XX.O.CT.346124.nii',
      modifiedDate: new Date('2019-04-17T07:52:41.4700000Z'),
      latestTask: new TaskModel({
        id: '87619062-ec17-4e4a-ab08-4f87aeea4249',
        createdDate: new Date('2019-05-21T14:05:26.1100000Z'),
        modifiedDate: new Date('2019-05-21T14:05:27.1900000Z'),
        status: TaskStatus.Queued,
        position: 12
      })
    });

    var object3 = new ObjectModel({
      id: 'a8bfea94-614b-466c-9400-3ace2d5e4f06',
      originalFilename: 'SMIR.Brain.XX.O.CT.346124.nii',
      modifiedDate: new Date('2019-04-17T07:52:41.4700000Z')
    });

    setTimeout(() => {
      var items = [object1, object2, object3] as ObjectModelExtended[];
      this.objectStore.setItems(items);
      this.afterInit();
    });
  }

  private createDemoResult(): ProcessResultModel {
    var result = new ProcessResultModel();
    var example1 = 'https://raw.githubusercontent.com/niklr/animated-medical-imaging/master/assets/images/example1/';
    var example2 = 'https://raw.githubusercontent.com/niklr/animated-medical-imaging/master/assets/images/example2/';
    result.labelCount = 4;
    result.combinedGif = example1 + 'Z.gif';
    result.gifs = [
      new AxisContainerModelOfString({
        axisType: AxisType.X,
        entity: example1 + 'Z.gif'
      }),
      new AxisContainerModelOfString({
        axisType: AxisType.Y,
        entity: example2 + 'Z.gif'
      }),
      new AxisContainerModelOfString({
        axisType: AxisType.Z,
        entity: example1 + 'Z.gif'
      })
    ];

    result.images = [];

    for (var i = 0; i < 10; i++) {
      result.images.push(this.createDemoResultItem(AxisType.X, example1, i));
    }

    for (var i = 0; i < 10; i++) {
      result.images.push(this.createDemoResultItem(AxisType.Y, example2, i));
    }

    for (var i = 0; i < 10; i++) {
      result.images.push(this.createDemoResultItem(AxisType.Z, example1, i));
    }

    return result;
  }

  private createDemoResultItem(axisType: AxisType, basePath: string, position: number): PositionAxisContainerModelOfString {
    return new PositionAxisContainerModelOfString({
      axisType: axisType,
      entity: basePath + 'Z_' + position + '.png',
      position: position
    });
  }

  public deleteSelectedObjects = (callbackFn) => {
    var items = this.objectStore.getItems();
    for (var i = 0; i < items.length; i++) {
      var item = items[i];
      if (item.isChecked) {
        this.deleteObject(item.id);
      }
    }
  }

  public deleteObject(id: string): void {
    this.objectProxy.deleteObject(id).subscribe(result => {
      this.objectStore.deleteById(id);
      if (this.objectStore.count <= 0) {
        this.refresh();
      }
    }, error => {
      this.notificationService.handleError(error);
    });
  }

  public toggleCheckbox(): void {
    this.isChecked = !this.isChecked;
    var items = this.objectStore.getItems();
    if (items) {
      for (var i = 0; i < items.length; i++) {
        var item = items[i];
        item.isChecked = this.isChecked;
      }
    }
  }

  public setPage(event: PageEvent): void {
    this.objectStore.pageEvent = event;
    this.pageEvent = this.objectStore.pageEvent;
    this.objectProxy.getObjects(this.pageEvent.pageIndex, this.pageEvent.pageSize).subscribe(result => {
      this.pageEvent.previousPageIndex = this.pageEvent.pageIndex;
      this.pageEvent.pageIndex = result.pagination.page;
      this.pageEvent.pageSize = result.pagination.limit;
      this.pageEvent.length = result.pagination.total;
      this.objectStore.setItems(result.items as ObjectModelExtended[]);
      this.afterInit();
    }, error => {
      this.notificationService.handleError(error);
    });
  }

  public refresh(): void {
    this.setPage(new PageEvent());
  }

  public downloadSelectedObjects = (callbackFn) => {
    var callbackWrapper = new CallbackWrapper(callbackFn);

    var items = this.objectStore.getItems();
    if (items) {
      for (var i = 0; i < items.length; i++) {
        var item = items[i];
        if (item.isChecked) {
          callbackWrapper.counter++;
          // TODO: start download
          callbackWrapper.invokeCallbackFn();
        }
      }
    }

    if (callbackWrapper.counter <= 0) {
      callbackWrapper.invokeCallbackFn();
    }
  }
}

