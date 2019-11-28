import { Component, OnInit, AfterViewInit } from '@angular/core';
import { AccountProxy } from '../../proxies/account.proxy';
import { TokenService } from '../../services/token.service';
import { CallbackWrapper } from '../../wrappers/callback.wrapper';
import { ConfigService } from 'src/app/services/config.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html'
})
export class AdminComponent implements OnInit, AfterViewInit {

  constructor(private accountProxy: AccountProxy, private tokenService: TokenService) {

  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    this.initMaterialize();
  }

  private initMaterialize(): void {
    setTimeout(() => {
      M.Tabs.init(document.querySelector('#adminTabs'), {});
    });
  }

  public openHangfireDashboard = (callbackFn) => {
    const redirectUrl = ConfigService.options.apiEndpoint + '/hangfire';
    const callbackWrapper = new CallbackWrapper(callbackFn);
    this.accountProxy.login(this.tokenService.getAccessToken(), redirectUrl).then(
      (s) => {
        setTimeout(() => {
          callbackWrapper.invokeCallbackFn();
        }, 100);
      },
      (e) => {
        setTimeout(() => {
          callbackWrapper.invokeCallbackFn();
        }, 100);
      });
  }

}
