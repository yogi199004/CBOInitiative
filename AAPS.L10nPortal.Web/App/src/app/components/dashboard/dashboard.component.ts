import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import * as _ from 'lodash';
import * as $ from 'jquery';
import { DashboardService } from '../../services/dashboard.service';
import { FileUploadService } from '../../services/file-upload.service';
import { LocaleService } from '../../services/locale.service';
import { UserService } from '../../services/user.service';
import { ToastrService } from 'ngx-toastr';
import { ILocale } from '../../entities/locale.interface';
import { IAppInfo } from '../../entities/app-info.interface';
import { IApplicationLocale } from '../../entities/application-locale.interface';
import { ApplicationInsightsService } from '../../services/application-insights.service';
import { error } from '@angular/compiler/src/util';
import { FilterDataService } from '../../services/filter-data.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})

export class DashboardComponent implements OnInit {
  locales: IApplicationLocale[];
  isSortingDesc: boolean = true;
  currentSortColumn: string = ''
  applications: IAppInfo[];
  isUserAppManager: boolean = false;
  availablesLocales: ILocale[] = [];
  newLocale = <IApplicationLocale>{};
  editLocale = <IApplicationLocale>{};
  deletedLocale = <IApplicationLocale>{};
  highlightingError: boolean = false;
  superAdmin: boolean = false;
  errorMessage: string;
  newApp = <IApplicationLocale>{};
  addAppManager = <IApplicationLocale>{};
  highlightErrorAppModal: boolean = false;
  highlightingenUSError: boolean = false;
  filterdropDownData: any = {}
  filterApplied: boolean = false;

  private enUsLocaleCode = this.dashboardService.enUsLocaleCodeConst;


  constructor(
    private router: Router,
    private dashboardService: DashboardService,
    private fileUploadService: FileUploadService,
    private localeService: LocaleService,
    private userService: UserService,
    private toasterService: ToastrService,
    private applicationInsightsService: ApplicationInsightsService,
    private filterDataService: FilterDataService
  ) { }

  ngOnInit(): void {
    this.init();
  }

  private init(forceGetUser: boolean = false): void {
    //this.getCurrentUserAsync(forceGetUser)
    //  .then(() => {
    //    this.populateDashboardData()
    //  });
    this.populateDashboardData();
    //debugger;
    //this.getAvailablesLocales();

  }
  populateDashboardData() {
    this.filterApplied = this.filterDataService.getFilterStatus()
    if (this.filterApplied) {
      this.reApplyFilter();
    }
    else {
      this.getLocalesAsync()
    }
  }


  private getLocalesAsync(): void {
    debugger;
    this.dashboardService
      .getLocalesAsync()
      .then(data => {
        this.locales = data;
        this.checkForSortAfterFilter()// check if sorting was applied earlier

        this.setDashBoardWithLocaleData();

        this.getFilterDataAsync(this.locales)
      });
  }

  private setDashBoardWithLocaleData(): void {
    this.locales.forEach(locale => {
      locale.IsSuperAppManager = this.superAdmin;

      if (locale.AppManagerCount >= 2) {

        locale.isAppManagerButtonDisabled = true;
      }
    })

    if (!this.isUserAppManager || this.locales.length === 0) {
      return;
    }
    this.locales.forEach(locale => {
      this.initLocale(locale);
    });
  }

  private getAvailablesLocales(): void {
    this.localeService
      .getAvailablesLocalesAsync()
      .then(locales => this.availablesLocales = locales);
  }

  private initLocale(locale: IApplicationLocale): IApplicationLocale {
    var app = this.applications.filter(app => app.Id === locale.ApplicationId);

    locale.IsAppManager = app.length > 0;

    return locale;
  }

  uploadLocale(event: Event, locale: IApplicationLocale): void {
    var inputElement = <HTMLInputElement>event.target;
    var promise;

    const files: FileList = inputElement.files;

    if (files.length === 0) {
      return;
    }

    const formData = new FormData();

    formData.append('uploadFile', files[0], files[0].name);

    if (locale.IsAppManager && locale.LocaleCode === this.enUsLocaleCode) {
      promise = this.dashboardService.importLocaleDefaultKeysFromExcelAsync(locale.ApplicationLocaleId, formData);
    } else {
      promise = this.dashboardService.importLocaleFromExcelAsync(locale.ApplicationLocaleId, formData);
    }

    promise.then(data => {
      locale.UpdatedDate = data.UpdatedDate;
      this.populateDashboardData();
      this.toasterService.success('File uploaded successfully', 'Success');
      this.fileUploadService.updateFileInput(event);
    }, error => {
      this.fileUploadService.updateFileInput(event);
    });
  }

  downloadLocale(locale: IApplicationLocale): void {
    if (locale.IsAppManager && locale.LocaleCode === this.enUsLocaleCode) {
      window.location.href = this.dashboardService.getLocaleDefaultKeysDownloadUrl(locale.ApplicationLocaleId);
    } else {
      window.location.href = this.dashboardService.getLocaleDownloadUrl(locale.ApplicationLocaleId);
    }
  }


  navigateToLocaleAssets(localeId: number): void {
    this.router.navigate(['app-assets', localeId]);
  }
  navigateToAssetsLink(localeId: number): void {
    this.router.navigate(['asset-links', localeId]);
  }


  getFilterDataAsync(locales): void {
    this.filterDataService.getDashboardFilterDataAsync(locales)
      .then((filterData: any) => {
        this.filterdropDownData = filterData;
      })
  }

  sortFilterFromChild(event): void {
    if (event.type == 'sort') {
      this.doSorting(event.columnName, event.direction)
    }
    else {
      this.addFilter(event, false);
    }
  }
  doSorting(property: any, direction): void {
    this.currentSortColumn = property;
    this.isSortingDesc = direction == 'down' ? true : false;
    if (direction == 'down') {
      this.locales = _.orderBy(this.locales, [(locale) => {
        let value = _.get(locale, property);
        return value != null ? value.toLowerCase() : ''
      }], 'desc');
    }
    else {
      this.locales = _.orderBy(this.locales, [(locale) => {
        let value = _.get(locale, property);
        return value != null ? value.toLowerCase() : ''
      }], 'asc');

    }
  }
  checkForSortAfterFilter(): void {
    if (this.currentSortColumn) {//alreadysorted view
      const direction = this.isSortingDesc == true ? 'down' : 'up';
      this.doSorting(this.currentSortColumn, direction)
    }
  }

  addFilter(e, bool) {
    e['called_from'] = 'dashboard'
    this.filterDataService.applyFilterDataAsync(e, bool)// if reappling filter use bool
      .then((data) => {
        this.locales = data;
        this.setDashBoardWithLocaleData();
        this.filterDataService.setFilterStatus(true);
        this.filterApplied = true;
        this.checkForSortAfterFilter();
      }, err => {
        
      })
  }

  reApplyFilter() {
    this.filterDataService.getDashBoardFilterAppliedData().then((data: any) => {
      let { master, filterData } = data
      this.filterdropDownData = master;
      this.addFilter(filterData, true);// reappling filter so using true 
      this.filterDataService.reApplyFilterOnDashBoard.next(filterData)
    })
  }
  clearFilter() {
    //send clear filter signal to child elements
    //clear all variables representing filterapplied
    //load  fresh dashboard data
    this.filterDataService.clearFilter.next(true);
    this.filterDataService.setFilterStatus(false);
    this.filterApplied = false;
    this.getLocalesAsync();
  }

  browseFile = (browseControlId: string, index: number): void => {
    document.getElementById(browseControlId + index).click();
  }

  openCreateLocaleModal(): void {
    this.newLocale = <IApplicationLocale>{};

    $("#addLocaleModal").modal("show");
  }

  openEditLocaleModal(locale: IApplicationLocale): void {
    this.editLocale = _.cloneDeep(locale);

    $("#editLocaleModal").modal("show");
  }

  openDeleteLocaleModal(locale: IApplicationLocale): void {
    this.deletedLocale = _.cloneDeep(locale);

    $("#deleteLocaleModal").modal("show");
  }

  saveNewLocale(): void {
    if (!this.validateLocale(this.newLocale)) {
      this.highlightingError = true;

      return;
    }

    this.dashboardService
      .createLocaleAsync(this.newLocale)
      .then(response => {
        var locale = this.initLocale(response);

        //this.locales.push(locale);
        this.populateDashboardData();
        $("#addLocaleModal").modal("toggle");

        if (this.highlightingError) {
          this.highlightingError = false;
        }
      });
  }

  saveEditLocale(): void {
    if (!this.validateLocale(this.editLocale)) {
      this.highlightingError = true;

      return;
    }

    this.dashboardService
      .editLocaleAsync(this.editLocale)
      .then(response => {
        var locale = this.locales.filter(locale => locale.ApplicationLocaleId === this.editLocale.ApplicationLocaleId && locale.UserId === this.editLocale.UserId)[0];

        locale.CanEdit = response.CanEdit;
        locale.PreferredName = response.PreferredName;
        locale.TotalAssetsCount = response.TotalAssetsCount;
        locale.UpdatedDate = response.UpdatedDate;
        locale.UserEmail = response.UserEmail;
        locale.UserId = response.UserId;
        //Refreshing the page

        this.populateDashboardData();

        $("#editLocaleModal").modal("toggle");

        if (this.editLocale.LocaleCode === this.enUsLocaleCode) {
          this.init(true);
        }

        if (this.highlightingError) {
          this.highlightingError = false;
        }

        this.editLocale = <IApplicationLocale>{};
      });
  }

  private validateLocale(locale: IApplicationLocale): boolean {
    return locale.ApplicationId != null && locale.LocaleId != null && locale.UserEmail != null && locale.UserEmail !== "";
  }

  closeModal(): void {
    if (this.highlightingError) {
      this.highlightingError = false;
    }
  }

  deleteLocale(): void {
    this.dashboardService
      .deleteLocaleAsync(this.deletedLocale)
      .then(() => {
        var locales = this.locales.filter(l => l.ApplicationLocaleId === this.deletedLocale.ApplicationLocaleId);

        locales.forEach(l => {
          var index = this.locales.indexOf(l);

          if (index >= 0) {
            this.locales.splice(index, 1);
          }
        });

        $("#deleteLocaleModal").modal("toggle");

        this.deletedLocale = <IApplicationLocale>{};
      });
  }

  openAppOnboardModal(): void {
    this.newApp = <IApplicationLocale>{};

    $("#addAppModal").modal("show");
  }

  saveNewApp(): void {
    if (!this.validateAppOnboard(this.newApp)) {
      this.highlightErrorAppModal = true;

      return;
    }
    if ((this.newApp.ApplicationName as string).indexOf(' ') >= 0) {
      this.toasterService.error('Application Name Can not contain space', 'Error');
      return;
    }


    this.dashboardService
      .addAppAsync(this.newApp)
      .then(user => {
        this.toasterService.success('Application Onboarded Successfully');

        this.populateDashboardData()
        $("#addAppModal").modal("toggle");

        if (this.highlightErrorAppModal) {
          this.highlightErrorAppModal = false;
        }
      }), error => {
        this.toasterService.error('Application Onboarding Failed');
      };
  }

  private validateAppOnboard(app: IApplicationLocale): boolean {
    return app.ApplicationName != null && app.ApplicationName != "" && app.UserEmail != null && app.UserEmail !== "";
  }

  openAddAppManagerModal(locale: IApplicationLocale): void {
    this.addAppManager = _.cloneDeep(locale);
    this.addAppManager.UserEmail = "";

    $("#addAppManagerModal").modal("show");
  }

  saveAddAppManager(): void {
    if (!this.validateAddAppManager(this.addAppManager)) {
      this.highlightingenUSError = true;

      return;
    }

    this.dashboardService
      .addAppManagerAsync(this.addAppManager)
      .then(response => {

        this.toasterService.success('Application Manager Added Successfully');
        //Refreshing the page
        this.populateDashboardData();

        $("#addAppManagerModal").modal("toggle");

        if (this.addAppManager.LocaleCode === this.enUsLocaleCode) {
          this.init(true);
        }

        if (this.highlightingenUSError) {
          this.highlightingenUSError = false;
        }

        this.addAppManager = <IApplicationLocale>{};
      }), error => {
        this.toasterService.error('Application Manager Addition Failed');
      };
  }

  private validateAddAppManager(locale: IApplicationLocale): boolean {
    return locale.ApplicationId != null && locale.LocaleId != null && locale.UserEmail != null && locale.UserEmail !== "";
  }

}
