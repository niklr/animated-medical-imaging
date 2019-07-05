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
    });
  }

  private initExtendedResult(): void {
    // Extend gifs
    if (this.result.gifs) {
      for (var i = 0; i < this.result.gifs.length; i++) {
        var currentGif = this.result.gifs[i] as AxisContainerModelOfString & ContainerModelExtended;
        currentGif.entityUrl = this.buildEntityUrl(currentGif.entity);
        switch (currentGif.axisType) {
          case AxisType.X:
            this.result.xAxisGif = currentGif;
            break;
          case AxisType.Y:
            this.result.yAxisGif = currentGif;
            break;
          case AxisType.Z:
            this.result.zAxisGif = currentGif;
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
      for (var i = 0; i < this.result.images.length; i++) {
        var currentImage = this.result.images[i] as PositionAxisContainerModelOfString & ContainerModelExtended;
        currentImage.entityUrl = this.buildEntityUrl(currentImage.entity);
        switch (currentImage.axisType) {
          case AxisType.X:
            this.result.xAxisImages.push(currentImage);
            break;
          case AxisType.Y:
            this.result.yAxisImages.push(currentImage);
            break;
          case AxisType.Z:
            this.result.zAxisImages.push(currentImage);
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
    var pattern = /^((http|https):\/\/)/;
    if (entity && !pattern.test(entity)) {
      return ConfigService.options.apiEndpoint + '/results/' + this.result.id + '/images/' + entity;
    } else {
      return entity;
    }
  }

  private initPhotoSwipe(): void {
    var currentIndex = -1;
    currentIndex = this.addPhotoSwipeItem(currentIndex, this.result.combinedGifExtended);
    currentIndex = this.addPhotoSwipeItem(currentIndex, this.result.xAxisGif);
    for (var i = 0; i < this.result.xAxisImages.length; i++) {
      currentIndex = this.addPhotoSwipeItem(currentIndex, this.result.xAxisImages[i]);
    }
    currentIndex = this.addPhotoSwipeItem(currentIndex, this.result.yAxisGif);
    for (var i = 0; i < this.result.yAxisImages.length; i++) {
      currentIndex = this.addPhotoSwipeItem(currentIndex, this.result.yAxisImages[i]);
    }
    currentIndex = this.addPhotoSwipeItem(currentIndex, this.result.zAxisGif);
    for (var i = 0; i < this.result.zAxisImages.length; i++) {
      currentIndex = this.addPhotoSwipeItem(currentIndex, this.result.zAxisImages[i]);
    }
  }

  private addPhotoSwipeItem(index: number, container: ContainerModelExtended): number {
    if (container && container.entityUrl) {
      this.photoswipeItems.push(new PhotoSwipeItemModel({
        src: container.entityUrl,
        h: this.command.desiredSize,
        w: this.command.desiredSize
      }));
      container.index = ++index;
    }
    return index;
  }

  public openPhotoSwipe(container: ContainerModelExtended): void {
    var pswpElement = document.querySelectorAll('.pswp')[0];
    var options = {
      index: container.index,
      shareButtons: [
        { id: 'download', label: 'Download image', url: '{{raw_image_url}}', download: true }
      ]
    };
    var gallery = new PhotoSwipe(pswpElement, PhotoSwipeUI_Default, this.photoswipeItems, options);
    gallery.init();
  }
}
