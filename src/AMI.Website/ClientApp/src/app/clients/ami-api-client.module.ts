import { NgModule } from '@angular/core';
import {
  ApiOptionsAmiApiClient,
  AppInfoAmiApiClient,
  AppLogsAmiApiClient,
  AppOptionsAmiApiClient,
  AuditEventsAmiApiClient,
  ObjectsAmiApiClient,
  TasksAmiApiClient,
  TokensAmiApiClient,
  WorkersAmiApiClient
} from './ami-api-client';

@NgModule({
  declarations: [

  ],
  imports: [

  ],
  providers: [
    ApiOptionsAmiApiClient,
    AppInfoAmiApiClient,
    AppLogsAmiApiClient,
    AppOptionsAmiApiClient,
    AuditEventsAmiApiClient,
    ObjectsAmiApiClient,
    TasksAmiApiClient,
    TokensAmiApiClient,
    WorkersAmiApiClient
  ]
})
export class AmiApiClientModule { }
