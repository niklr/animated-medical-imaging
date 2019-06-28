export class PhotoSwipeItemModel {
  src: string;
  w: number;
  h: number;

  public constructor(init?: Partial<PhotoSwipeItemModel>) {
    Object.assign(this, init);
  }
}
