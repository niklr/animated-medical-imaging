import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { ContainerModelExtended } from '../../../models/container-extended.model';
import { PhotoSwipeItemModel } from '../../../models/photoswipe-item.model';
import { ProcessResultModelExtended } from '../../../models/result-extended.model';
import { ConfigService } from '../../../services/config.service';
import {
  AxisType,
  AxisContainerModelOfString,
  PositionAxisContainerModelOfString,
  ProcessObjectCommand
} from '../../../clients/ami-api-client';

@Component({
  selector: 'app-processing-result',
  templateUrl: './result.component.html'
})
export class ResultComponent implements OnInit, AfterViewInit {

  @Input() result: ProcessResultModelExtended;
  @Input() command: ProcessObjectCommand;

  public photoswipeItems: PhotoSwipeItemModel[] = [];

  constructor() {
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      if (this.result) {
        this.initExtendedResult();
      }
    }, 500);
  }

  private initExtendedResult(): void {
    // Extend gifs
    if (this.result.gifs) {
      for (const gif of this.result.gifs) {
        const extendedGif = gif as AxisContainerModelOfString & ContainerModelExtended;
        extendedGif.entityUrl = this.buildEntityUrl(extendedGif.entity);
        switch (extendedGif.axisType) {
          case AxisType.X:
            this.result.xAxisGif = extendedGif;
            break;
          case AxisType.Y:
            this.result.yAxisGif = extendedGif;
            break;
          case AxisType.Z:
            this.result.zAxisGif = extendedGif;
            break;
          default:
            break;
        }
      }
    }

    // Extend images
    this.result.xAxisImages = [] as PositionAxisContainerModelOfString & ContainerModelExtended[];
    this.result.yAxisImages = [] as PositionAxisContainerModelOfString & ContainerModelExtended[];
    this.result.zAxisImages = [] as PositionAxisContainerModelOfString & ContainerModelExtended[];
    if (this.result.images) {
      for (const image of this.result.images) {
        const extendedImage = image as PositionAxisContainerModelOfString & ContainerModelExtended;
        extendedImage.entityUrl = this.buildEntityUrl(extendedImage.entity);
        switch (extendedImage.axisType) {
          case AxisType.X:
            this.result.xAxisImages.push(extendedImage);
            break;
          case AxisType.Y:
            this.result.yAxisImages.push(extendedImage);
            break;
          case AxisType.Z:
            this.result.zAxisImages.push(extendedImage);
            break;
          default:
            break;
        }
      }
    }

    // Other
    this.result.combinedGifExtended = new ContainerModelExtended({
      entityUrl: this.buildEntityUrl(this.result.combinedGif)
    });
    this.result.showCombinedGif = (this.result.xAxisGif ? 1 : 0)
      + (this.result.yAxisGif ? 1 : 0) + (this.result.zAxisGif ? 1 : 0) > 1;

    this.initPhotoSwipe();
  }

  private buildEntityUrl(entity: string): string {
    const pattern = /^((http|https):\/\/)/;
    if (entity && !pattern.test(entity)) {
      return ConfigService.options.apiEndpoint + '/results/' + this.result.id + '/images/' + entity;
    } else {
      return entity;
    }
  }

  private initPhotoSwipe(): void {
    let currentIndex = -1;
    currentIndex = this.addPhotoSwipeItem(currentIndex, this.result.combinedGifExtended);
    currentIndex = this.addPhotoSwipeItem(currentIndex, this.result.xAxisGif);
    for (const image of this.result.xAxisImages) {
      currentIndex = this.addPhotoSwipeItem(currentIndex, image);
    }
    currentIndex = this.addPhotoSwipeItem(currentIndex, this.result.yAxisGif);
    for (const image of this.result.yAxisImages) {
      currentIndex = this.addPhotoSwipeItem(currentIndex, image);
    }
    currentIndex = this.addPhotoSwipeItem(currentIndex, this.result.zAxisGif);
    for (const image of this.result.zAxisImages) {
      currentIndex = this.addPhotoSwipeItem(currentIndex, image);
    }
  }

  private addPhotoSwipeItem(index: number, container: ContainerModelExtended): number {
    if (container && container.entityUrl) {
      this.photoswipeItems.push(new PhotoSwipeItemModel({
        src: container.entityUrl,
        h: this.command.outputSize,
        w: this.command.outputSize
      }));
      container.index = ++index;
    }
    return index;
  }

  public openPhotoSwipe(container: ContainerModelExtended): void {
    const pswpElement = document.querySelectorAll('.pswp')[0];
    const options = {
      index: container.index,
      shareButtons: [
        { id: 'download', label: 'Download image', url: '{{raw_image_url}}', download: true }
      ]
    };
    const gallery = new PhotoSwipe(pswpElement, PhotoSwipeUI_Default, this.photoswipeItems, options);
    gallery.init();
  }
}
