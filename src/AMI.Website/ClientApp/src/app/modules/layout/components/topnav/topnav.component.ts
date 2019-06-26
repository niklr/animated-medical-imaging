import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-topnav',
  templateUrl: './topnav.component.html',
  styleUrls: ['./topnav.component.scss']
})
export class TopnavComponent implements OnInit, AfterViewInit {

  constructor(public router: Router) {

  }

  ngOnInit() {

  }

  ngAfterViewInit(): void {
    this.initSidenav();
  }

  private initSidenav(): void {
    setTimeout(() => {
      var elems = document.querySelectorAll('.sidenav');
      var options = {};
      var instances = M.Sidenav.init(elems, options);
    });
  }
}
