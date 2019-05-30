import { Component, OnInit } from '@angular/core';
import { AppProxy } from '../../../../proxies/app.proxy';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html'
})
export class FooterComponent implements OnInit {

  constructor(private appProxy: AppProxy) { }

  ngOnInit() {
  }

  get appVersion(): string {
    return this.appProxy.getAppVersion();
  }

}
