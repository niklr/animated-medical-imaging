import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProcessSettingsComponent } from './process-settings/process-settings.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule
  ],
  declarations: [
    ProcessSettingsComponent
  ],
  providers: [

  ],
  exports: [
    ProcessSettingsComponent
  ]
})
export class ObjectModule { }
