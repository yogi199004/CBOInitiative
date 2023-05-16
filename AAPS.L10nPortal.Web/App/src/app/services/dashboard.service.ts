import { Injectable } from '@angular/core';
import { IApplicationLocale } from '../entities/application-locale.interface';
import { DashboardRepository } from '../repositories/dashboard.repository';
import { ITranslatedValueExportRow } from '../entities/translatedvalue-export.interface';
import { IDownloadFileResult } from '../entities/downloadfileresult';

@Injectable()
export class DashboardService {
  constructor(
    private dashboardRepository: DashboardRepository
  ) { }

  public enUsLocaleCodeConst = "en-US";

  getLocalesAsync(): Promise<IApplicationLocale[]> {
    return this.dashboardRepository.getApplicationLocalesAsync();
  }

  importLocaleFromExcelAsync(localeId: number, formData: FormData): Promise<IApplicationLocale> {
    return this.dashboardRepository.importLocaleFromExcelAsync(localeId, formData);
  }

  getLocaleDownloadUrl(localeId): string {
    return '/api/UserApplicationLocale/ExportToExcel/' + localeId;
  }

  importLocaleDefaultKeysFromExcelAsync(localeId: number, formData: FormData): Promise<IApplicationLocale> {
    return this.dashboardRepository.importLocaleDefaultKeysFromExcelAsync(localeId, formData);
  }

  createLocaleAsync(locale: IApplicationLocale): Promise<IApplicationLocale> {
    return this.dashboardRepository.createLocaleAsync(locale);
  }

  editLocaleAsync(locale: IApplicationLocale): Promise<IApplicationLocale> {
    return this.dashboardRepository.editLocaleAsync(locale);
  }

  deleteLocaleAsync(locale: IApplicationLocale): Promise<IApplicationLocale> {
    return this.dashboardRepository.deleteLocaleAsync(locale);
  }

  getLocaleDefaultKeysDownloadUrl(localeId): string {
    return '/api/UserApplicationLocale/ExportToExcel/' + localeId;
  }
  addAppAsync(app: IApplicationLocale): Promise<number> {
    return this.dashboardRepository.addAppAsync(app);
  }
  addAppManagerAsync(locale: IApplicationLocale): Promise<number> {
    return this.dashboardRepository.addAppManagerAsync(locale);
  }

}
