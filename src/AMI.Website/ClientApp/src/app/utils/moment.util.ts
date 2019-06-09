import { Injectable } from '@angular/core';
import * as moment from 'moment';

@Injectable()
export class MomentUtil {

  constructor() {
    moment.locale(navigator.language);
  }

  public getFromUnix(date) {
    return moment.unix(date);
  }

  public getLocal(date) {
    return moment.utc(date).local();
  }

  public getLocalTime(date) {
    return this.getLocal(date).format('LT');
  }

  public getLocalDate(date) {
    return this.getLocal(date).format('L');
  }

  public getDiffInMinutes(date) {
    return moment().diff(date, 'minutes');
  }
}
