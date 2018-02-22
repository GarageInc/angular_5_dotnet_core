import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../../common/services/authentication.service';
import { OnChanges, SimpleChanges } from '@angular/core';
import { EventService } from '../../../../common/services/event-service/event.service';
import { HeaderEnums } from '../../enums/header.enum';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  constructor(
    private authenticationService: AuthenticationService,
    private eventService: EventService
  ) {}

  ngOnInit() {
    if (this.authorized) {
      this.loadCounter();
    }

    this.eventService.subscribe(HeaderEnums.reloadCounter, () => {
      this.loadCounter();
    });
  }

  private loadCounter(): void {}

  public get userName(): string {
    return this.authenticationService.getUserName();
  }

  public get authorized(): boolean {
    return this.authenticationService.isAuthorized();
  }
}
