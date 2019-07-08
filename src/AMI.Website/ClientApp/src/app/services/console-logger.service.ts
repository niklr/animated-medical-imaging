import { Injectable } from '@angular/core';
import { Logger } from './logger.service';
import { ConfigService } from './config.service';

const noop = (): any => undefined;

@Injectable()
export class ConsoleLoggerService implements Logger {

    private _isEnabled = false;

    constructor() {
        this._isEnabled = ConfigService.options.enableConsoleOutput;
    }

    get info() {
        if (this._isEnabled) {
            // tslint:disable-next-line:no-console
            return console.info.bind(console);
        } else {
            return noop;
        }
    }

    get warn() {
        if (this._isEnabled) {
            return console.warn.bind(console);
        } else {
            return noop;
        }
    }

    get error() {
        if (this._isEnabled) {
            return console.error.bind(console);
        } else {
            return noop;
        }
    }

    invokeConsoleMethod(type: string, args?: any): void {
        const logFn: Function = (console)[type] || console.log || noop;
        logFn.apply(console, [args]);
    }
}
