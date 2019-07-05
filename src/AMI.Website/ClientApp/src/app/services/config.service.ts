import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { IClientOptions } from '../models/client-options.model';

@Injectable()
export class ConfigService {

  static options: IClientOptions;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

  }

  public load() {
    return new Promise<void>((resolve, reject) => {
      this.http.get<IClientOptions>(this.baseUrl + 'options').subscribe(result => {
        ConfigService.options = result;
        ConfigService.options.isDevelopment = !environment.production;
        resolve();
      }, error => {
        reject(`Could not load options: ${JSON.stringify(error)}`);
      });
    });
  }
}
