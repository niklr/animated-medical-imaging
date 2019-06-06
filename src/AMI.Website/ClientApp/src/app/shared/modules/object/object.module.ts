import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProcessResultComponent } from './process-result/process-result.component';
import { ProcessSettingsComponent } from './process-settings/process-settings.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule
  ],
  declarations: [
    ProcessResultComponent,
    ProcessSettingsComponent
  ],
  providers: [

  ],
  exports: [
    ProcessResultComponent,
    ProcessSettingsComponent
  ]
})
export class ObjectModule { }
