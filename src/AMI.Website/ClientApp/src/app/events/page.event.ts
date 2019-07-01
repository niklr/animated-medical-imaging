export namespace PageEventNamespace {
  /** The default page size if there is no page size and there are no provided page size options. */
  export const DEFAULT_PAGE_SIZE = 10;

  /** The default page size options if there are no provided page size options. */
  export const DEFAULT_PAGE_SIZE_OPTIONS = [10, 25, 50];
}

/**
 * Change event object that is emitted when the user selects a
 * different page size or navigates to another page.
 */
export class PageEvent {
  /** The current page index. */
  pageIndex: number;
  /** Index of the page that was selected previously. */
  previousPageIndex: number;
  /** The current page size */
  pageSize: number;
  /** The current total number of items being paged */
  length: number;

  public constructor() {
    this.pageIndex = 0;
    this.previousPageIndex = 0;
    this.pageSize = PageEventNamespace.DEFAULT_PAGE_SIZE;
    this.length = 0;
  }
}
