import { Injectable } from '@angular/core';
import * as SignalR from '@aspnet/signalr';
import { BaseHub } from './base.hub';
import { GatewayOpCode, ConnectionState } from '../enums';
import { IGatewayResult } from '../models/gateway-result.model';
import { LoggerService } from '../services/logger.service';
import { ConfigService } from '../services/config.service';
import { GarbageCollector, BackgroundWorker } from '../utils';

@Injectable()
export class GatewayHub extends BaseHub {

  private events = [];
  private token: string;
  private connection: SignalR.HubConnection;
  public connectionState: ConnectionState = ConnectionState.Disconnected;

  constructor(private gc: GarbageCollector, private worker: BackgroundWorker, private logger: LoggerService) {
    super();
    this.gc.attach(function () {
      const that = this as GatewayHub;
      that.stop();
      that.token = undefined;
      that.connection = undefined;
    }.bind(this));

    this.worker.attach(function () {
      const that = this as GatewayHub;
      that.restart();
    }.bind(this));
  }

  public on(event: string, callback: Function): void {
    this.events.push(event.toLowerCase(), callback);
  }

  protected fire(...args: any[]): void {
    const event = args[0].toLowerCase();
    // Find event listeners, and support pseudo-event `catchAll`
    for (let i = 0; i <= this.events.length; i += 2) {
      if (this.events[i] === event) {
        this.events[i + 1].apply(this, args.slice(1));
      }
      if (this.events[i] === 'catchall') {
        this.events[i + 1].apply(null, args);
      }
    }
  }

  public start(token: string): void {
    this.stop();
    this.token = token;

    const connectionBuilder = new SignalR.HubConnectionBuilder()
      .withUrl(this.baseUrl + '/gateway?authtoken=' + this.token);

    if (ConfigService.options.enableConsoleOutput) {
      connectionBuilder.configureLogging(SignalR.LogLevel.Information);
    } else {
      connectionBuilder.configureLogging(SignalR.LogLevel.None);
    }

    this.connection = connectionBuilder.build();

    this.connection.on('notify', (result: any) => {
      const gatewayResult = <IGatewayResult<any>>result;
      switch (gatewayResult.op) {
        case GatewayOpCode.Dispatch:
          this.fire(gatewayResult.t, gatewayResult.d);
          break;
        default:
          break;
      }
    });
    this.connection.start()
      .then(() => {
        this.logger.info('Hub connection started');
        this.connectionState = ConnectionState.Connected;
      })
      .catch(err => {
        this.logger.info('Error while establishing connection');
        this.connectionState = ConnectionState.Disconnected;
      });
    this.connection.onclose(e => {
      this.logger.info('Connection closed');
      this.connectionState = ConnectionState.Disconnected;
    });
  }

  public stop(): void {
    if (this.connection) {
      this.connectionState = ConnectionState.Disconnected;
      this.connection.stop();
      this.connection.onclose(e => { });
    }
  }

  public restart(): void {
    if (this.connectionState === ConnectionState.Disconnected) {
      this.connectionState = ConnectionState.Connecting;
      this.start(this.token);
    }
  }

  public updateToken(token: string): void {
    this.token = token;
  }
}
