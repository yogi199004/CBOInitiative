import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {

  transform(filterList: string[], searchTerm: string): string[] {
    if (filterList.length == 0) {
      return [];
    }
    if (!searchTerm || searchTerm.length < 3) {
      return filterList;
    }

    let filteredList = filterList.filter((key) => {
      return key.toLocaleLowerCase().includes(searchTerm.toLocaleLowerCase());
    });
    return this.sortFilteredList(filteredList, searchTerm)

  }

  sortFilteredList(filteredList, searchTerm): string[] {
    return filteredList.sort((a) => {
      if (a.toLocaleLowerCase() == searchTerm.toLocaleLowerCase()) {
        return -1
      }
      else {
        return 1;
      }
    })
  }

}
