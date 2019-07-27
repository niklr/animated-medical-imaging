import { Injectable } from '@angular/core';
import { Logger } from './logger.service';
import { ConfigService } from './config.service';

const noop = (): any => undefined;

@Injectable()
export class ConsoleLoggerService implements Logger {

    private isEnabled = false;

    constructor() {
        this.isEnabled = ConfigService.options.enableConsoleOutput;
    }

    get info() {
        if (this.isEnabled) {
            // tslint:disable-next-line:no-console
            return console.info.bind(console);
        } else {
            return noop;
        }
    }

    get warn() {
        if (this.isEnabled) {
            return console.warn.bind(console);
        } else {
            return noop;
        }
    }

    get error() {
        if (this.isEnabled) {
            return console.error.bind(console);
        } else {
            return noop;
        }
    }

    invokeConsoleMethod(type: string, args?: any): void {
        const logFn = (console)[type] || console.log || noop;
        logFn.apply(console, [args]);
    }
}
