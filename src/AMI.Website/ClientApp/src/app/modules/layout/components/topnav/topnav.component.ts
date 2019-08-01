import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { IIdTokenModel } from '../../../../clients/ami-api-client';
import { RoleType } from '../../../../enums';
import { AuthService } from '../../../../services/auth.service';
import { LoggerService } from '../../../../services/logger.service';

@Component({
  selector: 'app-topnav',
  templateUrl: './topnav.component.html',
  styleUrls: ['./topnav.component.scss']
})
export class TopnavComponent implements OnInit, AfterViewInit {

  constructor(public router: Router, private authService: AuthService, private logger: LoggerService) {
  }

  ngOnInit() {

  }

  ngAfterViewInit(): void {
    this.initMaterialize();
  }

  private initMaterialize(): void {
    setTimeout(() => {
      M.Sidenav.init(document.querySelectorAll('.sidenav'), {});
      M.Dropdown.init(document.querySelector('#dropdown-account-button'), { alignment: 'right' });
    });
  }

  public get identity(): IIdTokenModel {
    return this.authService.user;
  }

  public get isAdmin(): boolean {
    return !!this.authService.user && this.authService.user.isInRole(RoleType.Administrator);
  }

  public get isAuthenticated(): boolean {
    return this.authService.isAuthenticated;
  }

  public get showLogout(): boolean {
    return this.authService.isAuthenticated && !this.authService.user.isAnon;
  }

  public get showLogin(): boolean {
    return !this.authService.isAuthenticated || this.authService.user.isAnon;
  }

  public logout(): void {
    this.authService.logout();
    this.authService.init().then(() => {
    }, (e) => {
      this.logger.error(e);
    });
  }
}
