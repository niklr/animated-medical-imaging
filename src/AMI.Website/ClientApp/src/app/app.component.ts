import { Component, OnInit } from '@angular/core';
import { ObjectModel } from './clients/ami-api-client';
import { PubSubTopic } from './enums';
import { AppProxy } from './proxies/app.proxy';
import { ObjectService } from './services/object.service';
import { PubSubService } from './services/pubsub.service';
import { BackgroundWorker } from './utils';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(private appProxy: AppProxy, private worker: BackgroundWorker,
              private objectService: ObjectService, private pubSubService: PubSubService) {
    this.appProxy.init(0);
    this.worker.init();
  }

  ngOnInit(): void {
    document.addEventListener('github:niklr/angular-material-datatransfer.item-completed', (item: any) => {
      try {
        const result = ObjectModel.fromJS(JSON.parse(item.detail.message));
        if (result && result.id) {
          this.pubSubService.publish(PubSubTopic.OBJECTS_INIT_TOPIC, undefined);
          this.objectService.processObject(result.id, undefined);
        }
      } catch (e) { }
    });
  }
}
