import { Injectable } from '@angular/core';

@Injectable()
export class BackgroundWorker {

  private _observers: any[] = [];
  private _lastUserActivity = new Date();

  constructor() {

  }

  public init(): void {
    this.notify();
  }

  public attach(callback): void {
    this._observers.push(callback);
  }

  private notify(): void {
    setTimeout(function () {
      const that = this as BackgroundWorker;
      if (!document.hidden) {
        that._observers.forEach(observer => {
          observer();
        });
      }
      that.notify();
    }.bind(this), 10000);
  }

  public get lastUserActivity(): Date {
    return this._lastUserActivity;
  }

  public updateLastUserActivity(): void {
    this._lastUserActivity = new Date();
  }
}
