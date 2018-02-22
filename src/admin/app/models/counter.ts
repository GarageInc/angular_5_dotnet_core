import { PartsCounter } from './parts-counter';

export class Counter {
  users: number;
  suppliers: number;
  salePoints: number;
  parts: PartsCounter;
  producers: number;

  constructor() {
    this.parts = new PartsCounter();
  }
}
