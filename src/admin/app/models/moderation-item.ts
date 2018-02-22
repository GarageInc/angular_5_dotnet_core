import { SeoParameter } from '../../../search/app/models/api/seo-parameter';

export class ModerationItem {
  id: number;
  producerCode: string;
  producerCodeTrimmed: string;

  producerId: number;
  producerName: string;
  supplierName: string;
  supplierId: number;
  ruName: string;
  enName: string;
  isDeleted: boolean;
  status: number;
  seoParameterId: number;
  seo: SeoParameter;

  haveSupplier: boolean;
  catalogItemId: number;

  constructor() {
    this.setSeo();
  }

  public setSeo(): void {
    this.seo = new SeoParameter();
  }
}
