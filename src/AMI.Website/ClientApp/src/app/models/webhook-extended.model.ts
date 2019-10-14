import { EnumContainer } from './enum-container.model';
import { WebhookModel } from '../../app/clients/ami-api-client';
import { EventType } from '../enums';
import { IKeyedCollection, KeyedCollection } from '../extensions';

export class WebhookModelExtended extends WebhookModel {
  public isChecked: boolean;
  public secret: string;
  public isWildcardEventSelected: boolean;
  public eventsContainer: IKeyedCollection<EnumContainer> = new KeyedCollection<EnumContainer>();

  private _wildcardEvent: string = '*';

  constructor(webhook?: WebhookModel) {
    super(webhook);

    if (!this.enabledEvents) {
      this.enabledEvents = [];
    }

    Object.values(EventType).forEach(element => {
      this.eventsContainer.add(element.toString(), new EnumContainer(
        {
          displayName: element.toString(),
          enum: element,
          checked: false
        })
      );
    });

    this.enabledEvents.forEach(element => {
      if (this.eventsContainer.containsKey(element)) {
        this.eventsContainer.item(element).checked = true;
      }
    });

    if (this.enabledEvents.length <= 0) {
      this.enabledEvents.push(this._wildcardEvent);
    }

    if (this.enabledEvents.indexOf(this._wildcardEvent) !== -1) {
      this.isWildcardEventSelected = true;
    }
  }

  toggleEvent(container: EnumContainer): void {
    container.checked = !container.checked;
    this.enabledEvents = [];
    this.eventsContainer.values().forEach(element => {
      if (element.checked) {
        this.enabledEvents.push(element.enum);
      }
    });
  }

  toggleWildcardEvent(event: any): void {
    this.isWildcardEventSelected = !this.isWildcardEventSelected;
    if (this.isWildcardEventSelected) {
      this.eventsContainer.values().forEach(element => {
        element.checked = false;
      });
      this.enabledEvents = [this._wildcardEvent];
    }
  }
}
