import { Component, OnInit, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html'
})
export class UserComponent implements OnInit, AfterViewInit {

  constructor() {

  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    this.initMaterialize();
  }

  private initMaterialize(): void {
  }

}
