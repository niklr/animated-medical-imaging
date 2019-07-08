// https://robferguson.org/blog/2017/09/09/a-simple-logging-service-for-angular-4/

import { Injectable } from '@angular/core';

export abstract class Logger {
  info: any;
  warn: any;
  error: any;
}

@Injectable()
export class LoggerService implements Logger {

  info: any;
  warn: any;
  error: any;

  invokeConsoleMethod(type: string, args?: any): void { }
}
