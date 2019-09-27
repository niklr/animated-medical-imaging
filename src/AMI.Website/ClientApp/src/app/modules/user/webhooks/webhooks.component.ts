import { Component, OnInit, AfterViewInit } from '@angular/core';
import { WebhookModel } from '../../../clients/ami-api-client';
import { PageEvent } from '../../../events/page.event';
import { IKeyedCollection, KeyedCollection } from '../../../extensions';
import { WebhookModelExtended } from '../../../models/webhook-extended.model';
import { NotificationService } from '../../../services/notification.service';
import { WebhookProxy } from '../../../proxies/webhook.proxy';

@Component({
    selector: 'app-user-webhooks',
    templateUrl: './webhooks.component.html'
})
export class UserWebhooksComponent implements OnInit, AfterViewInit {

    public isChecked = false;
    public webhooks: IKeyedCollection<WebhookModelExtended> = new KeyedCollection<WebhookModelExtended>();
    public currentWebhook: WebhookModelExtended = new WebhookModelExtended();

    // MatPaginator Output
    public pageEvent: PageEvent;

    private webhookModalContainer: any;

    constructor(private notificationService: NotificationService, private webhookProxy: WebhookProxy) {
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
            this.webhookModalContainer = M.Modal.init(document.querySelector('#webhookModalContainer'), {});
        });
    }

    private init(): void {
        this.setPage(this.pageEvent);
    }

    private afterInit(): void {
        this.initDropdown();
    }

    public toggleCheckbox(): void {
        this.isChecked = !this.isChecked;
        const items = this.webhooks.values();
        if (items) {
            for (const item of items) {
                item.isChecked = this.isChecked;
            }
        }
    }

    public deleteSelected(): void {

    }

    public openModal(webhook: WebhookModel): void {
        this.currentWebhook = new WebhookModelExtended(webhook);
        this.webhookModalContainer.open();
        setTimeout(() => {
            M.updateTextFields();
        });
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
        this.webhookModalContainer.close();
    }

    public refresh(): void {
        this.setPage(new PageEvent());
    }
}
