import { Component, OnInit, AfterViewInit } from '@angular/core';
import { PageEvent } from '../../../../events/page.event';
import { ObjectProxy } from '../../../../proxies/object.proxy';
import { ObjectStore } from '../../../../stores/object.store';
import { NotificationService } from '../../../../services/notification.service';
import {
  AxisContainerModelOfString,
  AxisType,
  ObjectModel,
  PositionAxisContainerModelOfString,
  ProcessObjectAsyncCommand,
  ProcessResultModel,
  TaskModel,
  TaskStatus
} from '../../../../clients/ami-api-client';

@Component({
  selector: 'app-processing-objects',
  templateUrl: './objects.component.html'
})
export class ObjectsComponent implements OnInit, AfterViewInit {

  private isChecked: boolean = true;

  // Paginator Output
  public pageEvent: PageEvent;

  constructor(private notificationService: NotificationService, private objectProxy: ObjectProxy, public objectStore: ObjectStore) {
    this.pageEvent = new PageEvent();
    this.pageEvent.pageIndex = 1;
    this.pageEvent.pageSize = 50;
    this.pageEvent.length = 0;
  }

  ngOnInit() {
    document.addEventListener('github:niklr/angular-material-datatransfer.item-completed', function (item) {
      try {
        let that = this as ObjectsComponent;
        that.refresh();
        let result = JSON.parse(item.detail.message) as ObjectModel;
        that.processObject(result.id);
      } catch (e) { }
    }.bind(this));
  }

  ngAfterViewInit(): void {
    this.init();
  }

  private initDropdown(): void {
    setTimeout(() => {
      var options = {};
      var elem = document.querySelector('#objectActionsDropdownButton');
      var instance = M.Dropdown.init(elem, options);
    });
  }

  private init(): void {
    // this.initDemoObjects();
    this.setPage(this.pageEvent);
  }

  private afterInit(): void {
    this.initDropdown();
    // Mark all objects as checked by default
    var items = this.objectStore.getItems();
    if (items) {
      for (var i = 0; i < items.length; i++) {
        var item: any = items[i];
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
      this.objectStore.setItems([object1, object2, object3]);
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

  public processObject(id: string): void {
    var settings = this.objectStore.settings;
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
    this.objectProxy.processObject(id, command).subscribe(result => {
      this.refresh();
    }, error => {
      this.notificationService.handleError(error);
    });
  }

  public toggleCheckbox(): void {
    this.isChecked = !this.isChecked;
    var items = this.objectStore.getItems();
    if (items) {
      for (var i = 0; i < items.length; i++) {
        var item: any = items[i];
        item.isChecked = this.isChecked;
      }
    }
  }

  public setPage(event: PageEvent): void {
    this.pageEvent = event;
    this.objectProxy.getObjects(this.pageEvent.pageIndex, this.pageEvent.pageSize).subscribe(result => {
      this.pageEvent.previousPageIndex = this.pageEvent.pageIndex;
      this.pageEvent.pageIndex = result.pagination.page;
      this.pageEvent.pageSize = result.pagination.limit;
      this.pageEvent.length = result.pagination.total;
      this.objectStore.setItems(result.items);
      this.afterInit();
    }, error => {
      console.log(error);
      this.notificationService.handleError(error);
    });
  }

  public refresh(): void {
    this.setPage(this.pageEvent);
  }
}

