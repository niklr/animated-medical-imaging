import { Injectable } from '@angular/core';
import { ProcessObjectCommand, TaskModel, ObjectModel } from '../clients/ami-api-client';
import { GatewayEvent } from '../enums';
import { PageEvent } from '../events/page.event';
import { GatewayHub } from '../hubs/gateway.hub';
import { ObjectModelExtended } from '../models/object-extended.model';

@Injectable()
export class ObjectStore {

  private items: ObjectModelExtended[] = [];

  public count = 0;
  public settings: ProcessObjectCommand = new ProcessObjectCommand();
  public pageEvent: PageEvent = new PageEvent();

  constructor(private gateway: GatewayHub) {
    this.settings.outputSize = 250;
    this.settings.amountPerAxis = 10;
    this.settings.grayscale = true;
    this.initGateway();
  }

  private initGateway(): void {
    this.gateway.on(GatewayEvent[GatewayEvent.UpdateTask], function (data: TaskModel) {
      const that = this as ObjectStore;
      if (data && data.object && data.object.id) {
        var index = that.items.findIndex((item) => {
          return item.id === data.object.id;
        });
        if (index >= 0) {
          that.items[index].latestTask = data;
        }
      }
    }.bind(this));
    this.gateway.on(GatewayEvent[GatewayEvent.DeleteObject], function (data: ObjectModel) {
      const that = this as ObjectStore;
      if (data && data.id) {
        that.deleteById(data.id);
      }
    }.bind(this));
  }

  private updateCount(): void {
    this.count = this.items.length;
  }

  public getItems(): ObjectModelExtended[] {
    return this.items;
  }

  public setItems(items: ObjectModelExtended[]): void {
    this.items = items;
    this.updateCount();
  }

  public deleteById(id: string): void {
    this.items = this.items.filter((item) => {
      return item.id !== id;
    });
    this.updateCount();
  }

  public clear(): void {
    this.items.length = 0;
    this.updateCount();
  }
}
