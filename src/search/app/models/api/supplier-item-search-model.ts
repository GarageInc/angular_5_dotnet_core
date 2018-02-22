import { Producer } from './producer';
import { Supplier } from './supplier';

export class SupplierItemSearchModel {
  producer: Producer;
  supplier: Supplier;

  name: number;
  producerCode: string;
  isExistInSuppliers: boolean;

  updatedAt: number;
  createdAt: number;
  freshOfPrice: number;
}
