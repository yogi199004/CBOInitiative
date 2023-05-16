import { Injectable, LOCALE_ID, Inject } from '@angular/core';
import { IApplicationLocale } from '../entities/application-locale.interface';
import { DashboardRepository } from '../repositories/dashboard.repository';
import { formatDate } from '@angular/common';
import * as _ from 'lodash';
import { ReplaySubject, Subject } from 'rxjs';
import { AssetRepository } from '../repositories/asset.repository';
@Injectable({
  providedIn: 'root'
})
export class FilterDataService {

  constructor(private dashboardRepository: DashboardRepository,
    private assetRepository: AssetRepository,
    @Inject(LOCALE_ID) localID: string) {
    this.localID = localID;
  }
  private isFilterAppliedOnDashBoard: boolean = false;
  private dashboardFilterMasterData: any;
  private dashboardFilterData: any;
  public localID: string;
  clearFilter: Subject<boolean> = new Subject();
  reApplyFilterOnDashBoard: ReplaySubject<Object> = new ReplaySubject(1);
  assetLinksFilter: ReplaySubject<Object> = new ReplaySubject(1);

  assetFilterData: { Key: { data: string[]; name: string; }; UpdatedDate: { data: string[]; name: string; }; };
  filterAppliedOnAsset: boolean = false;
  getDashboardFilterDataAsync(locales) {
    let applicationFilter: string[] = []
    let nameFilter: string[] = []
    let localeCodeFilter: string[] = []
    let updatedDateFilter: string[] = []

    locales.forEach(locale => {
      applicationFilter.push(locale.ApplicationName)
      nameFilter.push(locale.PreferredName)
      localeCodeFilter.push(locale.LocaleCode + '/' + locale.EnglishName)
      if (locale.UpdatedDate) {
        updatedDateFilter.push(locale.UpdatedDate);
      }
    });

    applicationFilter = this.sortandUnique(applicationFilter)
    nameFilter = this.sortandUnique(nameFilter);
    localeCodeFilter = this.sortandUnique(localeCodeFilter)
    updatedDateFilter = this.sortandUnique(updatedDateFilter, true);
    updatedDateFilter.unshift('empty')
    this.dashboardFilterMasterData = {
      'ApplicationName': {
        name: 'ApplicationName',
        data: applicationFilter
      },
      'PreferredName': {
        name: 'PreferredName',
        data: nameFilter
      },
      'LocaleCode': {
        name: 'LocaleCode',
        data: localeCodeFilter
      },
      'UpdatedDate': {
        name: 'UpdatedDate',
        data: updatedDateFilter
      }
    }
    this.dashboardFilterData = _.cloneDeep(this.dashboardFilterMasterData);
    return new Promise((resolve) => {
      resolve(_.cloneDeep(this.dashboardFilterMasterData));
    })
  }

  sortandUnique(list, dates?) {
    let sortedlist: string[] = list;//sorting is not required. *if req sort A-> Z
    if (!dates) {
      list = _.uniq(sortedlist)
    }
    else {
      list = _.uniqBy(sortedlist, (x) => {
        return formatDate(x, 'longDate', this.localID)
      });
    }
    return list;
  }

  getFilterStatus() {
    return this.isFilterAppliedOnDashBoard;
  }
  setFilterStatus(bool) {
    this.isFilterAppliedOnDashBoard = bool;
  }
  getDashBoardFilterAppliedData() {
    let obj = {
      master: _.cloneDeep(this.dashboardFilterMasterData),
      filterData: _.cloneDeep(this.dashboardFilterData)
    }
    return new Promise((resolve) => {
      resolve(obj)
    })
  }
  applyFilterDataAsync(obj, reapplyFilterBool): Promise<IApplicationLocale[]> {
    let filterCompleteData = new Object();
    for (const col in this.dashboardFilterData) {
      filterCompleteData[col] = [...this.dashboardFilterData[col]['data']]
    }

    if (!reapplyFilterBool) {
      filterCompleteData[obj.columnName] = obj.filterData;
      this.dashboardFilterData[obj.columnName]['data'] = obj.filterData;
      filterCompleteData['columnName'] = obj.columnName;
    }
    else {//set column name to any column as we are reapplying filter with all the modified cloumns
      filterCompleteData['columnName'] = 'ApplicationName';
    }

    filterCompleteData['called_from'] = obj.called_from;
    filterCompleteData['IsFirstFilter'] = !this.getFilterStatus();

    if (filterCompleteData['LocaleCode']) {
      filterCompleteData['LocaleCode'] = this.getlocalCodeDataForForm(filterCompleteData)
    }
    if (filterCompleteData['UpdatedDate']) {
      filterCompleteData['UpdatedDate'] = this.getUpdateDateDataforForm(filterCompleteData)
    }
    return this.dashboardRepository.getFilteredApplicationLocalesAsync(filterCompleteData);

  }
  getlocalCodeDataForForm(filterCompleteData) {
    return filterCompleteData['LocaleCode'] = filterCompleteData['LocaleCode'].map((x) => {
      return x.split('/')[0]
    })
  }
  getUpdateDateDataforForm(filterCompleteData) {
    return filterCompleteData['UpdatedDate'] = filterCompleteData['UpdatedDate'].map(x => {
      if (x == 'empty') {
        let d
        d = new Date('0001-01-01')
        return d
      }
      else return x
    })
  }
  getAssetsFilterDataAsync(assets) {
    let applicationFilter: string[] = [];
    let updatedDateFilter: string[] = [];
    let ifEmptyDateAvailable = false;
    assets.forEach(asset => {
      applicationFilter.push(asset.Key);
      if (!asset.UpdatedDate) {
        ifEmptyDateAvailable = true;
      }
      if (asset.UpdatedDate) {
        updatedDateFilter.push(asset.UpdatedDate);
      }
    });
    applicationFilter = this.sortandUnique(applicationFilter);
    updatedDateFilter = this.sortandUnique(updatedDateFilter, true);
    if (ifEmptyDateAvailable)
      updatedDateFilter.unshift('empty')

    let filterData = {
      'Key': { data: applicationFilter, name: 'Key' },
      'UpdatedDate': { data: updatedDateFilter, name: 'UpdatedDate' }
    }
    this.assetFilterData = _.cloneDeep(filterData)
    return new Promise((resolve) => {
      resolve(_.cloneDeep(filterData));
    })
  }

  getassetFilterstate() {
    return this.filterAppliedOnAsset;
  }
  setassetFilterState(bool: boolean) {
    this.filterAppliedOnAsset = bool;
  }
  applyFilterForAssets(obj) {
    let filterCompleteData = new Object();
    //arrange the keys from last filter state
    for (const col in this.assetFilterData) {
      filterCompleteData[col] = [...this.assetFilterData[col]['data']]
    }
    //modify key  for current filter application
    filterCompleteData[obj.columnName] = obj.filterData;//apply filter keys for current column
    this.assetFilterData[obj.columnName]['data'] = obj.filterData;//set same data for global assetfilterData
    filterCompleteData['columnName'] = obj.columnName;

    filterCompleteData['called_from'] = obj.called_from;
    filterCompleteData['IsFirstFilter'] = !this.getassetFilterstate();
    filterCompleteData['ApplicationLocaleId'] = obj.ApplicationLocaleId;

    if (filterCompleteData['UpdatedDate']) {
      filterCompleteData['UpdatedDate'] = this.getUpdateDateDataforForm(filterCompleteData)
    }

    return this.assetRepository.getAssetFilterDataAsync(filterCompleteData);

  }

}
