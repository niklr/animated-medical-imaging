export interface IGatewayResult<T> {
    op: number;
    t: string;
    d: T;
}

export class GatewayResult<T> implements IGatewayResult<T> {
    public op: number;
    public t: string;
    public d: T;

    public constructor(init?: Partial<IGatewayResult<T>>) {
        Object.assign(this, init);
    }
}
