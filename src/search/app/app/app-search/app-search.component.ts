import { Component, OnInit } from '@angular/core';
import { SearchResult } from './../../models/search-result';
import { SearchBackendService } from '../../services/backend-service/search-backend.service';
import { Observable } from 'rxjs/Observable';
import { ProducerItemSearchModel } from '../../models/api/producer-item-search-model';
import { SupplierItemSearchModel } from '../../models/api/supplier-item-search-model';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { debounce } from 'rxjs/operators/debounce';
import { AsyncPipe } from '@angular/common/src/pipes/async_pipe';
import { ActivatedRoute } from '@angular/router';
import { EventService } from './../../../../common/services/event-service/event.service';
import { Subscription } from 'rxjs/Subscription';
import { Title } from '@angular/platform-browser';
import { SeoAdapterService } from '../../services/seo-adapter/seo-adapter.service';
import { SearchDataService } from '../../services/search-data-service/search-data.service';
import { SearchEvents } from '../../enums/search-events';
import { SearchTripAndTricksService } from '../../services/search-data-service/search-trip-and-tricks.service';
import { SearchTripAndTrick } from '../../models/search-trip-and-trick';

@Component({
  selector: 'app-search',
  templateUrl: './app-search.component.html',
  styleUrls: ['./app-search.component.scss']
})
export class AppSearchComponent implements OnInit {
  constructor(
    public backend: SearchBackendService,
    private activateRoute: ActivatedRoute,
    private seoAdapter: SeoAdapterService,
    public eventsService: EventService,
    public tripAndTricks: SearchTripAndTricksService
  ) {}

  public searchString: string;

  public searchProducerResults$: Observable<ProducerItemSearchModel[]>;
  public searchSupplierResults$: Observable<SupplierItemSearchModel[]>;

  private searchProducerTerms = new BehaviorSubject<string>(this.searchString);
  private searchSupplierTerms = new BehaviorSubject<string>(this.searchString);

  private subscription: Subscription;
  private COUNT_ON_PAGE = 10;

  ngOnInit() {
    this.subscription = this.activateRoute.queryParams.subscribe(params => {
      this.searchString = params['art'];

      if (this.searchString) {
        this.search();
      }
    });

    this.searchProducerResults$ = this.searchProducerTerms.pipe(
      // wait 300ms after each keystroke before considering the term
      debounceTime(0),

      // ignore new term if same as previous term
      distinctUntilChanged(),

      // switch to new search observable each time the term changes
      switchMap((term: string) => this.backend.searchInProducers(term))
    );

    this.searchSupplierResults$ = this.searchSupplierTerms.pipe(
      // wait 300ms after each keystroke before considering the term
      debounceTime(0),

      // ignore new term if same as previous term
      distinctUntilChanged(),

      // switch to new search observable each time the term changes
      switchMap((term: string) => this.backend.searchInSuppliers(term))
    );
  }

  public search(): void {
    if (this.isValidSearchString()) {
      this.tripAndTricks.add(this.searchString);
      this.searchProducerTerms.next(this.searchString);
      this.searchSupplierTerms.next(this.searchString);
    }
  }

  public isValidSearchString(): boolean {
    return !!this.searchString && this.searchString.length >= 4;
  }
}
