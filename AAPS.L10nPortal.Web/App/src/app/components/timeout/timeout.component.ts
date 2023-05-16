import { Component, OnInit } from '@angular/core';
import * as $ from 'jquery';
import { TimeoutService } from '../../services/timeout.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-timeout',
  templateUrl: './timeout.component.html',
  styleUrls: ['./timeout.component.scss']
})
export class TimeoutComponent implements OnInit {
  constructor(
    private timeoutService: TimeoutService,
    private userService: UserService
  ) { }

  time: number = null;

  private sessionRenewing: boolean = false;
  private timeoutInterval = null;

  ngOnInit(): void {
    setInterval(() => this.checkSession(), 10000);
  }

  private checkSession(): void {
    if (this.timeoutService.sessionValidTo == null) {
      return;
    }

    if (this.sessionRenewing) {
      return;
    }

    const timeToShowDialog: number = 3 * 60 * 1000;

    var timeToExpire = this.timeoutService.sessionValidTo.getTime() - Date.now();

    if (timeToExpire > timeToShowDialog) {
      return;
    }

    this.sessionRenewing = true;

    var time = this.timeoutService.sessionValidTo.getTime() - Date.now();

    if (time >= 0) {
      this.time = time;
    }

    this.timeoutInterval = setInterval(() => {
      time = this.timeoutService.sessionValidTo.getTime() - Date.now();

      if (time >= 0) {
        this.time = time;
      } else {
        this.logout();
      }
    }, 1000);

    $("#timeoutModal").modal({ backdrop: "static" });

    $("#timeoutModal").modal("show");
  }

  continueSession(): void {
    this.userService
      .renewSessionAsync()
      .then(() => {
        this.sessionRenewing = false;

        $("#timeoutModal").modal("hide");

        clearInterval(this.timeoutInterval);
      }).catch(() => {
        this.logout();
      });
  };

  logout(): void {
    window.location.href = "/Home/Logout";
  };

  ngOnDestroy(): void {
    if (this.timeoutInterval) {
      clearInterval(this.timeoutInterval);
    }
  }
}
