import { IAsset } from "./asset.interface";

export interface IApplicationLocaleAssets {
  ApplicationName: string;
  LocaleCode: string;
  Assets: IAsset[];
}
