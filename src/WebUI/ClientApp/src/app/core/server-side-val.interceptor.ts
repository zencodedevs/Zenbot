
/** core imports */
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Inject, Injectable, Injector } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

/** external imports */
import { tap, mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class ServerSideValidationInterceptor implements HttpInterceptor {

    /**
     * Creates an instance of server side validation interceptor.
     * @param injector
     */
    constructor(@Inject(Injector) private readonly injector: Injector) { }

    /**
     * Gets toastr service
     */
    private get toastrService(): ToastrService {
        return this.injector.get(ToastrService);
    }

    // tslint:disable-next-line:typedef
    private errorToClientResult(data: any) {
        const e: any = JSON.parse(data.response);
        if (e) {
            const eArray = [];
            if (Array.isArray(e)) {
                eArray.push(...e);
            }
            else {
                // tslint:disable-next-line: forin
                for (const item in e.errors) {
                    eArray.push(...e.errors[item]);
                }
            }
            return eArray.join('<br>');
        }

        return null;
    }

    /**
     * Intercepts server side validation interceptor
     * @param req
     * @param next
     * @returns intercept
     */
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        return next.handle(req).pipe(
            tap((event: HttpEvent<any>) => {
                if (event instanceof HttpResponse) {

                }
            }, (err: any) => {

                if (err instanceof HttpErrorResponse) {

                    // unauthorize
                    if (err.status === 401) {
                        this.toastrService.error('სესია არავალიდურია.გთხოვთ გაიაროთ ავტორიზაცია!', 'შეცდომა');
                    }

                    // forbidden
                    if (err.status === 403) {
                        this.toastrService.error('არ გაქვთ ოპერაციის შესრულების უფლება!', 'გაფრთხილება');
                    }

                    // other error handler
                    if (err.status !== 200 && err.status !== 204) {

                        const responseBlob = err instanceof HttpResponse ? err.body :
                            (err).error instanceof Blob ? (err).error : undefined;

                        new Observable<string>((observer: any) => {
                            if (!responseBlob) {
                                observer.next('');
                                observer.complete();
                            } else {
                                const reader = new FileReader();
                                reader.onload = event => {
                                    observer.next((event?.target)?.result);
                                    observer.complete();
                                };
                                reader.readAsText(responseBlob);
                            }
                        }).pipe(_observableMergeMap((value, index): any => {

                            if (!value) {
                                return undefined;
                            }

                            if (JSON.parse(value).hasOwnProperty('errors') || Array.isArray(JSON.parse(value))) {

                                const errorMsg: any = this.errorToClientResult({
                                    response: value
                                });

                                this.toastrService.error(errorMsg, 'შეცდომა');

                            } else {

                                this.toastrService.error('დაფიქსირდა შიდა სისტემური შეცდომა!', 'შეცდომა');
                            }

                            return undefined;

                        })).toPromise();
                    }
                }
            }));
    }
}
