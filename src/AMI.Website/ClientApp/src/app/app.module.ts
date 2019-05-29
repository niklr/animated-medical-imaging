import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AppLayoutModule } from './modules/layout/layout.module';
import { DatatransferModule } from './modules/datatransfer/datatransfer.module';

@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    AppLayoutModule,
    DatatransferModule
  ],
  declarations: [
    AppComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
