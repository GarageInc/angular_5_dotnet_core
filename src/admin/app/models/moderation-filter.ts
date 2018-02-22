import { BaseFilter } from './../../../common/models/base-filter';

export class ModerationFilter extends BaseFilter {
  producerCode: string;
  producerName: string;
  supplierName: string;
  ruName: string;
  enName: string;

  status: number;

  public constructor() {
    super();
    this.status = 1;
  }
}
