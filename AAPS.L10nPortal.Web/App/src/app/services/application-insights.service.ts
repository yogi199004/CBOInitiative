import { Injectable } from '@angular/core';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';

@Injectable()
export class ApplicationInsightsService {
    constructor() {
        const connectionString = window['ApplicationInsights'];

        if (connectionString === '') {
            return;
        }

        this.appInsights = new ApplicationInsights({
            config: {
                connectionString: connectionString,
                enableAutoRouteTracking: true,
                enableCorsCorrelation: true,
                namePrefix: this.mainChannelName
            }
        });

        this.appInsights.loadAppInsights();

        const telemetryInitializer = (envelope) => {
            envelope.tags['ai.cloud.role'] = this.name;
            // Fix for bug #991239 App Insights - Unnecessary information is displayed in 'track' Request Payload
            Object.setPrototypeOf(envelope.tags, {});
        };

        this.appInsights.addTelemetryInitializer(telemetryInitializer);
    }

    private appInsights: ApplicationInsights;
    private name = 'L10nPortal Front-End';
    private mainChannelName = 'L10nPortal';

    SetUserContext(userId: string): void {
        if (this.appInsights == null) {
            return;
        }

        this.appInsights.setAuthenticatedUserContext(userId);
    }

    LogPageView(name?: string, url?: string): void {
        if (this.appInsights == null) {
            return;
        }

        this.appInsights.trackPageView({
            name: name,
            uri: url
        });
    }

    LogEvent(name: string, properties?: { [key: string]: any }): void {
        if (this.appInsights == null) {
            return;
        }

        this.appInsights.trackEvent({ name: name }, properties);
    }

    LogMetric(name: string, average: number, properties?: { [key: string]: any }): void {
        if (this.appInsights == null) {
            return;
        }

        this.appInsights.trackMetric({ name: name, average: average }, properties);
    }

    LogException(exception: Error, severityLevel?: number): void {
        if (this.appInsights == null) {
            return;
        }

        this.appInsights.trackException({
            exception: exception,
            severityLevel: severityLevel
        });
    }

    LogTrace(message: string, properties?: { [key: string]: any }): void {
        if (this.appInsights == null) {
            return;
        }

        this.appInsights.trackTrace({ message: message }, properties);
    }
}
