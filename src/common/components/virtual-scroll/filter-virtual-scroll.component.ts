import { Component, OnInit } from '@angular/core';
import { ChangeEvent } from 'angular2-virtual-scroll';
import { MatDialog } from '@angular/material';
import { FilterListBackendService } from './../../services/filter-list-backend.service';
import { BaseFilter } from './../../models/base-filter';

export abstract class FilterVirtualScrollComponent<F extends BaseFilter, I>
  implements OnInit {
  constructor(public moderation: FilterListBackendService<F, I>) {}

  public filter: F = <F>{};

  public buffer: I[] = [];
  public scrollItems: Array<I>;
  public loading: boolean;
  protected countOnPage = 10;

  protected isEnd = false;

  ngOnInit() {
    this.load();
  }

  public load(): void {
    this.isEnd = false;
    this.fetchNextChunk(0, 30).then(data => {
      this.buffer = data;
    });
  }

  public fetchMore(event: ChangeEvent) {
    // tslint:disable-next-line:curly
    const end = event.end || 0;
    if (+end !== this.buffer.length || this.isEnd) {
      return;
    }
    this.loading = true;
    this.fetchNextChunk(this.buffer.length, this.countOnPage).then(chunk => {
      this.isEnd = chunk.length === 0;
      this.buffer = this.buffer.concat(chunk);
      this.loading = false;
    }, () => (this.loading = false));
  }

  protected fetchNextChunk(skip: number, limit: number): Promise<I[]> {
    this.filter.offset = skip;
    this.filter.count = limit;
    return this.moderation.next(this.filter).toPromise();
  }
}
