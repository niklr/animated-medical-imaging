import { Injectable } from '@angular/core';
import { ProcessObjectCommand, TaskModel, ObjectModel } from '../clients/ami-api-client';
import { GatewayEvent } from '../enums';
import { PageEvent } from '../events/page.event';
import { GatewayHub } from '../hubs/gateway.hub';
import { ObjectModelExtended } from '../models/object-extended.model';

@Injectable()
export class ObjectStore {

  private items: ObjectModelExtended[] = [];

  public settings: ProcessObjectCommand = new ProcessObjectCommand();
  public pageEvent: PageEvent = new PageEvent();

  constructor(private gateway: GatewayHub) {
    this.settings.outputSize = 250;
    this.settings.amountPerAxis = 10;
    this.settings.delay = 100;
    this.settings.grayscale = true;
    this.initGateway();
  }

  private initGateway(): void {
    this.gateway.on(GatewayEvent[GatewayEvent.CreateTask], (data: TaskModel) => {
      this.updateLatestTask(TaskModel.fromJS(data));
    });
    this.gateway.on(GatewayEvent[GatewayEvent.UpdateTask], (data: TaskModel) => {
      this.updateLatestTask(TaskModel.fromJS(data));
    });
    this.gateway.on(GatewayEvent[GatewayEvent.CreateObject], (data: ObjectModel) => {
      if (data && data.id) {
        this.addItem(ObjectModel.fromJS(data) as ObjectModelExtended);
      }
    });
    this.gateway.on(GatewayEvent[GatewayEvent.DeleteObject], (data: ObjectModel) => {
      if (data && data.id) {
        this.deleteById(data.id);
      }
    });
  }

  private updateLatestTask(data: TaskModel): void {
    if (data && data.object && data.object.id) {
      const index = this.items.findIndex((current) => {
        return current.id === data.object.id;
      });
      if (index >= 0) {
        this.items[index].latestTask = data;
      }
    }
  }

  private addItem(item: ObjectModelExtended): void {
    const index = this.items.findIndex((current) => {
      return current.id === item.id;
    });
    if (index < 0) {
      this.items.unshift(item);
    }
  }

  public get count(): number {
    return this.items.length;
  }

  public getItems(): ObjectModelExtended[] {
    return this.items;
  }

  public setItems(items: ObjectModelExtended[]): void {
    this.items = items;
  }

  public deleteById(id: string): void {
    this.items = this.items.filter((item) => {
      return item.id !== id;
    });
  }

  public clear(): void {
    this.items.length = 0;
  }
}
