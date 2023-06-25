import { Injectable } from '@angular/core';
import { BaseRepository } from './base.repository';
import { IRequest } from '../entities/request.interface';
import { IApplicationLocale } from '../entities/application-locale.interface';

@Injectable()
export class DashboardRepository extends BaseRepository {
  getApplicationLocalesAsync(): Promise<IApplicationLocale[]> {
    return this.get<IApplicationLocale[]>('/api/UserApplicationLocale');
  }

  getFilteredApplicationLocalesAsync(formData): Promise<IApplicationLocale[]> {

    var request: IRequest = {
      Url: "api/Filter/GetDashboardFilterData",
      Body: formData
    };

    return this.post<IApplicationLocale[]>(request);
  }

  importLocaleFromExcelAsync(applicationLocaleId: number, formData: FormData): Promise<IApplicationLocale> {
    var request: IRequest = {
      Url: "/api/UserApplicationLocale/ImportFromExcel/" + applicationLocaleId,
      Body: formData
    };

    return this.post<IApplicationLocale>(request);
  }

  importLocaleDefaultKeysFromExcelAsync(applicationLocaleId: number, formData: FormData): Promise<IApplicationLocale> {
    var request: IRequest = {
      Url: "/api/UserApplicationLocale/ImportFromExcel/" + applicationLocaleId,
      Body: formData
    };

    return this.post<IApplicationLocale>(request);
  }

  createLocaleAsync(locale: IApplicationLocale) {
    var request: IRequest = {
      Url: "api/UserApplicationLocale/Create",
      Body: {
        ApplicationId: locale.ApplicationId,
        LocaleId: locale.LocaleId,
        Email: locale.UserEmail
      }
    }

    return this.post<IApplicationLocale>(request);
  }

  editLocaleAsync(locale: IApplicationLocale) {
    debugger;
    var request: IRequest = {
      Url: "api/UserApplicationLocale/" + locale.ApplicationLocaleId + "/Reassign",
      Body: {
        AssignFromUserId: locale.UserId,
        AssignToUserEmail: locale.UserEmail
      }
    }

    return this.post<IApplicationLocale>(request);
  }

  deleteLocaleAsync(locale: IApplicationLocale) {
    return this.delete<IApplicationLocale>("api/UserApplicationLocale/" + locale.ApplicationLocaleId + "/Delete");
  }

  addAppAsync(app: IApplicationLocale) {
    var request: IRequest = {
      Url: "api/UserApplicationLocale/OnboardApp",
      Body: {
        ApplicationName: app.ApplicationName,
        Email: app.UserEmail
      }
    }
    return this.post<number>(request);
  }


  addAppManagerAsync(locale: IApplicationLocale) {
    var request: IRequest = {
      Url: "api/UserApplicationLocale/" + locale.ApplicationLocaleId + "/AddAppManager",
      Body: {
        AssignFromUserId: locale.UserId,
        AssignToUserEmail: locale.UserEmail
      }
    }

    return this.post<number>(request);
  }

}


