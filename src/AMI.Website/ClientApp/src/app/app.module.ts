import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AmiApiClientModule } from './clients/ami-api-client.module';
import { API_BASE_URL } from './clients/ami-api-client';
import { AppComponent } from './app.component';
import { ConfigService } from './services/config.service';

export function initializeConfig(configService: ConfigService) {
  return () => configService.load();
}

export function initializeBaseAmiApi() {
  return ConfigService.settings.api.endpoint;
}

@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    AmiApiClientModule
  ],
  declarations: [
    AppComponent
  ],
  providers: [
    ConfigService,
    {
      provide: APP_INITIALIZER,
      useFactory: initializeConfig,
      deps: [ConfigService], multi: true
    },
    {
      provide: API_BASE_URL,
      useFactory: initializeBaseAmiApi,
      deps: [ConfigService], multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
