export interface IApplicationLocale {
  ApplicationLocaleId: number;
  ApplicationId: number;
  ApplicationName: string;
  LocaleId: number;
  LocaleCode: string;
  EnglishName: string;
  NativeName: string;
  UpdatedDate?: Date;
  TotalLabelsCount: number;
  TotalAssetsCount: number;
  UserId: string;
  UserEmail: string;
  PreferredName: string;
  CanEdit: boolean;
  IsAppManager: boolean;
  IsSuperAppManager: boolean;
  AppManagerCount: number;
  isAppManagerButtonDisabled: boolean;
}
