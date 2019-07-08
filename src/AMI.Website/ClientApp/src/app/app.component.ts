import { Component, OnInit } from '@angular/core';
import { BackgroundWorker } from './utils';
import { AppProxy } from './proxies/app.proxy';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(private appProxy: AppProxy, private worker: BackgroundWorker) {
    this.appProxy.init(0);
    this.worker.init();
  }

  ngOnInit(): void {

  }
}
