import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { CallbackWrapper } from '../../wrappers/callback.wrapper';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit, AfterViewInit {

  loginFormGroup = this.fb.group({
    username: ['', {
      validators: Validators.required
    }],
    password: ['', {
      validators: Validators.required
    }]
  });

  constructor(private router: Router, private fb: FormBuilder, private authService: AuthService) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
  }

  public login = (callbackFn) => {
    const callbackWrapper = new CallbackWrapper(callbackFn);
    if (this.loginFormGroup.valid) {
      const username = this.loginFormGroup.get('username').value;
      const password = this.loginFormGroup.get('password').value;
      if (!!username && !!password) {
        this.authService.login(username, password).then(
          (s) => {
            setTimeout(() => {
              callbackWrapper.invokeCallbackFn();
              this.router.navigate(['/']);
            }, 100);
          },
          (e) => {
            callbackWrapper.invokeCallbackFn();
            this.loginFormGroup.reset();
          });
      }
    } else {
      setTimeout(() => {
        callbackWrapper.invokeCallbackFn();
      }, 100);
    }
  }

  public anonymous = (callbackFn) => {
    const callbackWrapper = new CallbackWrapper(callbackFn);
    this.authService.logout();
    this.authService.init().then(
      (s) => {
        setTimeout(() => {
          callbackWrapper.invokeCallbackFn();
          this.router.navigate(['/']);
        }, 100);
      },
      (e) => {
        callbackWrapper.invokeCallbackFn();
        this.loginFormGroup.reset();
      });
  }
}
