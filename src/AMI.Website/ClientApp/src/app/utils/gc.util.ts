import { Injectable } from '@angular/core';

@Injectable()
export class GarbageCollector {

    private observers: any[] = [];

    constructor() {

    }

    public attach(callback): void {
        this.observers.push(callback);
    }

    public notify(): void {
        this.observers.forEach(callback => {
            callback();
        });
    }
}
