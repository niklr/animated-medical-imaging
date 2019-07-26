export interface ISetCollection<T> {
    add(value: T);
    count(): number;
    toArray(): T[];
}

export class SetCollection<T> implements ISetCollection<T> {
    private _set: Set<T> = new Set<T>();
    private _items: T[] = [];

    public count(): number {
        return this._set.size;
    }

    public add(value: T) {
        if (!!value) {
            this._set.add(value);
            this.update();
        }
    }

    public toArray(): T[] {
        // return cloned values
        return Object.assign([], this._items);
    }

    public sort(compareFn: (a: T, b: T) => number): void {
        this._items = this._items.sort(compareFn);
    }

    private update(): void {
        this._items = [];

        this._set.forEach((item) => {
            this._items.push(item);
        });
    }
}
