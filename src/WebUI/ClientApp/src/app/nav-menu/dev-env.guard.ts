/** core imports */
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from "@angular/router";

/** external imports */
import { Observable } from "rxjs";

/** internal imports */
import { environment } from "src/environments/environment";

@Injectable({
  providedIn: 'root'
})
export class DevEnvGuard implements CanActivate {

  /**
   * Creates an instance of dev env guard.
   */
  constructor() { }

  /**
   * Determines whether activate can
   * @param route
   * @param state
   * @returns activate
   */
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    return !environment.production;
  }
}
