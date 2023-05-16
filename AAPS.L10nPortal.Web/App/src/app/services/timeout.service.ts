import { Injectable } from '@angular/core';

@Injectable()
export class TimeoutService {
  sessionValidTo: Date = null;

  updateSessionValidTo(): void {
    const sessionTime: number = 1800000;

    var actionTime = new Date();

    var sessionDate = new Date(actionTime.getTime() + sessionTime);

    if (sessionDate < this.sessionValidTo) {
      return;
    }

    this.sessionValidTo = sessionDate;
  };
}
