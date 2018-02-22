import { Injectable } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import { Title } from '@angular/platform-browser';
import { ProducerItemSearchModel } from '../../models/api/producer-item-search-model';
import { SupplierModel } from '../../models/api/supplier-model';
import { SeoService } from '../seo-service/seo.service';
import { SeoParameter } from '../../models/api/seo-parameter';
import { SearchSeoParams } from './../../../../common/models/seo/search-seo-params';
import { SeoConfig } from './../../../../common/models/seo/seo-config';
import { debounce } from 'rxjs/operators/debounce';
import { Observable } from 'rxjs/Observable';
import { BaseApiService } from '../../../../common/services/base-api.service';
import { HttpClient } from '@angular/common/http';
import { Headers, RequestOptions, Response, Http } from '@angular/http';
import { SearchTerm } from '../../models/dto/search-term';
import { SeoModel } from '../../../../common/models/seo/seo-model';

@Injectable()
export class SeoAdapterService extends BaseApiService {
  public SITE_NAME = 'Печкин';

  public searchSeoSettings: SearchSeoParams;
  public seoConfig: SeoConfig = new SeoConfig();

  constructor(
    private seoService: SeoService,
    protected httpClient: HttpClient,
    protected http: Http
  ) {
    super();
  }

  public get(): Subscription {
    return this.httpClient
      .get<SearchSeoParams>('/files/ceo/search.json')
      .subscribe(data => {
        this.searchSeoSettings = data;
      });
  }

  public checkStatisticForPart(model: SearchTerm): void {
    this.httpClient.post<boolean>('/api/v1/statistic/check', model).subscribe();
  }

  public setPartSeo(
    producerName: string,
    producerCode: string,
    item: ProducerItemSearchModel | null
  ): void {
    this.add(() => {
      this.setTitle(
        this.getPartParam(
          producerName,
          producerCode,
          item,
          this.searchSeoSettings.parts.title
        )
      );
      this.setDescription(
        this.getPartParam(
          producerName,
          producerCode,
          item,
          this.searchSeoSettings.parts.description
        )
      );
    });
  }

  private add(f: Function): void {
    if (!this.searchSeoSettings) {
      this.get().add(f);
    } else {
      f();
    }
  }

  public setAboutPageSeo(): void {
    this.add(() => {
      this.setTitle(this.getAboutPageParam(this.searchSeoSettings.about.title));
      this.setDescription(
        this.getAboutPageParam(this.searchSeoSettings.about.description)
      );
    });
  }

  public setSupplierSeo(supplier: SupplierModel): void {
    this.add(() => {
      this.setTitle(
        this.getSupplierParam(supplier, this.searchSeoSettings.suppliers.title)
      );
      this.setDescription(
        this.getSupplierParam(
          supplier,
          this.searchSeoSettings.suppliers.description
        )
      );
    });
  }

  public setClarifySeo(producerCode: string): void {
    this.add(() => {
      this.setTitle(
        this.getClarifyParam(producerCode, this.searchSeoSettings.clarify.title)
      );
      this.setDescription(
        this.getClarifyParam(
          producerCode,
          this.searchSeoSettings.clarify.description
        )
      );
    });
  }

  public setMainSeo(): void {
    this.add(() => {
      this.setTitle(this.getMainParam(this.searchSeoSettings.main.title));
      this.setDescription(
        this.getMainParam(this.searchSeoSettings.main.description)
      );
    });
  }

  public setSeoParameter(param: SeoParameter): void {
    this.add(() => {
      this.setTitle(param.name);
      this.seoService.setMetaDescription(param.description);
    });
  }

  public setTitle(title: string): void {
    this.seoService.setTitle(title);
  }

  public setDescription(description: string): void {
    this.seoService.setMetaDescription(description);
  }

  /*
  PRIVATE
  */

  private getAboutPageParam(param: string): string {
    return this.replace(param, this.seoConfig.SiteName.Name, this.SITE_NAME);
  }

  private getMainParam(param: string): string {
    return this.replace(param, this.seoConfig.SiteName.Name, this.SITE_NAME);
  }

  private getSupplierParam(supplier: SupplierModel, param: string): string {
    let value = param;
    value = this.replace(value, this.seoConfig.SiteName.Name, this.SITE_NAME);
    value = this.replace(value, this.seoConfig.Supplier.Name, supplier.name);
    return value;
  }

  private getClarifyParam(producerCode: string, param: string): string {
    return this.replace(param, this.seoConfig.ProducerCode.Name, producerCode);
  }

  private getPartParam(
    producerName: string,
    producerCode: string,
    item: ProducerItemSearchModel | null,
    param: string
  ): string {
    let value = param;
    value = this.replace(value, this.seoConfig.SiteName.Name, this.SITE_NAME);
    value = this.replace(value, this.seoConfig.Producer.Name, producerName);
    if (item) {
      value = this.replace(
        value,
        this.seoConfig.EnName.Name,
        item.enName || ''
      );
      value = this.replace(
        value,
        this.seoConfig.RuName.Name,
        item.ruName || ''
      );
    }
    value = this.replace(value, this.seoConfig.ProducerCode.Name, producerCode);
    return value;
  }

  private replace(from: string, oldValue: string, newValue): string {
    return from ? from.replace(oldValue, newValue) : from;
  }
}
