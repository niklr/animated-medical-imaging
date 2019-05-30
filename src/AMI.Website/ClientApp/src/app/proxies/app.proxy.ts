import { Injectable } from '@angular/core';
import { AppInfoAmiApiClient, AppInfo } from '../clients/ami-api-client';
import { BaseProxy } from './base.proxy';

@Injectable()
export class AppProxy extends BaseProxy {

  private appInfo: AppInfo;

  constructor(private appInfoClient: AppInfoAmiApiClient) {
    super();
  }

  public getAppVersion(): string {
    //if (!this.appInfo) {
    //  this.appInfoClient.get().subscribe(result => {
    //    this.appInfo = result;
    //  }, error => {
    //    // TODO: handle error
    //  });
    //}
    return this.appInfo ? this.appInfo.appVersion : '0.0.0';
  }
}
