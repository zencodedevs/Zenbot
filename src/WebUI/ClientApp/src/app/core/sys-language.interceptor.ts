/** core imports */
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, } from '@angular/common/http';
import { Injectable } from '@angular/core';

/** external imports */
import { Observable } from 'rxjs';

/** internal imports */
import { X_SYSTEM_LANGUAGE } from '../shared/constant';
import { LanguageService } from './sys-language.service';

@Injectable({
  providedIn: 'root',
})
export class SysLanguageInterceptor implements HttpInterceptor {

  /**
   *
   * @param languageService
   */
  constructor(private languageService: LanguageService) { }

  /**
   *
   * @param req
   * @param next
   * @returns
   */
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    req = req.clone({
      method: req.method,
      url: req.url,
      headers: req.headers.set(X_SYSTEM_LANGUAGE, this.languageService.language)
    });

    return next.handle(req);
  }
}
