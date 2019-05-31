import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { IClientSettings } from '../models/client-settings.model';

@Injectable()
export class ConfigService {

  static settings: IClientSettings;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

  }

  public load() {
    return new Promise<void>((resolve, reject) => {
      this.http.get<IClientSettings>(this.baseUrl + 'settings').subscribe(result => {
        ConfigService.settings = result;
        ConfigService.settings.isDevelopment = !environment.production;
        resolve();
      }, error => {
        reject(`Could not load settings: ${JSON.stringify(error)}`);
      });
    });
  }
}
