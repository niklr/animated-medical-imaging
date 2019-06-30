export class CallbackWrapper {
  // The amount of running processes.
  public counter: number;
  // The current page index.
  private _isDisposed: boolean;
  // The wrapped callback function.
  private _callbackFn: Function;

  public constructor(callbackFn: Function) {
    this.counter = 0;
    this._isDisposed = false;
    this._callbackFn = callbackFn;
  }

  // Invokes the callback function not more than once.
  public invokeCallbackFn(): void {
    if (!this._isDisposed) {
      // Decrease counter by 1 every time invokeCallbackFn is called until it is 0.
      this.counter--;
      if (this.counter <= 0) {
        this._isDisposed = true;
        if (!!this._callbackFn && typeof this._callbackFn === 'function') {
          this._callbackFn();
        }
      }
    }
    // console.log('Disposed: ' + this._isDisposed + ' Counter: ' + this.counter);
  }
}
