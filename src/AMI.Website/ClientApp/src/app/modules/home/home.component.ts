import { Component, OnInit, AfterViewInit } from '@angular/core';

import M from 'materialize-css';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit, AfterViewInit {

  constructor() { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.initCollapsible();
  }

  private initCollapsible(): void {
    var options = {
      accordion: false
    };
    var elem = document.querySelector('.collapsible.expandable');
    var instance = M.Collapsible.init(elem, options);
  }

}
