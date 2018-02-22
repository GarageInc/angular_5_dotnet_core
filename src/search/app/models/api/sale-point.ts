import { Contact } from './contact';
import { TimeWork } from './time-work';

export class SalePoint extends Contact {
  deliveryMethod: string;
  name: string;
  timeWorks: TimeWork[];
}
