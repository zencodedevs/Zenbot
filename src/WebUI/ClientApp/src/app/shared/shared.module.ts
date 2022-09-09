/** core imports */
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

/** external imports */
import { TranslateModule } from '@ngx-translate/core';
import { UtilitiesModule } from 'zencode-utilities';

/** internal imports */


const Modules = [
    CommonModule,
    FormsModule,
    RouterModule,
    UtilitiesModule,
    TranslateModule];

@NgModule({
    imports: [...Modules],
    declarations: [],
    exports: [],
    providers: []
})
export class SharedModule { }
