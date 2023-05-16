import { Injectable } from '@angular/core';

@Injectable()
export class FileUploadService {
  constructor() { }

  updateFileInput(event): void {
    if (!this.checkBrowserIsSafari()) {
      var fileInputElement = <HTMLInputElement>document.getElementById(event.target.id);

      if (fileInputElement != null) {
        fileInputElement.type = "text";
        fileInputElement.type = "file";
      }
    }
  }

  private checkBrowserIsSafari(): boolean {
    var safariIdentifier = "safari";
    var chromeIdentifier = "chrome";
    var userAgent = navigator.userAgent.toLowerCase();

    return userAgent.indexOf(safariIdentifier) > -1 && userAgent.indexOf(chromeIdentifier) === -1;
  }
}
