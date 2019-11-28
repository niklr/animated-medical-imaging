import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClickOnceDirective } from '../../directives';
import { ClipboardUtil, GuidUtil, UrlUtil } from '../../utils';
import { GuidBadgeComponent } from './guid-badge/guid-badge.component';
import { PaginationComponent } from './paginator/paginator.component';
import { TrimTextComponent } from './trim/trim.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule
  ],
  declarations: [
    ClickOnceDirective,
    GuidBadgeComponent,
    PaginationComponent,
    TrimTextComponent
  ],
  providers: [
    ClipboardUtil,
    GuidUtil,
    UrlUtil
  ],
  exports: [
    ClickOnceDirective,
    GuidBadgeComponent,
    PaginationComponent,
    TrimTextComponent
  ]
})
export class SharedCommonModule { }
