import { Producer } from './producer';

export class ProducerItemSearchModel {
  id: number;
  producerCode: string;
  producerCodeTrimmed: string;
  ruName: string;
  enName: string;
  producer: Producer;
  producerId: number;
  seoParameterId: any;
  recommendedPrice: number;
  isExistInSuppliers: boolean;
}
