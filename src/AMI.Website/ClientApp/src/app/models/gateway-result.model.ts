export interface IGatewayResult<T> {
    op: string;
    t: string;
    d: T;
}

export class GatewayResult<T> implements IGatewayResult<T> {
    public op: string;
    public t: string;
    public d: T;

    public constructor(init?: Partial<IGatewayResult<T>>) {
        Object.assign(this, init);
    }
}
