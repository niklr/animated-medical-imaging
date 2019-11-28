import { Injectable } from '@angular/core';

@Injectable()
export class UrlUtil {

  constructor() {
  }

  public GetBaseUrl() {
    const getUrl = window.location;
    return getUrl.protocol + '//' + getUrl.host;
  }
}
