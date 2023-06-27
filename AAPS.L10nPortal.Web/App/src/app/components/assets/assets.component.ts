import { Component, OnInit } from '@angular/core';
import { AssetService } from '../../services/asset.service';
import { FileUploadService } from '../../services/file-upload.service';
import * as _ from 'lodash';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IAsset } from '../../entities/asset.interface';
import { IApplicationLocaleAssets } from '../../entities/locale-assets.interface';
import { FilterDataService } from '../../services/filter-data.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-assets',
  templateUrl: './assets.component.html',
  styleUrls: ['./assets.component.scss']
})

export class AssetsComponent implements OnInit {
  assets: IAsset[];
  isSortingDesc: boolean = false;
  applicationLocaleId: number;
  applicationName: string;
  localeCode: string;
  pageLoadComplete: boolean = false;
  currentSortColumn: any;
  filterdropDownData: any = {};
  filterApplied: boolean = false;
  assetLinkFilter: any = false;
  constructor(
    private route: ActivatedRoute,
    private assetService: AssetService,
    private fileUploadService: FileUploadService,
    private toasterService: ToastrService,
    private filterDataService: FilterDataService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.applicationLocaleId = params['applicationLocaleId'];
      this.init();
    });
  }

  init(): void {
    this.getAssetsAsync()
      .then(data => {
        this.applicationName = data.ApplicationName;
        this.localeCode = data.LocaleCode;

        this.pageLoadComplete = true;
       
      });
  }

  getAssetsAsync(): Promise<IApplicationLocaleAssets> {
    return this.assetService
      .getAssetListAsync(this.applicationLocaleId)
      .then(data => {
        this.assets = data.Assets;
        return data;
      });
  }

  uploadAsset(event: Event, index: number): void {
    let currentAsset = this.assets[index];
    var inputElement = <HTMLInputElement>event.target;

    const files: FileList = inputElement.files;

    if (files.length === 0) {
      return;
    }

    const formData = new FormData();

    formData.append('uploadFile', files[0], files[0].name);

    this.assetService
      .importAssetsFromExcelAsync(this.applicationLocaleId, currentAsset.KeyId, formData)
      .then(data => {
        currentAsset.Value = data.Value;
        currentAsset.UpdatedDate = data.UpdatedDate;
        currentAsset.Changed = data.Changed;

        this.toasterService.success('File uploaded successfully', 'Success');

        this.fileUploadService.updateFileInput(event);
      }, error => {
        this.fileUploadService.updateFileInput(event);
      });
  }

  downloadAsset(keyId: number): void {
    window.location.href = this.assetService.getAssetDownloadUrl(this.applicationLocaleId, keyId);
  }

  sortFilterFromChild(event): void {
    if (event.type == 'sort') {
      this.doSorting(event.columnName, event.direction)
    }
    else {
      this.addFilter(event);
    }
  }
  doSorting(property: any, direction): void {
    this.currentSortColumn = property;
    this.isSortingDesc = direction == 'down' ? true : false;
    if (direction == 'down') {
      this.assets = _.orderBy(this.assets, [(asset) => {
        let value = _.get(asset, property);
        return value != null ? value.toLowerCase() : ''
      }], 'desc');
    }
    else {
      this.assets = _.orderBy(this.assets, [(asset) => {
        let value = _.get(asset, property);
        return value != null ? value.toLowerCase() : ''
      }], 'asc');

    }
  }

  getFilterDataAsync(assets) {
    return this.filterDataService.getAssetsFilterDataAsync(assets)
      .then((filterData: any) => {
        this.filterdropDownData = filterData;
        return this.filterdropDownData
      })
  }

  checkForSortAfterFilter(): void {
    if (this.currentSortColumn) {//alreadysorted view
      const direction = this.isSortingDesc == true ? 'down' : 'up';
      this.doSorting(this.currentSortColumn, direction)
    }
  }
  applyAssetLinkFilter() {
    this.assetService
      .getAssetKeysWithAssetAsync(this.applicationLocaleId)
      .then(data => {
        this.applicationName = data.ApplicationName;
        this.localeCode = data.LocaleCode;
        this.pageLoadComplete = true;
        this.assets = data.Assets;
        this.getFilterDataAsync(data.Assets);
        this.filterApplied = true;
        this.filterDataService.setassetFilterState(true);
      });
  }
  addFilter(e) {
    e['called_from'] = 'assetPage';
    e['ApplicationLocaleId'] = this.applicationLocaleId;
    this.filterDataService.applyFilterForAssets(e)// if reappling filter use bool
      .then((data: any) => {
        this.assets = [...data.Assets];
        this.filterApplied = true;
        this.filterDataService.setassetFilterState(true);
        this.checkForSortAfterFilter();
      }, err => {
        
      })
  }
  clearFilter() {
    this.filterDataService.clearFilter.next(true);
    this.filterDataService.setassetFilterState(false);
    this.filterApplied = false;
    this.init();
  }
  browseFile = (browseControlId: string, index: number): void => {
    document.getElementById(browseControlId + index).click();
  }
}
