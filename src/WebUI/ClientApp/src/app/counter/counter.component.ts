/** core imports */
import { Component } from '@angular/core';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent {

  /**
   * Current count of counter component
   */
  public currentCount = 0;

  /**
   * Increments counter
   */
  public incrementCounter() {
    this.currentCount++;
  }
}
