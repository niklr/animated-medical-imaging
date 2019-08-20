import { NgModule } from '@angular/core';
import {
  ApiOptionsAmiApiClient,
  AppInfoAmiApiClient,
  AppLogsAmiApiClient,
  AppOptionsAmiApiClient,
  AuditEventsAmiApiClient,
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
    AppInfoAmiApiClient,
    AppLogsAmiApiClient,
    AppOptionsAmiApiClient,
    AuditEventsAmiApiClient,
    ObjectsAmiApiClient,
    TasksAmiApiClient,
    TokensAmiApiClient
  ]
})
export class AmiApiClientModule { }
