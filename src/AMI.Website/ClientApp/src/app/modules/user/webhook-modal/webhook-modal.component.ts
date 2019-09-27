import { Component, OnInit, AfterViewInit, Input, Output, ViewChild, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';
import { WebhookModelExtended } from '../../../models/webhook-extended.model';
import { WebhookProxy } from '../../../proxies/webhook.proxy';
import { NotificationService } from '../../../services/notification.service';
import { CallbackWrapper } from '../../../wrappers/callback.wrapper';

@Component({
    selector: 'app-user-webhook-modal',
    templateUrl: './webhook-modal.component.html'
})
export class UserWebhookModalComponent implements OnInit, AfterViewInit {

    @Input() webhook: WebhookModelExtended;
    @Output() onSave: EventEmitter<any> = new EventEmitter();
    @ViewChild('webhookModalForm') public webhookModalForm: NgForm;

    constructor(private notificationService: NotificationService, private webhookProxy: WebhookProxy) {
    }

    ngOnInit() {
    }

    ngAfterViewInit() {
    }

    public save = (callbackFn) => {
        const callbackWrapper = new CallbackWrapper(callbackFn);
        this.webhookModalForm.onSubmit(undefined);
        if (this.webhookModalForm.valid) {
            if (this.webhook.id) {
                this.webhookProxy.update(this.webhook).then(result => {
                    this.onSave.emit(result);
                }, error => {
                  this.notificationService.handleError(error);
                }).finally(() => {
                    setTimeout(() => {
                        callbackWrapper.invokeCallbackFn();
                    }, 100);
                });
            } else {
                this.webhookProxy.create(this.webhook).then(result => {
                    this.onSave.emit(result);
                }, error => {
                  this.notificationService.handleError(error);
                }).finally(() => {
                    setTimeout(() => {
                        callbackWrapper.invokeCallbackFn();
                    }, 100);
                });
            }
        } else {
            setTimeout(() => {
                callbackWrapper.invokeCallbackFn();
            }, 100);
        }
    }
}
