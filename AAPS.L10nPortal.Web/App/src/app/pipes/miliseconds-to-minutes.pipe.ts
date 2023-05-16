import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'milisecondsToMinutesPipe'
})
export class MilisecondsToMinutesPipe implements PipeTransform {
  transform(time: number): string {
    if (time <= 0) {
      return "00";
    }

    var minutes = (time / 60 / 1000).toString();
    var roundedMinutes = parseInt(minutes);

    return roundedMinutes < 10 ? "0" + roundedMinutes.toString() : roundedMinutes.toString();
  };
}
