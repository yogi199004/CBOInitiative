import { Injectable, ErrorHandler } from '@angular/core';
import { ApplicationInsightsService } from './application-insights.service';

@Injectable()
export class ErrorHandlerService extends ErrorHandler {
    constructor(private applicationInsightsService: ApplicationInsightsService) {
        super();
    }

    handleError(error: Error) {
        this.applicationInsightsService.LogException(error);
        super.handleError(error);
    }
}
