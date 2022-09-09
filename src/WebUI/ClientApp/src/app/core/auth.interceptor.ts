/** core imports */
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, } from '@angular/common/http';
import { Injectable } from '@angular/core';

/** external imports */
import { Observable } from 'rxjs';

/** internal imports */
import { X_AUTH_TOKEN } from '../shared/constant';

@Injectable({
    providedIn: 'root',
})
export class AuthInterceptor implements HttpInterceptor {

    /**
     * Creates an instance of auth interceptor.
     */
    constructor() { }

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
            headers: req.headers.set(X_AUTH_TOKEN, '')
        });

        return next.handle(req);
    }
}
