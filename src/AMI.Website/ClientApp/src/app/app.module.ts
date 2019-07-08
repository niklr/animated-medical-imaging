import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AmiApiClientModule } from './clients/ami-api-client.module';
import { API_BASE_URL } from './clients/ami-api-client';
import { AppComponent } from './app.component';
import { TimeoutInterceptor, DEFAULT_TIMEOUT } from './interceptors';
import { GatewayHub } from './hubs';
import { AppProxy } from './proxies/app.proxy';
import { ConfigService } from './services/config.service';
import { ConsoleLoggerService } from './services/console-logger.service';
import { LoggerService } from './services/logger.service';
import { NotificationService } from './services/notification.service';
import { PubSubService } from './services/pubsub.service';
import { BackgroundWorker, GarbageCollector } from './utils';

export function initConfig(configService: ConfigService) {
  return () => configService.load();
}

export function initBaseAmiApi() {
  return ConfigService.options.apiEndpoint;
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
    GatewayHub,
    AppProxy,
    ConfigService,
    NotificationService,
    PubSubService,
    BackgroundWorker,
    GarbageCollector,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TimeoutInterceptor,
      multi: true
    },
    {
      provide: DEFAULT_TIMEOUT,
      useValue: 10000
    },
    {
      provide: APP_INITIALIZER,
      useFactory: initConfig,
      deps: [ConfigService], multi: true
    },
    {
      provide: API_BASE_URL,
      useFactory: initBaseAmiApi,
      deps: [ConfigService], multi: true
    },
    {
      provide: LoggerService,
      useClass: ConsoleLoggerService
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
