import { Component, OnInit, AfterViewInit, Input } from '@angular/core';

import M from 'materialize-css';

@Component({
  selector: 'app-processing-object',
  templateUrl: './object.component.html'
})
export class ObjectComponent implements OnInit, AfterViewInit {

  @Input() result: any;

  results = [];

  constructor() {
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {

  }
}

