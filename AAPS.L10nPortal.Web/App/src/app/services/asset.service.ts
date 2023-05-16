import { Injectable } from '@angular/core';
import { IAsset } from '../entities/asset.interface';
import { IApplicationLocaleAssets } from '../entities/locale-assets.interface';
import { AssetRepository } from '../repositories/asset.repository';

@Injectable()
export class AssetService {
  constructor(
    private assetRepository: AssetRepository
  ) { }

  getAssetListAsync(applicationLocaleId: number): Promise<IApplicationLocaleAssets> {
    return this.assetRepository.getAssetListAsync(applicationLocaleId);
  }

  importAssetsFromExcelAsync(applicationLocaleId: number, keyId: number, formData: FormData): Promise<IAsset> {
    return this.assetRepository.importAssetsFromExcelAsync(applicationLocaleId, keyId, formData);
  }

  getAssetDownloadUrl(applicationLocaleId, keyId: Number): string {
    return "/api/ApplicationLocaleAsset/Download/" + applicationLocaleId + "/" + keyId;
  }

  getAssetKeysWithAssetAsync(applicationLocaleId: number): Promise<IApplicationLocaleAssets> {
    return this.assetRepository.getAssetKeysWithAssetAsync(applicationLocaleId);
  }
}
