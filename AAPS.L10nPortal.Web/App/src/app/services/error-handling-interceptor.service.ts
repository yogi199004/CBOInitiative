import { Injectable } from "@angular/core";
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpResponse,
  HttpEvent,
  HttpErrorResponse,
} from "@angular/common/http";
import { Observable } from "rxjs";
import { tap } from "rxjs/operators";
import { ToastrService } from "ngx-toastr";

@Injectable()
export class ErrorHandlingInterceptorService implements HttpInterceptor {
  constructor(private toastrService: ToastrService) {}

  private httpRunningRequests = 0;
  private httpRequestActive: boolean;

  updateSpinnerState(running) {
    document.getElementById("spinner-container").style.display = running
      ? "block"
      : "none";
  }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    var requestVerificationToken: string = (<HTMLInputElement>(
      document.getElementById("__RequestVerificationToken")
    )).value;
    request = request.clone({
      setHeaders: {
        "X-Requested-With": "XMLHttpRequest",
        "RequestVerificationToken": requestVerificationToken,
      },
    });

    if (!request.params.get("async")) {
      this.httpRunningRequests++;
    }

    this.httpRequestActive = this.httpRunningRequests !== 0;
    this.updateSpinnerState(this.httpRequestActive);

    return next.handle(request).pipe(
      tap(
        (response: HttpEvent<any>) => {
          if (response instanceof HttpResponse) {
            if (!request.params.get("async")) {
              this.httpRunningRequests--;
            }

            this.httpRequestActive = this.httpRunningRequests !== 0;
            this.updateSpinnerState(this.httpRequestActive);

            if (request.method !== "GET") {
              var sessionValidTo = response.headers.get("SessionValidTo");

              if (sessionValidTo != undefined) {
                // TODO:
              }
            }
          }
        },
        (response: any) => {
          if (response instanceof HttpErrorResponse) {
            if (!request.params.get("async")) {
              this.httpRunningRequests--;
            }

            this.httpRequestActive = this.httpRunningRequests !== 0;
            this.updateSpinnerState(this.httpRequestActive);

            if (request.method !== "GET") {
              var sessionValidTo = response.headers.get("SessionValidTo");

              if (sessionValidTo != undefined) {
                // TODO:
              }
            }

            if (!response) {
              this.toastrService.error("Unknown http error occurred.", "Error");

              return;
            }

            var errorCode: number;

            if (response.error?.ExceptionMessage) {
              this.toastrService.error(
                response.error.ExceptionMessage,
                "Error: " + response.status
              );
              return;
            }
            
            if (
              response.error &&
              response.error.error &&
              response.error.error.message
            ) {
             
              errorCode = response.error.error.code || response.status;
              this.toastrService.error(
                response.error.error.message,
                "Error: " + errorCode
              );

              return;
            }

            if (response.status && response.status !== 0) {
              var message: string;
              switch (response.status) {
                case 400:
                  message = response.error.message;
                  errorCode = 400;
                  break;
                case 401:
                  message = "Your session has expired. Please log-in again.";
                  errorCode = 401;
                  // Redirect to login after 1 second to allow error popup to appear
                  setTimeout(() => (window.location.href = "/"), 1000);
                  break;
                case 403:
                  message = "You are not allowed to access requested resource.";
                  errorCode = 403;
                  break;
                case 404:
                  message = "The requested resource is not found.";
                  errorCode = 404;
                  break;
                case 408:
                case 504:
                  message = "The request timed out. Please try again.";
                  errorCode = 504;
                  break;
                default:
                  errorCode = -1;
                  message =
                    "There was an error processing your request. If the problem persists, please contact Support.";
                  errorCode = -1;
                  break;
              }

              this.toastrService.error(message, "Error: " + errorCode);
            }
          }
        }
      )
    );
  }
}
