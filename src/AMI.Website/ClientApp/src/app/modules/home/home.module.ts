import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { DatatransferModule } from '../../shared/modules/datatransfer/datatransfer.module';
import { ProcessingModule } from '../../shared/modules/processing/processing.module';

@NgModule({
  imports: [
    CommonModule,
    DatatransferModule,
    ProcessingModule
  ],
  declarations: [
    HomeComponent
  ]
})
export class HomeModule { }
