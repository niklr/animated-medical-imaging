import { Component, OnInit, AfterViewInit } from '@angular/core';
import { PubSubTopic } from '../../../enums/pub-sub-topic.enum';
import { TaskProxy } from '../../../proxies';
import { NotificationService } from '../../../services/notification.service';
import { PubSubService } from '../../../services/pubsub.service';
import { ObjectStore } from '../../../stores/object.store';
import { CallbackWrapper } from '../../../wrappers/callback.wrapper';
import { AxisType, ProcessObjectCommand, ObjectModel } from '../../../clients/ami-api-client';

class AxisTypeContainer {
  displayName: string;
  enum: AxisType;
  checked: boolean;
  public constructor(init?: Partial<AxisTypeContainer>) {
    Object.assign(this, init);
  }
}

@Component({
  selector: 'app-processing-settings',
  templateUrl: './settings.component.html'
})
export class SettingsComponent implements OnInit, AfterViewInit {

  public settings: ProcessObjectCommand;

  public axisTypeContainers: AxisTypeContainer[] = [
    new AxisTypeContainer({ displayName: "X-Axis", enum: AxisType.X, checked: true }),
    new AxisTypeContainer({ displayName: "Y-Axis", enum: AxisType.Y, checked: true }),
    new AxisTypeContainer({ displayName: "Z-Axis", enum: AxisType.Z, checked: true })
  ];

  constructor(public objectStore: ObjectStore, private notificationService: NotificationService,
    private pubSubService: PubSubService, private taskProxy: TaskProxy) {
    this.settings = this.objectStore.settings;
  }

  ngOnInit() {
    this.setAxisTypes();
    document.addEventListener('github:niklr/angular-material-datatransfer.item-completed', function (item) {
      try {
        let that = this as SettingsComponent;
        that.pubSubService.publish(PubSubTopic.OBJECTS_INIT_TOPIC, undefined);
        let result = JSON.parse(item.detail.message) as ObjectModel;
        that.processObject(result.id, undefined);
      } catch (e) { }
    }.bind(this));
  }

  ngAfterViewInit(): void {
    M.updateTextFields();
  }

  public setAxisTypes(): void {
    this.settings.axisTypes = [];
    for (var i = 0; i < this.axisTypeContainers.length; i++) {
      var axisTypeContainer = this.axisTypeContainers[i];
      if (axisTypeContainer.checked) {
        this.settings.axisTypes.push(axisTypeContainer.enum);
      }
    }
  }

  public toggleAxisTypeContainer(axisTypeContainer: AxisTypeContainer, $event) {
    axisTypeContainer.checked = !axisTypeContainer.checked;
    this.setAxisTypes();
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
    this.taskProxy.create(id, settings).subscribe(result => {
      this.pubSubService.publish(PubSubTopic.OBJECTS_INIT_TOPIC, undefined);
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
