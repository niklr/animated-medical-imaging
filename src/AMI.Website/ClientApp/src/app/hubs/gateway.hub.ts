import { Injectable } from '@angular/core';
import * as SignalR from '@aspnet/signalr';
import { BaseHub } from './base.hub';
import { ConnectionState } from '../enums';
import { IGatewayResult } from '../models/gateway-result.model';
import { ConfigService } from '../services/config.service';
import { LoggerService } from '../services/logger.service';
import { GarbageCollector, BackgroundWorker } from '../utils';

@Injectable()
export class GatewayHub extends BaseHub {

  private events = [];
  private token: string;
  private connection: SignalR.HubConnection;
  public connectionState: ConnectionState = ConnectionState.Disconnected;
  public disconnectedDate: Date;

  constructor(private gc: GarbageCollector, private worker: BackgroundWorker, private logger: LoggerService) {

    super();

    this.disconnectedDate = new Date();

    this.gc.attach(() => {
      this.stop();
      this.token = undefined;
      this.connection = undefined;
    });

    this.worker.attach(() => {
      this.restart();
    });
  }

  public on(event: string, callback: (data: any) => void): void {
    this.events.push(event, callback);
  }

  protected fire(...args: any[]): void {
    const event = args[0];
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
      const gatewayResult = result as IGatewayResult<any>;
      this.fire(gatewayResult.eventType, gatewayResult.data);
    });
    this.connection.start()
      .then(() => {
        this.logger.info('Hub connection started');
        this.changeConnectionState(ConnectionState.Connected);
      })
      .catch(err => {
        this.logger.info('Error while establishing connection');
        this.changeConnectionState(ConnectionState.Disconnected);
      });
    this.connection.onclose(e => {
      this.logger.info('Connection closed');
      this.changeConnectionState(ConnectionState.Disconnected);
    });
  }

  public stop(): void {
    if (this.connection) {
      this.changeConnectionState(ConnectionState.Disconnected);
      this.connection.stop();
      this.connection.onclose(e => { });
    }
  }

  public restart(): void {
    if (this.token && this.connectionState === ConnectionState.Disconnected) {
      this.changeConnectionState(ConnectionState.Connecting);
      this.start(this.token);
    }
  }

  public updateToken(token: string): void {
    this.token = token;
  }

  private changeConnectionState(newConnectionState: ConnectionState): void {
    if (this.connectionState !== newConnectionState) {
      if (this.connectionState === ConnectionState.Connected &&
        newConnectionState === ConnectionState.Disconnected) {
        this.disconnectedDate = new Date();
      }
      this.connectionState = newConnectionState;
    }
  }
}
