import { Injectable } from '@angular/core';
import { ObjectModel, ProcessObjectCommand } from '../clients/ami-api-client';

@Injectable()
export class ObjectStore {

  private items: ObjectModel[] = [];

  public count = 0;
  public settings: ProcessObjectCommand = new ProcessObjectCommand()

  constructor() {
    this.settings.desiredSize = 250;
    this.settings.amountPerAxis = 10;
    this.settings.grayscale = true;
  }

  private updateCount(): void {
    this.count = this.items.length;
  }

  public getItems(): ObjectModel[] {
    return this.items;
  }

  public setItems(items: ObjectModel[]): void {
    this.items = items;
    this.updateCount();
  }

  public clear(): void {
    this.items.length = 0;
    this.updateCount();
  }
}
