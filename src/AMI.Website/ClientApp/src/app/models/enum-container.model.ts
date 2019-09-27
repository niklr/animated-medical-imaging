export class EnumContainer {
  displayName: string;
  enum: any;
  checked: boolean;
  public constructor(init?: Partial<EnumContainer>) {
    Object.assign(this, init);
  }
}