import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { GuidUtil, MomentUtil } from '../../../../utils';

import M from 'materialize-css';
import * as $ from 'jquery';

@Component({
  selector: 'app-processing-task',
  templateUrl: './task.component.html'
})
export class TaskComponent implements OnInit, AfterViewInit {

  @Input() task: any;

  constructor(private guidUtil: GuidUtil, private momentUtil: MomentUtil) {

  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    this.initMaterialize();
  }

  private initMaterialize(): void {

  }
}
