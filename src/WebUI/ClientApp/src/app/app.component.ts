/** core imports */
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {

  /**
   * Title  of app component
   */
  title = 'app';

  /**
   * Determines whether started progress bar on
   */
  onStartedProgressBar(): void { }

  /**
   * Determines whether completed progress bar on
   */
  onCompletedProgressBar(): void { }
}
