import { Component, OnInit } from '@angular/core';
import { AppProxy } from './proxies/app.proxy';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(private appProxy: AppProxy) {
    this.appProxy.init(0);
  }

  ngOnInit(): void {

  }
}
