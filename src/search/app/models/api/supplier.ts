import { Address } from './address';
import { Price } from './price';

export class Supplier {
  searchName: string;
  logoName: string;
  deliveryMethods: string[];
  phoneNumbers: string;
  addresses: Address[];
  prices: Price;
  count: number;
}
