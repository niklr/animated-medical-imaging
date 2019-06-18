import { Directive, HostListener, Input, Renderer2, ElementRef } from '@angular/core';

@Directive({
  selector: '[appClickOnce]'
})
export class ClickOnceDirective {

  private isDisabled: boolean;
  private initialText: string;
  @Input() clickFn: (callbackFn: Function) => void;

  constructor(private renderer: Renderer2, private elementRef: ElementRef) {
    this.isDisabled = false;
  }

  addClass(className: string) {
    this.renderer.addClass(this.elementRef.nativeElement, className);
  }

  removeClass(className: string) {
    this.renderer.removeClass(this.elementRef.nativeElement, className);
  }

  disableButton() {
    this.isDisabled = true;
    this.addClass('disabled');
    this.initialText = this.elementRef.nativeElement.innerHTML;
    let loadingIcon = '<i class="fas fa-circle-notch fa-spin mr-2"></i>';
    this.elementRef.nativeElement.innerHTML = loadingIcon + this.initialText;
  }

  releaseButton = () => {
    this.isDisabled = false;
    this.removeClass('disabled');
    this.elementRef.nativeElement.innerHTML = this.initialText;
  }

  @HostListener('click', ['$event'])
  clickEvent(event) {
    event.preventDefault();
    event.stopPropagation();
    if (!this.isDisabled) {
      this.disableButton();
      this.clickFn(this.releaseButton);
    }
  }
}
