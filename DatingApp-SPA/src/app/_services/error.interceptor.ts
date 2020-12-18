import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { NEXT } from '@angular/core/src/render3/interfaces/view';
import { nextTick } from 'process';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
// import { runInNewContext } from 'vm';

@Injectable()
 export class ErrorInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError(error => {
                if (error instanceof HttpErrorResponse) {
                    if ( error.status === 401) {
                        return throwError(error.statusText);
                    }
                    const applicationError = error.headers.get('Application-Error');
                    if (applicationError) {
                        console.error(applicationError);
                        return throwError(applicationError);
                    }
                    const serverError = error.error.errors;
                    // const serverError = error.error;

                    let modalStateErrors = '';
                    if (serverError && typeof serverError === 'object') {
                            for (const key in serverError) {
                                if (serverError[key]) {
                                    modalStateErrors += serverError[key] + '\n';
                                }
                            }
                    }
                    return throwError(modalStateErrors || serverError || 'server Error' );
                }



            })
        );
    }
 }
 export const ErrorInterceptorProvider = {
     provide: HTTP_INTERCEPTORS,
     useClass: ErrorInterceptor,
     multi: true
 };
