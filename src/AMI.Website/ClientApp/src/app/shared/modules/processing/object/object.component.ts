import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { GuidUtil, MomentUtil } from '../../../../utils';

import M from 'materialize-css';
import * as $ from 'jquery';

@Component({
  selector: '[app-processing-object]',
  templateUrl: './object.component.html'
})
export class ObjectComponent implements OnInit, AfterViewInit {

  @Input() object: any;

  public dropdownButtonGuid: string;
  public dropdownTargetGuid: string;

  constructor(private guidUtil: GuidUtil, public momentUtil: MomentUtil) {
    this.dropdownButtonGuid = this.guidUtil.createGuid();
    this.dropdownTargetGuid = this.guidUtil.createGuid();
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    this.initMaterialize();
  }

  private initMaterialize(): void {
    M.Dropdown.init($('#' + this.dropdownButtonGuid), {});
  }

  public download(): void {
    console.log("Download " + this.object.id);
  }
}

