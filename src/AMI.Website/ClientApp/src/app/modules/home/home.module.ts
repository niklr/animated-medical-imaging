import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { DatatransferModule } from '../../shared/modules/datatransfer/datatransfer.module';

@NgModule({
  imports: [
    CommonModule,
    DatatransferModule
  ],
  declarations: [
    HomeComponent
  ]
})
export class HomeModule { }
