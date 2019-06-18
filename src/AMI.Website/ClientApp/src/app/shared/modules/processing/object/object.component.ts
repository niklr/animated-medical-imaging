import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { ObjectProxy } from '../../../../proxies/object.proxy';
import { NotificationService } from '../../../../services/notification.service';
import { ObjectStore } from '../../../../stores/object.store';
import { GuidUtil, MomentUtil } from '../../../../utils';
import { ObjectModel } from '../../../../clients/ami-api-client';

@Component({
  selector: '[app-processing-object]',
  templateUrl: './object.component.html'
})
export class ObjectComponent implements OnInit, AfterViewInit {

  @Input() object: ObjectModel;

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
    console.log("Download " + this.object.id);
  }

  public delete(): void {
    this.objectProxy.deleteObject(this.object.id).subscribe(result => {
      this.objectStore.deleteById(this.object.id);
    }, error => {
      this.notificationService.handleError(error);
    });
  }
}

