import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ObjectComponent } from './object/object.component';
import { ObjectsComponent } from './objects/objects.component';
import { ResultComponent } from './result/result.component';
import { SettingsComponent } from './settings/settings.component';
import { GuidUtil, MomentUtil } from '../../../utils';

@NgModule({
  imports: [
    CommonModule,
    FormsModule
  ],
  declarations: [
    ObjectComponent,
    ObjectsComponent,
    ResultComponent,
    SettingsComponent
  ],
  providers: [
    GuidUtil,
    MomentUtil
  ],
  exports: [
    ObjectComponent,
    ObjectsComponent,
    ResultComponent,
    SettingsComponent
  ]
})
export class ProcessingModule { }
