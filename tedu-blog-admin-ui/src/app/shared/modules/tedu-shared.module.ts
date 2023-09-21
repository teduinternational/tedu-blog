import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ValidationMessageComponent } from './validation-message/validation-message.component';

@NgModule({
  imports: [CommonModule],
  declarations: [ValidationMessageComponent],
  exports: [ValidationMessageComponent],
})
export class TeduSharedModule {}
