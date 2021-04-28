import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {ArrayEnumToStringPipe} from "./pipes/array-enum-to-string.pipe";
import {EnumToStringPipe} from "./pipes/enum-to-string.pipe";
import { EnumToArrayPipe } from './pipes/enum-to-array.pipe';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
  ],
  declarations: [
    EnumToStringPipe,
    ArrayEnumToStringPipe,
    EnumToArrayPipe
  ],
  exports: [
    EnumToStringPipe,
    ArrayEnumToStringPipe,
    EnumToArrayPipe
  ]
})
export class SharedModule {
}
