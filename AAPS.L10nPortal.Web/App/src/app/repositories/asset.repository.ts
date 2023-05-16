import { Injectable } from '@angular/core';
import { BaseRepository } from './base.repository';
import { IRequest } from '../entities/request.interface';
import { IAsset } from '../entities/asset.interface';
import { IApplicationLocaleAssets } from '../entities/locale-assets.interface';

@Injectable()
export class AssetRepository extends BaseRepository {
  getAssetListAsync(applicationLocaleId: number): Promise<IApplicationLocaleAssets> {
    return this.get<IApplicationLocaleAssets>(`/api/ApplicationLocaleAsset/${applicationLocaleId}`);
  }

  importAssetsFromExcelAsync(applicationLocaleId: number, keyId: number, formData: FormData): Promise<IAsset> {
    var request: IRequest = {
      Url: `/api/ApplicationLocaleAsset/Upload/${applicationLocaleId}/${keyId}`,
      Body: formData
    };

    return this.post<IAsset>(request);
  }
  getAssetFilterDataAsync(formData): Promise<IApplicationLocaleAssets[]> {
    var request: IRequest = {
      Url: "api/Filter/GetAssetFilterData",
      Body: formData
    };
    return this.post<IApplicationLocaleAssets[]>(request);
  }

  getAssetKeysWithAssetAsync(applicationLocaleId: number): Promise<IApplicationLocaleAssets> {
    return this.get<IApplicationLocaleAssets>(`/api/Filter/${applicationLocaleId}`);
  }
}
