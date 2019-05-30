import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { IAppConfig } from '../models/app-config.model';

@Injectable()
export class ConfigService {

  static settings: IAppConfig;

  constructor(private http: HttpClient) {

  }

  public load() {
    let jsonFilePath: string;
    if (environment.production) {
      jsonFilePath = 'assets/config.production.json';
    } else {
      jsonFilePath = 'assets/config.development.json';
    }
    return new Promise<void>((resolve, reject) => {
      this.http.get(jsonFilePath).subscribe((response: IAppConfig) => {
        ConfigService.settings = response;
        ConfigService.settings.isDevelopment = !environment.production;
        resolve();
      }, error => {
        reject(`Could not load file '${jsonFilePath}': ${JSON.stringify(error)}`);
      });
    });
  }
}
