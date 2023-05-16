import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sortPipe'
})
export class sortPipe implements PipeTransform {
  transform(value: any[], direc: string): any[] {
    let multiplier = 1;
    if (direc == 'desc') {
      multiplier = -1
    }



    return value;
  };
}
