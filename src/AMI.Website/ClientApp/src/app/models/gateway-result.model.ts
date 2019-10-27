export interface IGatewayResult<T> {
    eventType: string;
    data: T;
}

export class GatewayResult<T> implements IGatewayResult<T> {
    public eventType: string;
    public data: T;

    public constructor(init?: Partial<IGatewayResult<T>>) {
        Object.assign(this, init);
    }
}
