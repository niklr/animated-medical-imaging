import { Component, OnInit, AfterViewInit } from '@angular/core';
import { WebhookModel } from '../../../clients/ami-api-client';
import { EventType } from '../../../enums';
import { PageEvent } from '../../../events/page.event';
import { IKeyedCollection, KeyedCollection } from '../../../extensions';
import { WebhookModelExtended } from '../../../models/webhook-extended.model';
import { NotificationService } from '../../../services/notification.service';
import { WebhookProxy } from '../../../proxies/webhook.proxy';
import { MomentUtil } from '../../../utils';

@Component({
    selector: 'app-user-webhooks',
    templateUrl: './webhooks.component.html'
})
export class UserWebhooksComponent implements OnInit, AfterViewInit {

    public isChecked = true;
    public webhooks: IKeyedCollection<WebhookModelExtended> = new KeyedCollection<WebhookModelExtended>();
    public currentWebhook: WebhookModelExtended = new WebhookModelExtended();

    // MatPaginator Output
    public pageEvent: PageEvent;

    constructor(private notificationService: NotificationService, private momentUtil: MomentUtil, private webhookProxy: WebhookProxy) {
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

    private initDropdown(): void {
        setTimeout(() => {
            M.Dropdown.init(document.querySelector('#webhookActionsDropdownButton'), {});
            M.Modal.init(document.querySelector('#webhookCreateButton'), {});
        });
    }

    private init(): void {
        this.setPage(this.pageEvent);
    }

    private afterInit(): void {
        this.initDropdown();
        // Mark all objects as checked by default
        this.isChecked = true;
        const items = this.webhooks.values();
        if (items) {
            for (const item of items) {
                item.isChecked = true;
            }
        }
    }

    public toggleCheckbox(): void {
        this.isChecked = !this.isChecked;
    }

    public deleteSelected(): void {

    }

    public openModal(webhook: WebhookModel): void {
        this.currentWebhook = new WebhookModelExtended(webhook);
    }

    public setPage(event: PageEvent): void {
        this.webhooks = new KeyedCollection<WebhookModelExtended>();

        this.pageEvent = event;
        this.webhookProxy.getPaginated(this.pageEvent.pageIndex, this.pageEvent.pageSize).then(result => {
            this.pageEvent.previousPageIndex = this.pageEvent.pageIndex;
            this.pageEvent.pageIndex = result.pagination.page;
            this.pageEvent.pageSize = result.pagination.limit;
            this.pageEvent.length = result.pagination.total;
            if (result.items) {
                result.items.forEach(webhook => {
                    this.webhooks.add(webhook.id, webhook as WebhookModelExtended);
                });
            }
            this.afterInit();
        }, error => {
            this.notificationService.handleError(error);
        });
    }

    public addOrUpdate(webhook: WebhookModel): void {
        if (webhook) {
            if (this.webhooks.containsKey(webhook.id)) {
                this.webhooks.update(webhook.id, webhook as WebhookModelExtended);
            } else {
                this.webhooks.add(webhook.id, webhook as WebhookModelExtended);
            }
        }
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
