import { Injectable } from '@angular/core';
import { PubSubTopic } from '../enums/pub-sub-topic.enum';
import PubSub from 'pubsub-js';

@Injectable()
export class PubSubService {

  constructor() {

  }

  public subscribe(topic: PubSubTopic, subscriber: any): string {
    return PubSub.subscribe(topic, subscriber);
  }

  public unsubscribe(token: string): void {
    PubSub.unsubscribe(token);
  }

  public publish(topic: PubSubTopic, data: any): void {
    PubSub.publish(topic, data);
  }
}
