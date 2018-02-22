import { Counter } from './counter';

export class Statistic {
  counters: Counter;

  constructor() {
    this.counters = new Counter();
  }
}
