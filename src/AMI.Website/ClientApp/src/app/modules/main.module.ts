import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainRoutingModule } from './main-routing.module';
import { HomeModule } from './home/home.module';
import { AppLayoutModule } from '../shared/modules/layout/layout.module';

@NgModule({
  imports: [
    CommonModule,
    MainRoutingModule,
    HomeModule,
    AppLayoutModule
  ],
  declarations: [

  ],
  providers: [

  ]
})
export class AppMainModule { }
