import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { ObjectModelExtended } from '../../../models/object-extended.model';
import { ObjectService } from '../../../services/object.service';
import { GuidUtil, MomentUtil } from '../../../utils';

@Component({
  selector: '[app-processing-object]',
  templateUrl: './object.component.html'
})
export class ObjectComponent implements OnInit, AfterViewInit {

  @Input() object: ObjectModelExtended;

  public dropdownButtonGuid: string;
  public dropdownTargetGuid: string;

  constructor(public momentUtil: MomentUtil, private objectService: ObjectService, private guidUtil: GuidUtil) {
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

  public process(): void {
    this.objectService.processObject(this.object.id, undefined);
  }

  public download(): void {
    this.objectService.downloadObject(this.object, undefined);
  }

  public delete(): void {
    this.objectService.deleteObject(this.object.id, undefined);
  }
}

