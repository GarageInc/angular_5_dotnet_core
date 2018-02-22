import { SeoParameter } from './seo-parameter';
import { Warehouse } from './warehouse';
import { ModelFile } from './model-file';
import { Contact } from './contact';
import { SalePoint } from './sale-point';

export class SupplierModel {
  name: string;
  searchName: string;
  email: string;
  logo: ModelFile;
  site: string;
  inn: string;

  seoParameterId: number;

  warehouses: Warehouse[];
  contacts: Contact[];

  salePoints: SalePoint[];
}
