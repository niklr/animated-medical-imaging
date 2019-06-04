import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { DatatransferModule } from '../../shared/modules/datatransfer/datatransfer.module';
import { ObjectModule } from '../../shared/modules/object/object.module';
import { ProcessSettingsComponent } from '../../shared/modules/object/process-settings/process-settings.component';

@NgModule({
  imports: [
    CommonModule,
    DatatransferModule,
    ObjectModule
  ],
  declarations: [
    HomeComponent
  ]
})
export class HomeModule { }
