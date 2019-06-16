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
    this.result.labelCount = 4;
    this.result.combinedGif = example1 + 'Z.gif';
    this.result.gifs = [
      new AxisContainerModelOfString({
        axisType: AxisType.Z,
        entity: example1 + 'Z.gif'
      })
    ];
    this.result.images = [
      new PositionAxisContainerModelOfString({
        axisType: AxisType.Z,
        entity: example1 + 'Z_0.png',
        position: 0
      }),
      new PositionAxisContainerModelOfString({
        axisType: AxisType.Z,
        entity: example1 + 'Z_1.png',
        position: 1
      }),
      new PositionAxisContainerModelOfString({
        axisType: AxisType.Z,
        entity: example1 + 'Z_2.png',
        position: 2
      }),
      new PositionAxisContainerModelOfString({
        axisType: AxisType.Z,
        entity: example1 + 'Z_3.png',
        position: 3
      }),
      new PositionAxisContainerModelOfString({
        axisType: AxisType.Z,
        entity: example1 + 'Z_4.png',
        position: 4
      }),
      new PositionAxisContainerModelOfString({
        axisType: AxisType.Z,
        entity: example1 + 'Z_5.png',
        position: 5
      }),
      new PositionAxisContainerModelOfString({
        axisType: AxisType.Z,
        entity: example1 + 'Z_6.png',
        position: 6
      }),
      new PositionAxisContainerModelOfString({
        axisType: AxisType.Z,
        entity: example1 + 'Z_7.png',
        position: 7
      }),
      new PositionAxisContainerModelOfString({
        axisType: AxisType.Z,
        entity: example1 + 'Z_8.png',
        position: 8
      }),
      new PositionAxisContainerModelOfString({
        axisType: AxisType.Z,
        entity: example1 + 'Z_9.png',
        position: 9
      })
    ]
  }
}

