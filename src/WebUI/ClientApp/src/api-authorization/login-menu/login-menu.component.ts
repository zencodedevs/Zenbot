/** core imports */
import { Component, OnInit } from '@angular/core';

/** external imports */
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

/** internal imports */
import { AuthorizeService } from '../authorize.service';

@Component({
  selector: 'app-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.scss']
})
export class LoginMenuComponent implements OnInit {

  /**
   * Determines whether authenticated is
   */
  public isAuthenticated: Observable<boolean>;

  /**
   * User name of login menu component
   */
  public userName: Observable<string>;

  /**
   * Creates an instance of login menu component.
   * @param authorizeService
   */
  constructor(private authorizeService: AuthorizeService) { }

  /**
   * on init
   */
  ngOnInit() {

    this.isAuthenticated = this.authorizeService.isAuthenticated();

    this.userName = this.authorizeService.getUser().pipe(map(u => u && u["x-user-name"]));

  }
}
