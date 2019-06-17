import { Component, OnInit, AfterViewInit } from '@angular/core';
import {
  AxisType,
  AxisContainerModelOfString,
  ProcessResultModel,
  PositionAxisContainerModelOfString
} from '../../../../clients/ami-api-client';

@Component({
  selector: 'app-processing-result',
  templateUrl: './result.component.html'
})
export class ResultComponent implements OnInit, AfterViewInit {

  result: ProcessResultModel = new ProcessResultModel();
  extendedResult: any = {};

  constructor() {
    this.initDemoResult();
    this.initExtendedResult();
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
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
    // X-Axis
    this.extendedResult.xAxisGif = this.result.gifs.filter((value) => {
      return value.axisType === AxisType.X;
    }).shift();
    this.extendedResult.xAxisImages = this.result.images.filter((value) => {
      return value.axisType === AxisType.X;
    });

    // Y-Axis
    this.extendedResult.yAxisGif = this.result.gifs.filter((value) => {
      return value.axisType === AxisType.Y;
    }).shift();
    this.extendedResult.yAxisImages = this.result.images.filter((value) => {
      return value.axisType === AxisType.Y;
    });

    // Z-Axis
    this.extendedResult.zAxisGif = this.result.gifs.filter((value) => {
      return value.axisType === AxisType.Z;
    }).shift();
    this.extendedResult.zAxisImages = this.result.images.filter((value) => {
      return value.axisType === AxisType.Z;
    });

    // Other
    this.extendedResult.showCombinedGif = (this.extendedResult.xAxisGif ? 1 : 0)
      + (this.extendedResult.yAxisGif ? 1 : 0) + (this.extendedResult.zAxisGif ? 1 : 0) > 1;
  }

  private initDemoResult(): void {
    var example1 = 'https://raw.githubusercontent.com/niklr/animated-medical-imaging/master/assets/images/example1/';
    var example2 = 'https://raw.githubusercontent.com/niklr/animated-medical-imaging/master/assets/images/example2/';
    this.result.labelCount = 4;
    this.result.combinedGif = example1 + 'Z.gif';
    this.result.gifs = [
      new AxisContainerModelOfString({
        axisType: AxisType.X,
        entity: example1 + 'Z.gif'
      }),
      new AxisContainerModelOfString({
        axisType: AxisType.Y,
        entity: example2 + 'Z.gif'
      }),
      new AxisContainerModelOfString({
        axisType: AxisType.Z,
        entity: example1 + 'Z.gif'
      })
    ];

    this.result.images = [];

    for (var i = 0; i < 10; i++) {
      this.result.images.push(this.createDemoItem(AxisType.X, example1, i));
    }

    for (var i = 0; i < 10; i++) {
      this.result.images.push(this.createDemoItem(AxisType.Y, example2, i));
    }

    for (var i = 0; i < 10; i++) {
      this.result.images.push(this.createDemoItem(AxisType.Z, example1, i));
    }
  }

  private createDemoItem(axisType: AxisType, basePath: string, position: number): PositionAxisContainerModelOfString {
    return new PositionAxisContainerModelOfString({
      axisType: axisType,
      entity: basePath + 'Z_' + position + '.png',
      position: position
    });
  }
}

