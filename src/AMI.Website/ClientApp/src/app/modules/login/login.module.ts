import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';
import { SharedCommonModule } from '../common/common.module';

@NgModule({
  declarations: [
    LoginComponent
  ],
  imports: [
    CommonModule,
    SharedCommonModule,
    ReactiveFormsModule,
    LoginRoutingModule
  ],
  providers: [

  ]
})
export class LoginModule { }
