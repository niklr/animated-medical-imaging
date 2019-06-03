import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout.component';
import { TopnavComponent } from './components/topnav/topnav.component';
import { FooterComponent } from './components/footer/footer.component';

@NgModule({
  imports: [
    CommonModule
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
