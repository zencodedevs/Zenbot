/** core imports */
import { Component, OnInit } from '@angular/core';

/** external imports */
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {

  /**
   * Determines whether expanded is
   */
  isExpanded = false;

  /**
   * Determines whether production is
   */
  isProduction: any = false;

  /**
   * on init
   */
  ngOnInit() : void {
    this.isProduction = environment.production;
  }

  /**
   * Collapses nav menu component
   */
  collapse() {
    this.isExpanded = false;
  }

  /**
   * Toggles nav menu component
   */
  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
