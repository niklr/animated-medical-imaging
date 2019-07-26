// https://www.dustinhorne.com/post/2016/06/09/implementing-a-dictionary-in-typescript

export interface IKeyedCollection<T> {
    add(key: string, value: T): void;
    containsKey(key: string): boolean;
    count(): number;
    item(key: string): T;
    keys(): string[];
    remove(key: string): T;
    values(): T[];
    sort(compareFn: (a: T, b: T) => number): void;
}

export class KeyedCollection<T> implements IKeyedCollection<T> {
    private _items: { [index: string]: T } = {};
    private _keys: string[] = [];
    private _values: T[] = [];

    private _count = 0;

    public containsKey(key: string): boolean {
        return this._items.hasOwnProperty(key);
    }

    public count(): number {
        return this._count;
    }

    public add(key: string, value: T): void {
        if (!this.containsKey(key)) {
            this._count++;
        }

        this._items[key] = value;
        this.update();
    }

    public remove(key: string): T {
        if (this.containsKey(key)) {
            const val = this._items[key];
            delete this._items[key];
            this._count--;
            this.update();
            return val;
        } else {
            return undefined;
        }
    }

    public item(key: string): T {
        return this._items[key];
    }

    public keys(): string[] {
        // return cloned values
        return Object.assign([], this._keys);
    }

    public values(): T[] {
        // return cloned values
        return Object.assign([], this._values);
    }

    public sort(compareFn: (a: T, b: T) => number): void {
        this._values = this._values.sort(compareFn);
    }

    private update(): void {
        this._keys = [];
        this._values = [];

        for (const prop in this._items) {
            if (this._items.hasOwnProperty(prop)) {
                this._keys.push(prop);
                this._values.push(this._items[prop]);
            }
        }
    }
}
