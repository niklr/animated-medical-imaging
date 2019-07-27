import { NgModule } from '@angular/core';
import {
  ApiOptionsAmiApiClient,
  AppOptionsAmiApiClient,
  AppInfoAmiApiClient,
  ObjectsAmiApiClient,
  TasksAmiApiClient,
  TokensAmiApiClient
} from './ami-api-client';

@NgModule({
  declarations: [

  ],
  imports: [

  ],
  providers: [
    ApiOptionsAmiApiClient,
    AppOptionsAmiApiClient,
    AppInfoAmiApiClient,
    ObjectsAmiApiClient,
    TasksAmiApiClient,
    TokensAmiApiClient
  ]
})
export class AmiApiClientModule { }
