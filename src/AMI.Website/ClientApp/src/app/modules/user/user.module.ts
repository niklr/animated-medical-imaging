import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedCommonModule } from '../common/common.module';
import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user.component';
import { UserWebhooksComponent } from './webhooks/webhooks.component';

@NgModule({
  declarations: [
    UserComponent,
    UserWebhooksComponent
  ],
  imports: [
    CommonModule,
    SharedCommonModule,
    FormsModule,
    UserRoutingModule
  ],
  exports: [
    UserComponent,
    UserWebhooksComponent
  ]
})
export class UserModule { }
