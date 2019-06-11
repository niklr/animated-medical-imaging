import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { ClipboardUtil, GuidUtil, MomentUtil } from '../../../../utils';

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

  private objectIdShort: string;

  constructor(private clipboardUtil: ClipboardUtil, private guidUtil: GuidUtil, private momentUtil: MomentUtil) {
    this.dropdownButtonGuid = this.guidUtil.createGuid();
    this.dropdownTargetGuid = this.guidUtil.createGuid();
  }

  ngOnInit() {
    this.objectIdShort = this.object.id.slice(0, 8);
  }

  ngAfterViewInit(): void {
    this.initMaterialize();
  }

  private initMaterialize(): void {
    M.Dropdown.init($('#' + this.dropdownButtonGuid), {});
  }

  public copyObjectId(): void {
    this.clipboardUtil.copy(this.object.id);
    M.toast({ html: this.object.id + ' copied to clipboard.', classes: 'rounded' });
  }

  public download(): void {
    console.log("Download " + this.object.id);
  }
}

