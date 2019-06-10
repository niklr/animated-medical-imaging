import { Injectable } from '@angular/core';

@Injectable()
export class ClipboardUtil {
  public copy(value: string): void {
    // Source: https://hackernoon.com/copying-text-to-clipboard-with-javascript-df4d4988697f
    const el = document.createElement('textarea');
    el.value = value;
    el.setAttribute('readonly', '');
    el.style.position = 'absolute';
    el.style.left = '-9999px';
    document.body.appendChild(el);
    el.select();
    document.execCommand('copy');
    document.body.removeChild(el);
  }
}
