import { Component, OnInit, Output, EventEmitter, Input, OnDestroy, SimpleChanges, ChangeDetectorRef } from '@angular/core';
import * as _ from 'lodash';
import * as $ from 'jquery';
import { Subscription } from 'rxjs';
import { FilterDataService } from '../../services/filter-data.service';
import { FilterPipe } from '../../pipes/filter.pipe';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sort-filter',
  templateUrl: './sort-filter.component.html',
  styleUrls: ['./sort-filter.component.scss']
})
export class SortFilterComponent implements OnInit {
  currentSortColumn: any;
  isSortingDesc: boolean;
  assetSearchTerm: string = ''
  dashBoardUrl = '/';
  assetLinkUrl = '/'
  private clearFilterSubscription: Subscription;
  reApplyFilterSubscription: Subscription;
  assetLinksFilterSubscription: Subscription;
  clearFilterReceived: boolean = false;
  constructor(private filterDataService: FilterDataService,
    private ChangeDetectorRef: ChangeDetectorRef,
    private filterPipe: FilterPipe,
    private router: Router) {

  }
  filterList = [];
  unMutatedList = [];


  @Output() sortFilterFromChild: EventEmitter<any> = new EventEmitter();
  @Input() dropDownFilterData: any
  @Input() currentSortedColumn: String
  reverseSorted: boolean = false;

  ngOnInit(): void {
    this.generateNewFilterList()
    this.clearFilterSubscribe();
    if (this.router.url == this.dashBoardUrl) {

      this.reApplyFilterSubscribe();
    }
    else if (this.router.url.includes('/asset-links')) {
      this.applyAssetLinkFilter();
    }
  }


  ngAfterViewInit(): void {

    //Called after ngAfterContentInit when the component's view has been initialized. Applies to components only.
    //Add 'implements AfterViewInit' to the class.
    $('body').on('hide.bs.dropdown', () => {
      this.cancelChanges();
    });

  }
  ngOnChanges(changes: SimpleChanges) {
    if (this.clearFilterReceived) {
      this.generateNewFilterList();
      this.clearFilterReceived = false;
    }
    // this.generateNewFilterList();
  }

  generateNewFilterList() {
    this.filterList = []
    this.filterList = _.cloneDeep(this.dropDownFilterData.data);
    this.unMutatedList = _.cloneDeep(this.dropDownFilterData.data);
  }

  isUpActive(): boolean {
    if (this.currentSortedColumn == this.dropDownFilterData.name &&
      this.reverseSorted == false) {
      return true
    }
    else {
      return false
    }
  }
  isDownActive(): boolean {
    if (this.currentSortedColumn == this.dropDownFilterData.name &&
      this.reverseSorted == true) {
      return true
    }
    else {
      return false
    }
  }
  showSearchTermInput(): boolean {
    if (this.dropDownFilterData.name == 'Key') {
      return true;
    }
    else {
      return false;
    }
  }
  sort(columnName: string, direction: String) {
    const obj = {
      columnName, direction,
      type: 'sort'
    }

    this.reverseSorted = direction == 'down' ? true : false
    this.sortFilterFromChild.emit(obj)
  }
  cancelChanges() {
    this.filterList = _.cloneDeep(this.unMutatedList);
    if (this.assetSearchTerm) {
      this.assetSearchTerm = '';
    }
    this.ChangeDetectorRef.detectChanges();
  }
  checkForChecked(value) {
    if (value == 'all') {
      return this.filterList.length == this.dropDownFilterData.data.length ? true : false
    }
    else {
      let index = _.findIndex(this.filterList, (x) => { return x == value });
      return index > -1 ? true : false;

    }
  }

  selectAll(event) {
    if (event.target.checked) {
      this.filterList = _.cloneDeep(this.dropDownFilterData.data);
    }
    else {
      this.filterList = [];
    }
  }

  onfilterSelect(e) {
    if (e.target.checked) {
      this.filterList.push(e.target.value)
    }
    else {
      _.remove(this.filterList, (x) => {
        return x == e.target.value
      });
    }
  }

  applyFilter() {
    let obj;
    if (this.assetSearchTerm && this.dropDownFilterData.name == 'Key') {//if on asset page user have aaplied an search term
      this.filterList = this.filterPipe.transform(this.filterList, this.assetSearchTerm);
    }
    this.unMutatedList = _.cloneDeep(this.filterList);
    obj = {
      'columnName': this.dropDownFilterData.name,
      'type': 'filter',
      'filterData': this.filterList
    }

    this.sortFilterFromChild.emit(obj);
    this.assetSearchTerm = ''
  }

  clearFilterSubscribe() {
    this.clearFilterSubscription = this.filterDataService.clearFilter.subscribe((bool) => {
      if (bool) {
        this.clearFilterReceived = true;
      }
    })
  }
  reApplyFilterSubscribe() {
    this.reApplyFilterSubscription = this.filterDataService.reApplyFilterOnDashBoard.subscribe((obj) => {
      if (obj) {
        this.filterList = _.cloneDeep(obj[this.dropDownFilterData.name].data);
        this.unMutatedList = _.cloneDeep(obj[this.dropDownFilterData.name].data);
      }
    })
  }
  applyAssetLinkFilter() {
    this.assetLinksFilterSubscription = this.filterDataService.assetLinksFilter.subscribe((obj) => {
      if (obj) {
        this.filterList = _.cloneDeep(obj[this.dropDownFilterData.name].data);
        this.unMutatedList = _.cloneDeep(obj[this.dropDownFilterData.name].data);
      }
    })
  }
  ngOnDestroy(): void {
    if (this.clearFilterSubscription)
      this.clearFilterSubscription.unsubscribe();
    if (this.reApplyFilterSubscription)
      this.reApplyFilterSubscription.unsubscribe();
  }
}
