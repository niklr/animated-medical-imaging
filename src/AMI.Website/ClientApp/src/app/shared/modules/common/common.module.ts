import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClickOnceDirective } from '../../../directives';
import { ClipboardUtil, GuidUtil } from '../../../utils';
import { GuidBadgeComponent } from './guid-badge/guid-badge.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule
  ],
  declarations: [
    ClickOnceDirective,
    GuidBadgeComponent
  ],
  providers: [
    ClipboardUtil,
    GuidUtil
  ],
  exports: [
    ClickOnceDirective,
    GuidBadgeComponent
  ]
})
export class SharedCommonModule { }
