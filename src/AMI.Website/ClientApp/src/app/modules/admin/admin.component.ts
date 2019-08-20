import { Component, OnInit, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html'
})
export class AdminComponent implements OnInit, AfterViewInit {

  constructor() {

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

}
