import { Injectable } from '@angular/core';
import { ProcessObjectCommand } from '../clients/ami-api-client';
import { PageEvent } from '../events/page.event';
import { ObjectModelExtended } from '../models/object-extended.model';

@Injectable()
export class ObjectStore {

  private items: ObjectModelExtended[] = [];

  public count = 0;
  public settings: ProcessObjectCommand = new ProcessObjectCommand();
  public pageEvent: PageEvent = new PageEvent();

  constructor() {
    this.settings.desiredSize = 250;
    this.settings.amountPerAxis = 10;
    this.settings.grayscale = true;
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
