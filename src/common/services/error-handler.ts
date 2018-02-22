import { NgModule, ErrorHandler } from '@angular/core';

export class MyErrorHandler implements ErrorHandler {
  handleError(error: Error) {
    const message = error && error.message ? error.message : error.toString();

    console.log('------>');

    console.log(message);
    console.log(error.stack);

    console.log('<------');
    console.log(error);
    // do something with the exception
  }
}
