import { Token } from './token';

export class SeoConfig {
  public SiteName = new Token('{{Название сайта}}');
  public ProducerCode = new Token('{{Артикул}}');
  public RuName = new Token('{{Название на русском}}');
  public EnName = new Token('{{Название на английском}}');
  public Producer = new Token('{{Производитель}}');
  public Supplier = new Token('{{Поставщик}}');
}
