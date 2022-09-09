/** core imports */
import { Injectable, ErrorHandler } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AppErrorHandler extends ErrorHandler {

    /**
     * Handles error
     * @param error
     */
    handleError(error: any): void {
        console.error(error);
        super.handleError(error);
    }

}
