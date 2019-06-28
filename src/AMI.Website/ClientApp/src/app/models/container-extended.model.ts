export class ContainerModelExtended {
  entityUrl: string;
  index: number;

  public constructor(init?: Partial<ContainerModelExtended>) {
    Object.assign(this, init);
  }
}
