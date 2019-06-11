import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClipboardUtil, GuidUtil } from '../../../utils';
import { GuidBadgeComponent } from './guid-badge/guid-badge.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule
  ],
  declarations: [
    GuidBadgeComponent
  ],
  providers: [
    ClipboardUtil,
    GuidUtil
  ],
  exports: [
    GuidBadgeComponent
  ]
})
export class SharedCommonModule { }
