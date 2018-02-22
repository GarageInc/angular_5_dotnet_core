import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from './../../../../common/services/authentication.service';
import { OnChanges, SimpleChanges } from '@angular/core';
import { ModerationService } from '../../services/moderation/moderation.service';
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
    private moderation: ModerationService,
    private eventService: EventService
  ) {}

  public countOnModeration = 0;

  ngOnInit() {
    if (this.authorized) {
      this.loadCounter();
    }

    this.eventService.subscribe(HeaderEnums.reloadCounter, () => {
      this.loadCounter();
    });
  }

  private loadCounter(): void {
    this.moderation
      .needModerationCount()
      .map(data => (this.countOnModeration = data))
      .subscribe();
  }

  public get userName(): string {
    return this.authenticationService.getUserName();
  }

  public get authorized(): boolean {
    return this.authenticationService.isAuthorized();
  }
}
