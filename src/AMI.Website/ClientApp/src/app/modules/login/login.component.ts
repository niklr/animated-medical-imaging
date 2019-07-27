import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

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

    ngOnInit() { }

    ngAfterViewInit() {
        setTimeout(async () => {
            this.authService.init().then(() => {
                if (this.authService.isAuthenticated) {
                    // this.router.navigate(['/']);
                }
            }, () => { });
        });
    }

    login($event: any): void {
        $event.preventDefault();
        if (this.loginFormGroup.valid) {
            const username = this.loginFormGroup.get('username').value;
            const password = this.loginFormGroup.get('password').value;
            if (!!username && !!password) {
                this.authService.login(username, password).then(
                    (s) => {
                        setTimeout(() => {
                            this.router.navigate(['/']);
                        }, 100);
                    },
                    (e) => {
                        this.loginFormGroup.reset();
                    });
            }
        }
    }
}
