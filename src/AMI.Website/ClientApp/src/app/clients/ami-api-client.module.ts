import { NgModule } from '@angular/core';
import {
  AppInfoAmiApiClient,
  ObjectsAmiApiClient
} from './ami-api-client';

@NgModule({
  declarations: [

  ],
  imports: [

  ],
  providers: [
    AppInfoAmiApiClient,
    ObjectsAmiApiClient
  ]
})
export class AmiApiClientModule { }
