import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { GuidUtil } from '../../../../utils';

import M from 'materialize-css';
import * as $ from 'jquery';

@Component({
  selector: '[app-processing-object]',
  templateUrl: './object.component.html'
})
export class ObjectComponent implements OnInit, AfterViewInit {

  @Input() object: any;

  private dropdownButtonGuid: string;
  private dropdownTargetGuid: string;

  constructor(private guidUtil: GuidUtil) {
    this.dropdownButtonGuid = this.guidUtil.createGuid();
    this.dropdownTargetGuid = this.guidUtil.createGuid();
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    this.initDropdown();
  }

  private initDropdown(): void {
    var options = {};
    var instance = M.Dropdown.init($('#' + this.dropdownButtonGuid), options);
  }

  public download(): void {
    console.log("Download " + this.object.id);
  }
}

