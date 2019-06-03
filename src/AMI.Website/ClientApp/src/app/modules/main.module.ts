import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainRoutingModule } from './main-routing.module';
import { HomeModule } from './home/home.module';
import { LayoutComponent } from '../shared/modules/layout/layout.component';
import { TopnavComponent } from '../shared/modules/layout/components/topnav/topnav.component';
import { FooterComponent } from '../shared/modules/layout/components/footer/footer.component';

@NgModule({
  imports: [
    CommonModule,
    MainRoutingModule,
    HomeModule
  ],
  declarations: [
    LayoutComponent,
    TopnavComponent,
    FooterComponent
  ],
  providers: [

  ]
})
export class AppMainModule { }
