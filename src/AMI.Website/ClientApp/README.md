# AMI.Website

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 7.3.9.

## Docker build/run

In a command prompt, make sure you are in the root directory containing the solution file AMI.sln

* docker build --tag=ami-website:v0.0.0 -f AMI.Website\Dockerfile .
* docker run --name ami-website -e "ClientOptions:ApiEndpoint=http://localhost:23000" -p 23600:80 ami-website:v0.0.0

Now the Website should be accessible on http://localhost:23600/ in your browser.

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).
