import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClickOnceDirective } from '../../directives';
import { ClipboardUtil, GuidUtil } from '../../utils';
import { GuidBadgeComponent } from './guid-badge/guid-badge.component';
import { PaginationComponent } from './paginator/paginator.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule
  ],
  declarations: [
    ClickOnceDirective,
    GuidBadgeComponent,
    PaginationComponent
  ],
  providers: [
    ClipboardUtil,
    GuidUtil
  ],
  exports: [
    ClickOnceDirective,
    GuidBadgeComponent,
    PaginationComponent
  ]
})
export class SharedCommonModule { }
