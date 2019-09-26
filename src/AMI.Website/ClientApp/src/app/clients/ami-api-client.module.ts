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
  WebhooksAmiApiClient,
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
    WebhooksAmiApiClient,
    WorkersAmiApiClient
  ]
})
export class AmiApiClientModule { }
