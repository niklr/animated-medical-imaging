import { Injectable } from '@angular/core';
import { ErrorModel, IErrorModel } from '../clients/ami-api-client';

import M from 'materialize-css';

@Injectable()
export class NotificationService {

  constructor() {

  }

  public raiseMessage(message: string): void {
    M.toast({ html: message, classes: 'rounded' });
  }

  public raiseError(message: string, error: any): void {
    this.raiseMessage(message);
  }

  public handleError(error: any): string {
    const defaultMessage = 'An unexpected error occurred.';
    let message: string;

    if (error instanceof Error) {
      message = (<Error>error).message;
    } else if (error instanceof ErrorModel) {
      message = (<ErrorModel>error).error;
    } else if (error instanceof Response) {
      message = error.status ? `${error.status} - ${error.statusText}` : 'Server error';
      try {
        const errorResult = <IErrorModel>error.json();
        if (errorResult.error) {
          message = errorResult.error;
        }
      } catch (error) { }
    }

    if (!!message) {
      this.raiseError(message, error);
      return message;
    } else {
      this.raiseError(defaultMessage, error);
      return defaultMessage;
    }
  }
}
