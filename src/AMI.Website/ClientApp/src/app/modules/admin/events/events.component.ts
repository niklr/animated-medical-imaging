import { Component, OnInit, AfterViewInit } from '@angular/core';
import { AuditEventModel, Target } from '../../../clients/ami-api-client';
import { PageEvent } from '../../../events/page.event';
import { NotificationService } from '../../../services/notification.service';
import { AdminProxy } from '../../../proxies/admin.proxy';
import { MomentUtil } from '../../../utils';

@Component({
  selector: 'app-admin-events',
  templateUrl: './events.component.html'
})
export class AdminEventsComponent implements OnInit, AfterViewInit {

  items: AuditEventModel[] = [];

  // MatPaginator Output
  pageEvent: PageEvent;

  constructor(private notificationService: NotificationService, private momentUtil: MomentUtil, private adminProxy: AdminProxy) {
    this.pageEvent = new PageEvent();
    this.pageEvent.pageSize = 50;
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.init();
    });
  }

  private init(): void {
    this.setPage(this.pageEvent);
  }

  public setPage(event: PageEvent): void {
    this.pageEvent = event;
    this.adminProxy.getAuditEvents(this.pageEvent.pageIndex, this.pageEvent.pageSize).then(result => {
      this.pageEvent.previousPageIndex = this.pageEvent.pageIndex;
      this.pageEvent.pageIndex = result.pagination.page;
      this.pageEvent.pageSize = result.pagination.limit;
      this.pageEvent.length = result.pagination.total;
      this.items = result.items;
    }, error => {
      this.notificationService.handleError(error);
    });
  }

  public refresh(): void {
    this.setPage(new PageEvent());
  }

  public getLocalDate(date: any): string {
    return this.momentUtil.getLocalDate(date);
  }

  public getLocalTime(date: any): string {
    return this.momentUtil.getLocalTime(date);
  }

  public stringify(value: any): string {
    return JSON.stringify(value, null, 4);
  }

  private convertKeyValuePair(obj: Object, el: any) {
    if (Array.isArray(el.value)) {
      const innerObject = {};
      Array.from(el.value, el1 => {
        this.convertKeyValuePair(innerObject, el1);
      });
      obj[el.name] = innerObject;
    } else {
      obj[el.name] = el.value;
    }
  }

  public stringifyTarget(target: Target): string {
    const newTarget = new Target();
    newTarget.entity = target.entity;
    newTarget.account = target.account;
    newTarget.data = {};
    if (!!target.data && Array.isArray(target.data)) {
      try {
        Array.from(target.data, x => {
          this.convertKeyValuePair(newTarget.data, x);
        });
      } catch (error) {
        newTarget.data = {};
      }
    }
    return JSON.stringify(newTarget, null, 4);
  }
}
