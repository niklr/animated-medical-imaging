import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedCommonModule } from '../common/common.module';
import { ObjectComponent } from './object/object.component';
import { ObjectsComponent } from './objects/objects.component';
import { ResultComponent } from './result/result.component';
import { SettingsComponent } from './settings/settings.component';
import { TaskComponent } from './task/task.component';
import { ClipboardUtil, GuidUtil, MomentUtil } from '../../utils';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SharedCommonModule
  ],
  declarations: [
    ObjectComponent,
    ObjectsComponent,
    ResultComponent,
    SettingsComponent,
    TaskComponent
  ],
  providers: [
    ClipboardUtil,
    GuidUtil,
    MomentUtil
  ],
  exports: [
    ObjectComponent,
    ObjectsComponent,
    ResultComponent,
    SettingsComponent,
    TaskComponent
  ]
})
export class ProcessingModule { }
