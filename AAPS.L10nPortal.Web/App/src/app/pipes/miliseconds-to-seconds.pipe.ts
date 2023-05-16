import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'milisecondsToSecondsPipe'
})
export class MilisecondsToSecondsPipe implements PipeTransform {
  transform(time: number): string {
    if (time <= 0) {
      return "00";
    }

    var seconds = ((time / 1000) % 60).toString();
    var roundedSeconds = parseInt(seconds);

    return roundedSeconds < 10 ? "0" + roundedSeconds.toString() : roundedSeconds.toString();
  };
}
