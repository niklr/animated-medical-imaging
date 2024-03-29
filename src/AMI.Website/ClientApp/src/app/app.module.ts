import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AmiApiClientModule } from './clients/ami-api-client.module';
import { API_BASE_URL } from './clients/ami-api-client';
import { AppComponent } from './app.component';
import { TimeoutInterceptor, DEFAULT_TIMEOUT, TokenInterceptor } from './interceptors';
import { GatewayHub } from './hubs';
import { AccountProxy } from './proxies/account.proxy';
import { AdminProxy } from './proxies/admin.proxy';
import { AppProxy } from './proxies/app.proxy';
import { ObjectProxy } from './proxies/object.proxy';
import { TaskProxy } from './proxies/task.proxy';
import { TokenProxy } from './proxies/token.proxy';
import { WebhookProxy } from './proxies/webhook.proxy';
import { AuthService } from './services/auth.service';
import { ConfigService } from './services/config.service';
import { ConsoleLoggerService } from './services/console-logger.service';
import { LoggerService } from './services/logger.service';
import { NotificationService } from './services/notification.service';
import { ObjectService } from './services/object.service';
import { PubSubService } from './services/pubsub.service';
import { TokenService } from './services/token.service';
import { ObjectStore, TokenStore } from './stores';
import { BackgroundWorker, GarbageCollector, MomentUtil } from './utils';

/*
 * Bootstrap:
 * 1. Retrieve options from HomeController and initialize ConfigService.
 * 2. Provide API_BASE_URL to auto-generated OpenAPI/Swagger client.
 */

export function initConfig(configService: ConfigService) {
  return () => configService.init(0);
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
    AccountProxy,
    AdminProxy,
    AppProxy,
    ObjectProxy,
    TaskProxy,
    TokenProxy,
    WebhookProxy,
    AuthService,
    ConfigService,
    NotificationService,
    ObjectService,
    PubSubService,
    TokenService,
    ObjectStore,
    TokenStore,
    BackgroundWorker,
    GarbageCollector,
    MomentUtil,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TimeoutInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      deps: [TokenStore, MomentUtil],
      multi: true
    },
    {
      provide: DEFAULT_TIMEOUT,
      useValue: 10000
    },
    {
      provide: APP_INITIALIZER,
      useFactory: initConfig,
      deps: [ConfigService],
      multi: true
    },
    {
      provide: API_BASE_URL,
      useFactory: initBaseAmiApi,
      deps: [ConfigService],
      multi: true
    },
    {
      provide: LoggerService,
      useClass: ConsoleLoggerService
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
