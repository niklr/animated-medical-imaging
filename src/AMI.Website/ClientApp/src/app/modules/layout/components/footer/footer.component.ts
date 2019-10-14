import { Component, OnInit } from '@angular/core';
import { ConfigService } from '../../../../services/config.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html'
})
export class FooterComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  get showClientVersion(): boolean {
    return ConfigService.options.clientVersion !== ConfigService.options.serverVersion;
  }

  get clientVersion(): string {
    return ConfigService.options.clientVersion;
  }

  get serverVersion(): string {
    return ConfigService.options.serverVersion;
  }

}
