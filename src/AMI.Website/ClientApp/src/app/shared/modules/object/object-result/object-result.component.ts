import { Component, OnInit, AfterViewInit, Input } from '@angular/core';

import M from 'materialize-css';

@Component({
  selector: 'app-object-result',
  templateUrl: './object-result.component.html'
})
export class ObjectResultComponent implements OnInit, AfterViewInit {

  @Input() result: any;

  results = [];

  constructor() {
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {

  }
}

