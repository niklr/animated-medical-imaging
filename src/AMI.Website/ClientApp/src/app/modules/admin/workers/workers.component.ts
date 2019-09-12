import { Component, OnInit, AfterViewInit } from '@angular/core';
import { QueueWorkerModel, RecurringWorkerModel, WorkerType } from '../../../clients/ami-api-client';
import { PageEvent } from '../../../events/page.event';
import { IKeyedCollection, KeyedCollection } from '../../../extensions';
import { NotificationService } from '../../../services/notification.service';
import { AdminProxy } from '../../../proxies/admin.proxy';
import { MomentUtil } from '../../../utils';

@Component({
    selector: 'app-admin-workers',
    templateUrl: './workers.component.html'
})
export class AdminWorkersComponent implements OnInit, AfterViewInit {

    queueWorkers: IKeyedCollection<QueueWorkerModel> = new KeyedCollection<QueueWorkerModel>();
    recurringWorkers: IKeyedCollection<RecurringWorkerModel> = new KeyedCollection<RecurringWorkerModel>();

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
        this.queueWorkers = new KeyedCollection<QueueWorkerModel>();
        this.recurringWorkers = new KeyedCollection<RecurringWorkerModel>();

        this.pageEvent = event;
        this.adminProxy.getWorkers(this.pageEvent.pageIndex, this.pageEvent.pageSize).then(result => {
            this.pageEvent.previousPageIndex = this.pageEvent.pageIndex;
            this.pageEvent.pageIndex = result.pagination.page;
            this.pageEvent.pageSize = result.pagination.limit;
            this.pageEvent.length = result.pagination.total;
            if (result.items) {
                result.items.forEach(worker => {
                    switch (worker.workerType) {
                        case WorkerType.Queue:
                            this.queueWorkers.add(worker.id, worker);
                            break;
                        case WorkerType.Recurring:
                            this.recurringWorkers.add(worker.id, worker);
                            break;
                        default:
                            break;
                    }
                });
            }
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

}
