import { Component, OnInit, AfterViewInit, Input, Output, EventEmitter } from '@angular/core';
import { WebhookModelExtended } from '../../../models/webhook-extended.model';
import { GuidUtil, MomentUtil } from '../../../utils';

@Component({
    selector: '[app-user-webhook]',
    templateUrl: './webhook.component.html'
})
export class UserWebhookComponent implements OnInit, AfterViewInit {

    @Input() webhook: WebhookModelExtended;
    @Output() openModalEmitter: EventEmitter<any> = new EventEmitter();
    @Output() deleteEmitter: EventEmitter<any> = new EventEmitter();

    public dropdownButtonGuid: string;
    public dropdownTargetGuid: string;

    constructor(public momentUtil: MomentUtil, private guidUtil: GuidUtil) {
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

    public getLocalDate(date: any): string {
        return this.momentUtil.getLocalDate(date);
    }

    public getLocalTime(date: any): string {
        return this.momentUtil.getLocalTime(date);
    }

    public edit(): void {
        this.openModalEmitter.emit(this.webhook);
    }

    public delete(): void {
        this.deleteEmitter.emit(this.webhook);
    }
}

