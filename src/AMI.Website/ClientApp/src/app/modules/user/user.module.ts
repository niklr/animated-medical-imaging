import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedCommonModule } from '../common/common.module';
import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user.component';
import { UserWebhooksComponent } from './webhooks/webhooks.component';
import { UserWebhookModalComponent } from './webhook-modal/webhook-modal.component';

@NgModule({
  declarations: [
    UserComponent,
    UserWebhooksComponent,
    UserWebhookModalComponent
  ],
  imports: [
    CommonModule,
    SharedCommonModule,
    FormsModule,
    UserRoutingModule
  ],
  exports: [
    UserComponent,
    UserWebhooksComponent,
    UserWebhookModalComponent
  ]
})
export class UserModule { }
