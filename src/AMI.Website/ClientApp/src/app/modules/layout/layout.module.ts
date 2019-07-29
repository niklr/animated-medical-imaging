import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { TopnavComponent } from './components/topnav/topnav.component';
import { FooterComponent } from './components/footer/footer.component';
import { SharedCommonModule } from '../common/common.module';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    SharedCommonModule
  ],
  declarations: [
    LayoutComponent,
    TopnavComponent,
    FooterComponent
  ],
  providers: [

  ]
})
export class AppLayoutModule { }
