import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { ClipboardUtil, GuidUtil } from '../../../utils';
import { NotificationService } from '../../../services/notification.service';

@Component({
    selector: 'app-common-trim',
    templateUrl: './trim.component.html'
})
export class TrimTextComponent implements OnInit, AfterViewInit {

    @Input() text: any;
    @Input() length: number;

    public tooltipGuid: string;

    public textShortened: string;

    constructor(private notificationService: NotificationService, private clipboardUtil: ClipboardUtil, private guidUtil: GuidUtil) {
        this.tooltipGuid = this.guidUtil.createGuid();
    }

    ngOnInit() {
        if (this.text.length > this.length) {
            this.textShortened = this.text.slice(0, this.length) + '...';
        } else {
            this.textShortened = this.text;
        }
    }

    ngAfterViewInit(): void {
        this.initMaterialize();
    }

    private initMaterialize(): void {
        M.Tooltip.init($('#' + this.tooltipGuid), {});
    }

    public copyToClipboard(): void {
        this.clipboardUtil.copy(this.text);
        this.notificationService.raiseMessage(this.text + ' copied to clipboard.');
    }
}
