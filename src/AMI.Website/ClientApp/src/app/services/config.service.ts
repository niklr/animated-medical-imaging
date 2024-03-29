import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { IApiOptions, IAppInfoModel } from '../clients/ami-api-client';
import { IClientOptions } from '../models/client-options.model';

@Injectable()
export class ConfigService {

  static isInitialized = false;
  static options: IClientOptions;
  static apiOptions: IApiOptions;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.baseUrl = this.removeTrailingSlash(baseUrl);
  }

  public init(retryCount: number): Promise<void> {
    // Everything loaded before calling resolve can be accessed in a non-deferred manner.
    return new Promise<void>((resolve, reject) => {
      const handleError = (error: any) => {
        if (retryCount > 3) {
          const message = `Could not load options: ${JSON.stringify(error)}`;
          const loadingElement = document.querySelector('#loading');
          if (loadingElement) {
            loadingElement.innerHTML = message;
          }
          reject(message);
        } else {
          setTimeout(() => {
            this.init(++retryCount);
          }, 500);
        }
      };
      this.http.get<IClientOptions>(this.baseUrl + '/options').subscribe(result => {
        ConfigService.options = result;
        ConfigService.options.isDevelopment = !environment.production;
        ConfigService.options.enableConsoleOutput = !environment.production;
        ConfigService.options.apiEndpoint = this.removeTrailingSlash(ConfigService.options.apiEndpoint);
        this.http.get<IAppInfoModel>(this.baseUrl + '/app-info').subscribe(innerResult => {
          if (innerResult) {
            ConfigService.options.clientVersion = innerResult.appVersion;
          }
        }, error => {
          handleError(error);
        });
        this.http.get<IApiOptions>(ConfigService.options.apiEndpoint + '/api-options').subscribe(innerResult => {
          if (innerResult) {
            ConfigService.apiOptions = innerResult;
            ConfigService.isInitialized = true;
            resolve();
          }
        }, error => {
          handleError(error);
        });
      }, error => {
        handleError(error);
      });
    });
  }

  private removeTrailingSlash(url: string): string {
    return url.replace(/\/$/, '');
  }
}
