import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedCommonModule } from '../common/common.module';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { AdminEventsComponent } from './events/events.component';
import { AdminLogsComponent } from './logs/logs.component';
import { AdminWorkersComponent } from './workers/workers.component';

@NgModule({
  declarations: [
    AdminComponent,
    AdminEventsComponent,
    AdminLogsComponent,
    AdminWorkersComponent
  ],
  imports: [
    CommonModule,
    SharedCommonModule,
    FormsModule,
    AdminRoutingModule
  ],
  exports: [
    AdminComponent,
    AdminEventsComponent,
    AdminLogsComponent,
    AdminWorkersComponent
  ]
})
export class AdminModule { }
