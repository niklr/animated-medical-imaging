import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { ClipboardUtil, GuidUtil } from '../../../../utils';

import M from 'materialize-css';
import * as $ from 'jquery';

@Component({
  selector: 'app-common-guid-badge',
  templateUrl: './guid-badge.component.html'
})
export class GuidBadgeComponent implements OnInit, AfterViewInit {

  @Input() guid: any;

  public tooltipGuid: string;

  public guidShortened: string;

  constructor(private clipboardUtil: ClipboardUtil, private guidUtil: GuidUtil) {
    this.tooltipGuid = this.guidUtil.createGuid();
  }

  ngOnInit() {
    this.guidShortened = this.guid.slice(0, 8);
  }

  ngAfterViewInit(): void {
    this.initMaterialize();
  }

  private initMaterialize(): void {
    M.Tooltip.init($('#' + this.tooltipGuid), {});
  }

  public copyToClipboard(): void {
    this.clipboardUtil.copy(this.guid);
    M.toast({ html: this.guid + ' copied to clipboard.', classes: 'rounded' });
  }
}
