import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ConnectionState } from '../../enums';
import { GatewayHub } from '../../hubs';
import { MomentUtil } from '../../utils';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit, AfterViewInit {

  constructor(private gateway: GatewayHub, private momentUtil: MomentUtil) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.initCollapsible();
  }

  private initCollapsible(): void {
    setTimeout(() => {
      const options = {
        accordion: false
      };
      M.Collapsible.init(document.querySelector('.collapsible.expandable'), options);
    });
  }

  public get isDisconnected(): boolean {
    // console.log(this.gateway.connectionState + ' ' + this.momentUtil.getDiffInSeconds(this.gateway.disconnectedDate));
    return this.gateway.connectionState == ConnectionState.Disconnected &&
      this.momentUtil.getDiffInSeconds(this.gateway.disconnectedDate) >= 5;
  }

}
