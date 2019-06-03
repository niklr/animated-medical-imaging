import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DatatransferComponent } from './datatransfer.component';

@NgModule({
  declarations: [DatatransferComponent],
  exports: [DatatransferComponent],
  imports: [
    CommonModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class DatatransferModule { }
