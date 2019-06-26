import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { DatatransferModule } from '../datatransfer/datatransfer.module';
import { ProcessingModule } from '../processing/processing.module';

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
