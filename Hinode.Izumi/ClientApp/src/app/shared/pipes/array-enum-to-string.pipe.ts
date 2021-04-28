import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'arrayEnumToString'
})
export class ArrayEnumToStringPipe implements PipeTransform {

  transform(value: Array<number>, enumType: any): any {
    return value.map(x => enumType[x]).join(', ');
  }

}
