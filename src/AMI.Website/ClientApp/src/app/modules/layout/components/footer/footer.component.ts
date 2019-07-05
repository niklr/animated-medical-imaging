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

  get appVersion(): string {
    return ConfigService.options.version;
  }

}
