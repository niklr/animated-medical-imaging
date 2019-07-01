import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { ObjectModelExtended } from '../../../models/object-extended.model';
import { ObjectProxy } from '../../../proxies';
import { NotificationService } from '../../../services/notification.service';
import { ObjectStore } from '../../../stores/object.store';
import { GuidUtil, MomentUtil } from '../../../utils';

@Component({
  selector: '[app-processing-object]',
  templateUrl: './object.component.html'
})
export class ObjectComponent implements OnInit, AfterViewInit {

  @Input() object: ObjectModelExtended;

  public dropdownButtonGuid: string;
  public dropdownTargetGuid: string;

  constructor(private guidUtil: GuidUtil, public momentUtil: MomentUtil,
    private notificationService: NotificationService, private objectProxy: ObjectProxy, private objectStore: ObjectStore) {
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
    this.objectProxy.downloadObject(this.object);
  }

  public delete(): void {
    this.objectProxy.deleteObject(this.object.id).subscribe(result => {
      this.objectStore.deleteById(this.object.id);
    }, error => {
      this.notificationService.handleError(error);
    });
  }
}

