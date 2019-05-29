import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatButtonModule,
  MatSelectModule
} from '@angular/material';
import { HomeComponent } from './home.component';
import { DatatransferModule } from '../../datatransfer/datatransfer.module';

@NgModule({
  imports: [
    CommonModule,
    MatButtonModule,
    MatSelectModule,
    DatatransferModule
  ],
  declarations: [
    HomeComponent
  ]
})
export class HomeModule { }
