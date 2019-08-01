import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ConnectionState } from '../../enums';
import { GatewayHub } from '../../hubs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit, AfterViewInit {

  constructor(private gateway: GatewayHub) { }

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
      const elem = document.querySelector('.collapsible.expandable');
      const instance = M.Collapsible.init(elem, options);
    });
  }

  public get connectionState(): ConnectionState {
    return this.gateway.connectionState;
  }

}
