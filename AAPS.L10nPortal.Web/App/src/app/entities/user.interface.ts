import { IAppInfo } from "./app-info.interface";

export interface IUser {
  ApplicationManager: IAppInfo[];
  GlobalPersonUid: string;
  MemberFirmCode: string;
  CountryCode: string;
  PreferredFullName: string;
  Email: string;
}
