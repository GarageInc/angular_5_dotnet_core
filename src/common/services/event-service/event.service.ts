import { Injectable } from '@angular/core';
import { EventMessage } from './../../interfaces/event-message';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';

type MessageCallback = (payload: any) => void;

@Injectable()
export class EventService {
  public unsubscribeFromGroup = function(group: string | any): void {
    if (this.subscriptions[group]) {
      this.subscriptions[group].forEach((subscription: Subscription) => {
        if (subscription) {
          subscription.unsubscribe();
        }
        return subscription;
      });
    }
    this.subscriptions[group] = [];
  };
  // tslint:disable-next-line:member-ordering
  private handler = new Subject<EventMessage>();
  // tslint:disable-next-line:member-ordering
  private subscriptions: Array<Array<Subscription>> = [];

  public broadcast(type: any, payload?: any) {
    this.handler.next({ type, payload });
  }

  public subscribe(type: any, callback: MessageCallback): Subscription {
    const subscription: any = this.handler
      .filter(message => message.type === type)
      .map(message => message.payload)
      .subscribe(callback);

    if (!this.subscriptions[type]) {
      this.subscriptions[type] = [];
    }

    this.subscriptions[type].push(subscription);

    return subscription;
  }
}
