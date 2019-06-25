import { NgModule } from '@angular/core';
import {
  AppInfoAmiApiClient,
  ObjectsAmiApiClient,
  TasksAmiApiClient
} from './ami-api-client';

@NgModule({
  declarations: [

  ],
  imports: [

  ],
  providers: [
    AppInfoAmiApiClient,
    ObjectsAmiApiClient,
    TasksAmiApiClient
  ]
})
export class AmiApiClientModule { }
