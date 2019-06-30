import { Component, Input, Output, EventEmitter, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { PageEvent, PageEventNamespace } from '../../../events/page.event';
import { GuidUtil } from '../../../utils';

@Component({
  selector: 'paginator',
  templateUrl: 'paginator.component.html'
})

// Source: https://github.com/angular/components/blob/8b5c0f12c823a16bdf3d4bcd767d0c42c6b0e870/src/material/paginator/paginator.ts
export class PaginationComponent implements OnInit, AfterViewInit, OnDestroy {

  public pageIndexGuid: string;
  public pageSizeOptionsGuid: string;

  private _initialized: boolean;
  private _showComponent: boolean;

  /** The zero-based page index of the displayed list of items. Defaulted to 0. */
  @Input()
  get pageIndex(): number { return this._pageIndex; }
  set pageIndex(value: number) {
    this._pageIndex = Math.max(value, 0);
  }
  private _pageIndex = 0;

  /** The length of the total number of items that are being paginated. Defaulted to 0. */
  @Input()
  get length(): number { return this._length; }
  set length(value: number) {
    this._length = value;
  }
  private _length = 0;

  /** Number of items to display on a page. By default set to 50. */
  @Input()
  get pageSize(): number { return this._pageSize; }
  set pageSize(value: number) {
    const previousPageSize = this._pageSize;
    this._pageSize = Math.max(value, 0);
    this._updateDisplayedPageSizeOptions();
    if (this._pageSize != previousPageSize) {
      this.pageIndex = 0;
      this._emitPageEvent(0);
    }
  }
  private _pageSize: number;

  /** The set of provided page size options to display to the user. */
  @Input()
  get pageSizeOptions(): number[] { return this._pageSizeOptions; }
  set pageSizeOptions(value: number[]) {
    this._pageSizeOptions = value;
    this._updateDisplayedPageSizeOptions();
  }
  private _pageSizeOptions: number[] = [];

  /** Event emitted when the paginator changes the page size or page index. */
  @Output() page = new EventEmitter<PageEvent>();

  /** Displayed set of page size options. Will be sorted and include current page size. */
  _displayedPageSizeOptions: number[];

  constructor(private guidUtil: GuidUtil) {
    this.pageIndexGuid = this.guidUtil.createGuid();
    this.pageSizeOptionsGuid = this.guidUtil.createGuid();
  }

  ngOnInit() {
    this._initialized = true;
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this._updateDisplayedPageSizeOptions();
    });
  }

  ngOnDestroy() {
  }

  /** Advances to the next page if it exists. */
  nextPage(): void {
    if (!this.hasNextPage()) { return; }

    const previousPageIndex = this.pageIndex;
    this.pageIndex++;
    this._emitPageEvent(previousPageIndex);
  }

  /** Move back to the previous page if it exists. */
  previousPage(): void {
    if (!this.hasPreviousPage()) { return; }

    const previousPageIndex = this.pageIndex;
    this.pageIndex--;
    this._emitPageEvent(previousPageIndex);
  }

  /** Move to the first page if not already there. */
  firstPage(): void {
    // hasPreviousPage being false implies at the start
    if (!this.hasPreviousPage()) { return; }

    const previousPageIndex = this.pageIndex;
    this.pageIndex = 0;
    this._emitPageEvent(previousPageIndex);
  }

  /** Move to the last page if not already there. */
  lastPage(): void {
    // hasNextPage being false implies at the end
    if (!this.hasNextPage()) { return; }

    const previousPageIndex = this.pageIndex;
    this.pageIndex = this.getNumberOfPages() - 1;
    this._emitPageEvent(previousPageIndex);
  }

  /** Whether there is a previous page. */
  hasPreviousPage(): boolean {
    return this.pageIndex >= 1 && this.pageSize != 0;
  }

  /** Whether there is a next page. */
  hasNextPage(): boolean {
    const maxPageIndex = this.getNumberOfPages() - 1;
    return this.pageIndex < maxPageIndex && this.pageSize != 0;
  }

  /** Calculate the number of pages */
  getNumberOfPages(): number {
    if (!this.pageSize) {
      return 0;
    }

    return Math.ceil(this.length / this.pageSize);
  }

  /** Calculate the maximum value */
  getMax(): number {
    return this.hasNextPage() ? (this.pageIndex + 1) * this.pageSize : this.length;
  }

  /** Calculate the minimum value */
  getMin(): number {
    return this.length > 0 ? (this.pageIndex + 1) * this.pageSize - this.pageSize + 1 : 0;
  }

  /** Whether the component should be shown. */
  showComponent(): boolean {
    var showComponent = Math.ceil(this.length / PageEventNamespace.DEFAULT_PAGE_SIZE) > 1;
    if (this._showComponent != showComponent && showComponent) {
      this._initMaterialize();
    }
    this._showComponent = showComponent;
    return this._showComponent;
  }

  private _initMaterialize() {
    setTimeout(() => {
      var options = {};
      var instance = M.FormSelect.init($('#' + this.pageSizeOptionsGuid), options);
    });
  }

  /**
   * Updates the list of page size options to display to the user. Includes making sure that
   * the page size is an option and that the list is sorted.
   */
  private _updateDisplayedPageSizeOptions() {
    if (!this._initialized) { return; }

    // If no page size options are provided, use the default page size options.
    if (!this.pageSizeOptions || this.pageSizeOptions.length == 0) {
      this.pageSizeOptions = PageEventNamespace.DEFAULT_PAGE_SIZE_OPTIONS.slice();
    }

    // If no page size is provided, use the first page size option or the default page size.
    if (!this.pageSize) {
      this._pageSize = this.pageSizeOptions.length != 0 ?
        this.pageSizeOptions[0] :
        PageEventNamespace.DEFAULT_PAGE_SIZE;
    }

    this._displayedPageSizeOptions = this.pageSizeOptions.slice();

    if (this._displayedPageSizeOptions.indexOf(this.pageSize) === -1) {
      this._displayedPageSizeOptions.push(this.pageSize);
    }

    // Sort the numbers using a number-specific sort function.
    this._displayedPageSizeOptions.sort((a, b) => a - b);

    this._initMaterialize();
  }

  /** Emits an event notifying that a change of the paginator's properties has been triggered. */
  private _emitPageEvent(previousPageIndex: number) {
    this.page.emit({
      previousPageIndex,
      pageIndex: this.pageIndex,
      pageSize: this.pageSize,
      length: this.length
    });
  }
}
