import { BrowserModule } from "@angular/platform-browser";
import { CommonModule } from "@angular/common";
import { NgModule, CUSTOM_ELEMENTS_SCHEMA, ErrorHandler } from "@angular/core";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ToastrModule } from "ngx-toastr";
import { AppRoutingModule } from "./app-routing.module";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";

import { AppComponent } from "./app.component";
import { HeaderComponent } from "./components/header/header.component";
import { FooterComponent } from "./components/footer/footer.component";
import { DashboardComponent } from "./components/dashboard/dashboard.component";
import { AssetsComponent } from "./components/assets/assets.component";
import { TimeoutComponent } from "./components/timeout/timeout.component";
import { SortFilterComponent } from './components/sort-filter/sort-filter.component';
import { EmptyResultsComponent } from './empty-results/empty-results.component';

import { AssetRepository } from "./repositories/asset.repository";
import { DashboardRepository } from "./repositories/dashboard.repository";
import { LocaleRepository } from "./repositories/locale.repository";
import { UserRepository } from "./repositories/user.repository";

import { ApplicationInsightsService } from "./services/application-insights.service";
import { ErrorHandlingInterceptorService } from "./services/error-handling-interceptor.service";
import { AssetService } from "./services/asset.service";
import { DashboardService } from "./services/dashboard.service";
import { FileUploadService } from "./services/file-upload.service";
import { LocaleService } from "./services/locale.service";
import { TimeoutService } from "./services/timeout.service";
import { UserService } from "./services/user.service";
import { FilterDataService } from "./services/filter-data.service";
import { ErrorHandlerService } from "./services/error-handler.service";

import { MilisecondsToSecondsPipe } from "./pipes/miliseconds-to-seconds.pipe";
import { MilisecondsToMinutesPipe } from "./pipes/miliseconds-to-minutes.pipe";
import { FilterPipe } from './pipes/filter.pipe';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    DashboardComponent,
    AssetsComponent,
    TimeoutComponent,
    MilisecondsToSecondsPipe,
    MilisecondsToMinutesPipe,
    SortFilterComponent,
    EmptyResultsComponent,
    FilterPipe,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    CommonModule,
    ToastrModule.forRoot(),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorHandlingInterceptorService,
      multi: true,
    },
    {
      provide: ErrorHandler,
      useClass: ErrorHandlerService
    },

    //Repositories
    AssetRepository,
    DashboardRepository,
    LocaleRepository,
    UserRepository,

    //Services
    AssetService,
    DashboardService,
    FileUploadService,
    LocaleService,
    TimeoutService,
    UserService,
    ApplicationInsightsService,
    FilterDataService,

    // Pipes
    MilisecondsToSecondsPipe,
    MilisecondsToMinutesPipe,
    FilterPipe,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
