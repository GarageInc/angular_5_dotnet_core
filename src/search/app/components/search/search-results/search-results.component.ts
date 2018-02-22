import { Component, OnInit, Input } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ProducerItemSearchModel } from '../../../models/api/producer-item-search-model';
import { SupplierItemSearchModel } from '../../../models/api/supplier-item-search-model';
import { SearchTerm } from '../../../models/dto/search-term';
import { OnChanges, SimpleChanges, OnDestroy } from '@angular/core';
import { debounce } from 'rxjs/operators/debounce';
import { SearchBackendService } from '../../../services/backend-service/search-backend.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import {
  debounceTime,
  distinctUntilChanged,
  switchMap,
  merge
} from 'rxjs/operators';
import 'rxjs/add/observable/merge';
import 'rxjs/add/observable/forkJoin';
import { Title } from '@angular/platform-browser';
import { SeoAdapterService } from '../../../services/seo-adapter/seo-adapter.service';
import { NavigatorService } from '../../../services/navigator-service/navigator.service';
import { SeoBackendService } from '../../../services/seo-service/seo-backend.service';
import { SeoParameter } from '../../../models/api/seo-parameter';
import { EventService } from './../../../../../common/services/event-service/event.service';
import { SearchEvents } from '../../../enums/search-events';
import { ProducerCodeService } from '../../../services/producer-code/producer-code.service';
import { SearchDataService } from '../../../services/search-data-service/search-data.service';
import { SearchTripAndTrick } from '../../../models/search-trip-and-trick';
import { SearchTripAndTricksService } from '../../../services/search-data-service/search-trip-and-tricks.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.scss']
})
export class SearchResultsComponent implements OnInit, OnChanges, OnDestroy {
  @Input() producerItemsInput: ProducerItemSearchModel[];
  @Input() supplierItemsInput: SupplierItemSearchModel[];

  originalProducerItems: ProducerItemSearchModel[] = [];
  possibleProducerItems: ProducerItemSearchModel[] = [];

  @Input() producerCode = '';

  private producerName: string;
  private subscriptions: Array<Subscription> = new Array<Subscription>();

  private searchTerm = new Subject();
  public isDuringSearch = false;

  constructor(
    private backend: SearchBackendService,
    private seoBackend: SeoBackendService,
    private router: Router,
    private seoAdapter: SeoAdapterService,
    private activateRoute: ActivatedRoute,
    private navigator: NavigatorService,
    private events: EventService,
    private producerCodeService: ProducerCodeService,
    private dataService: SearchDataService
  ) {}

  ngOnInit() {
    this.subscriptions.push(
      this.activateRoute.params.subscribe(params => {
        this.producerName = params['producer'];
        this.producerCode = params['art'];

        if (this.producerName && this.producerCode) {
          this.isDuringSearch = true;
          this.events.broadcast(
            SearchEvents.SetSearchString,
            this.producerCode
          );

          if (this.shouldReload()) {
            this.loadDetails(
              new SearchTerm(this.producerName, this.producerCode)
            );
          } else {
            this.recalculate();
          }
        }
      })
    );
    this.subscriptions.push(
      this.activateRoute.queryParams.subscribe(params => {
        if (params['art']) {
          this.isDuringSearch = true;
          this.producerCode = params['art'];
        }
      })
    );
  }

  public get producerItems(): ProducerItemSearchModel[] {
    return this.dataService.producerItems;
  }

  public set producerItems(value: ProducerItemSearchModel[]) {
    this.dataService.producerItems = value;
  }

  public get supplierItems(): SupplierItemSearchModel[] {
    return this.dataService.supplierItems;
  }

  public set supplierItems(value: SupplierItemSearchModel[]) {
    this.dataService.supplierItems = value;
  }

  public shouldReload(): boolean {
    return (
      this.producerItems &&
      this.supplierItems &&
      (this.producerItems.length >= 0 && !this.haveActualSupplierItems())
    );
  }

  public haveActualSupplierItems(): boolean {
    this.calculateOriginals();
    const catalogItem = this.originalProducerItems[0];

    if (catalogItem) {
      const any = this.supplierItems.some(x => {
        return +x.producer.producerId === +catalogItem.producerId;
      });

      return any;
    }
    return false;
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.producerItemsInput && changes.producerItemsInput.currentValue) {
      this.producerItems = <any>changes.producerItemsInput.currentValue;
    }
    if (changes.supplierItemsInput && changes.supplierItemsInput.currentValue) {
      this.supplierItems = <any>changes.supplierItemsInput.currentValue;
    }

    if (!changes.producerCode && !changes.supplierItemsInput) {
      this.recalculate();
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(x => (x ? x.unsubscribe() : ''));
  }

  public loadDetails(term: SearchTerm) {
    Observable.forkJoin([
      this.backend
        .searchInProducers(term.producerCode, term.producerName)
        .map(result => {
          this.producerItems = result;
          return result;
        }),

      this.backend
        .searchInSuppliers(term.producerCode, term.producerName)
        .map(result => {
          this.supplierItems = result;
          return result;
        })
    ]).subscribe(() => {
      this.recalculate();
      // this.setTitle();
    });
  }

  public setTitle(): void {
    if (this.originalProducerItems.length === 1) {
      const item = this.originalProducerItems[0];

      if (item.seoParameterId) {
        this.seoBackend
          .getSeo(item.seoParameterId)
          .subscribe((model: SeoParameter) => {
            this.seoAdapter.setSeoParameter(model);
          });
      } else {
        this.setPartTitle(item);
      }
    } else {
      this.setPartTitle(null);
    }

    if (this.producerCode && this.producerName) {
      this.seoAdapter.checkStatisticForPart(
        new SearchTerm(this.producerName, this.producerCode)
      );
    }
  }

  public setPartTitle(item: ProducerItemSearchModel | null): void {
    if (item) {
      this.seoAdapter.setPartSeo(item.producer.name, this.producerCode, item);
    } else if (this.producerName) {
      this.seoAdapter.setPartSeo(this.producerName, this.producerCode, null);
    }
  }

  public calculateOriginals(): void {
    this.originalProducerItems = this.producerItems.filter(item =>
      this.isMatch(item)
    );
  }

  public recalculate(): void {
    if (this.producerItems) {
      this.possibleProducerItems = this.producerItems.filter(
        item => !this.isMatch(item)
      );

      this.calculateOriginals();

      if (this.originalProducerItems.length > 1) {
        this.possibleProducerItems = this.possibleProducerItems.concat(
          this.originalProducerItems
        );
      }
    }

    if (this.supplierItems) {
      /*for (let i = 0; i < this.originalProducerItems.length; i++) {
        const item = this.originalProducerItems[i] || {};

        item.isExistInSuppliers = this.supplierItems.some(supplierItem => {
          return (
            // tslint:disable-next-line:triple-equals
            +supplierItem.producer.producerId == +item.producerId &&
            this.producerCodeService.compare(
              supplierItem.producerCode,
              item.producerCode
            )
          );
        });
      }*/
    }

    this.checkPossibilityToShowPart();

    this.isDuringSearch = false;
  }

  public checkPossibilityToShowPart(): boolean {
    const item = this.canShowTableWithoutClarify();
    if (item) {
      this.navigator
        .goToPart(item.producer.name, this.producerCode)
        .then(() => {
          this.setTitle();
        });
      return true;
    }

    return false;
  }

  public isMatch(item: ProducerItemSearchModel): boolean {
    return (
      this.producerCodeService.compare(
        item.producerCodeTrimmed || item.producerCode,
        this.producerCode
      ) &&
      (!this.producerName ||
        item.producer.name.toLowerCase() === this.producerName)
    );
  }

  // services

  public canShowTableWithoutClarify(): ProducerItemSearchModel | undefined {
    const checkingArray =
      this.originalProducerItems.length > 0
        ? this.originalProducerItems
        : this.possibleProducerItems;

    if (checkingArray.length === 1) {
      const item = checkingArray[0];
      return this.isMatch(item) ? item : undefined;
    }

    return undefined;
  }

  public canShowClarify(): boolean {
    return (
      this.originalProducerItems.length !== 1 &&
      this.possibleProducerItems.length > 0 &&
      !this.canShowTableWithoutClarify()
    );
  }

  public canShowTable(): boolean {
    return (
      this.originalProducerItems.length <= 1 &&
      (!!this.canShowTableWithoutClarify() ||
        (this.supplierItems && this.supplierItems.length > 0))
    );
  }

  public canShowWarning(): boolean {
    return (
      !this.isDuringSearch &&
      !!this.supplierItems &&
      +this.supplierItems.length === 0
    );
  }
}
