import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { ContainerModelExtended } from '../../../models/container-extended.model';
import { ProcessResultModelExtended } from '../../../models/result-extended.model';
import { ConfigService } from '../../../services/config.service';
import {
  AxisType, AxisContainerModelOfString, PositionAxisContainerModelOfString
} from '../../../clients/ami-api-client';

@Component({
  selector: 'app-processing-result',
  templateUrl: './result.component.html'
})
export class ResultComponent implements OnInit, AfterViewInit {

  @Input() result: ProcessResultModelExtended;

  constructor() {
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      if (this.result) {
        this.initExtendedResult();
      }
      this.initMaterialbox();
    });
  }

  private initMaterialbox(): void {
    setTimeout(() => {
      var options = {};
      var elems = document.querySelectorAll('.materialboxed');
      var instance = M.Materialbox.init(elems, options);
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
    this.result.combinedGifUrl = this.buildEntityUrl(this.result.combinedGif);
    this.result.showCombinedGif = (this.result.xAxisGif ? 1 : 0)
      + (this.result.yAxisGif ? 1 : 0) + (this.result.zAxisGif ? 1 : 0) > 1;
  }

  private buildEntityUrl(entity: string): string {
    var pattern = /^((http|https):\/\/)/;
    if (entity && !pattern.test(entity)) {
      return ConfigService.settings.apiEndpoint + '/results/' + this.result.id + '/images/' + entity;
    } else {
      return entity;
    }
  }
}
