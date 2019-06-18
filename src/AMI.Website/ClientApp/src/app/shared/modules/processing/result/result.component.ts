import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import {
  AxisType,
  ProcessResultModel
} from '../../../../clients/ami-api-client';

@Component({
  selector: 'app-processing-result',
  templateUrl: './result.component.html'
})
export class ResultComponent implements OnInit, AfterViewInit {

  @Input() result: ProcessResultModel;
  
  extendedResult: any = {};

  constructor() {
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    if (this.result) {
      this.initExtendedResult();
    }
    this.initMaterialbox();
  }

  private initMaterialbox(): void {
    setTimeout(() => {
      var options = {};
      var elems = document.querySelectorAll('.materialboxed');
      var instance = M.Materialbox.init(elems, options);
    });
  }

  private initExtendedResult(): void {
    var extendedResult: any = {};
    // Filter gifs
    if (this.result.gifs) {
      extendedResult.xAxisGif = this.result.gifs.filter((value) => {
        return value.axisType === AxisType.X;
      }).shift();
      extendedResult.yAxisGif = this.result.gifs.filter((value) => {
        return value.axisType === AxisType.Y;
      }).shift();
      extendedResult.zAxisGif = this.result.gifs.filter((value) => {
        return value.axisType === AxisType.Z;
      }).shift();
    }

    // Filter images
    if (this.result.images) {
      extendedResult.xAxisImages = this.result.images.filter((value) => {
        return value.axisType === AxisType.X;
      });
      extendedResult.yAxisImages = this.result.images.filter((value) => {
        return value.axisType === AxisType.Y;
      });
      extendedResult.zAxisImages = this.result.images.filter((value) => {
        return value.axisType === AxisType.Z;
      });
    }

    // Other
    extendedResult.showCombinedGif = (extendedResult.xAxisGif ? 1 : 0)
      + (extendedResult.yAxisGif ? 1 : 0) + (extendedResult.zAxisGif ? 1 : 0) > 1;

    setTimeout(() => {
      this.extendedResult = extendedResult;
    });
  }
}

