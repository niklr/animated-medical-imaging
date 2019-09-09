import { Component, OnInit, AfterViewInit } from '@angular/core';
import { PageEvent } from '../../../events/page.event';
import { NotificationService } from '../../../services/notification.service';
import { MomentUtil } from '../../../utils';

@Component({
    selector: 'app-admin-workers',
    templateUrl: './workers.component.html'
})
export class AdminWorkersComponent implements OnInit, AfterViewInit {

    workerId = "e79e1283-2edc-4dd5-9ddd-e56f11b4820c";

    // MatPaginator Output
    pageEvent: PageEvent;

    constructor(private notificationService: NotificationService, private momentUtil: MomentUtil) {
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

}
