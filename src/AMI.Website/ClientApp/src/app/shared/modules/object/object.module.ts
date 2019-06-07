import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ObjectResultsComponent } from './object-results/object-results.component';
import { ProcessResultComponent } from './process-result/process-result.component';
import { ProcessSettingsComponent } from './process-settings/process-settings.component';
import { ObjectResultComponent } from './object-result/object-result.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule
  ],
  declarations: [
    ObjectResultComponent,
    ObjectResultsComponent,
    ProcessResultComponent,
    ProcessSettingsComponent
  ],
  providers: [

  ],
  exports: [
    ObjectResultComponent,
    ObjectResultsComponent,
    ProcessResultComponent,
    ProcessSettingsComponent
  ]
})
export class ObjectModule { }
