import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout.component';
import { TopnavComponent } from './components/topnav/topnav.component';
import { FooterComponent } from './components/footer/footer.component';
import { LayoutRoutingModule } from './layout-routing.module';
import { HomeModule } from './home/home.module';
import { AppProxy } from '../../proxies/app.proxy';

@NgModule({
  imports: [
    CommonModule,
    LayoutRoutingModule,
    HomeModule
  ],
  declarations: [
    LayoutComponent,
    TopnavComponent,
    FooterComponent
  ],
  providers: [
    AppProxy
  ]
})
export class AppLayoutModule { }
