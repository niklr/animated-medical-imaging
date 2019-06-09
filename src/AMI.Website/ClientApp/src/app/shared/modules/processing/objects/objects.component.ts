import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { ProcessResult, AxisContainerOfString, AxisType, PositionAxisContainerOfString } from '../../../../clients/ami-api-client';

import * as $ from 'jquery';
import 'datatables.net';
import M from 'materialize-css';

@Component({
  selector: 'app-processing-objects',
  templateUrl: './objects.component.html'
})
export class ObjectsComponent implements OnInit, AfterViewInit {

  @Input() tableId: string;

  results = [];

  private table: any;

  constructor() {
    this.initDemoResults();
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    this.initDataTable();
  }

  private initDataTable(): void {

  }

  private initDemoResults(): void {
    var result1 = {
      id: '19f06aa9-1856-4cc9-ba30-1ea0483fc154',
      filename: 'SMIR.Brain.XX.O.MR_Flair.36620.mha',
      createdDate: '2019-05-17T07:52:41.4700000Z'
    };

    var result2 = {
      id: 'a8bfea94-614b-466c-9400-3ace2d5e4f06',
      filename: 'SMIR.Brain.XX.O.CT.346124.nii',
      createdDate: '2019-04-17T07:52:41.4700000Z'
    };

    this.results = [result1, result2];
  }
}

