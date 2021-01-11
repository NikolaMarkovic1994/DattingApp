import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Messages } from '../_model/messages';

import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';
/*

Koriste sae kako bi pokupili podatke prenego sto ih klijent zatrazi
odnosno se uvita odmah a posle se prikayuje korisniku kad ih on zatrazi
Moraju biti dodati u App Module
Mora biti dodatu i u Routs
*/
@Injectable()
export class MessagesResolver implements Resolve<Messages[]> {
    pageNumber = 1;
    pageSize = 8;
    messageContainer = 'Unread';
    constructor(private userService: UserService,
         private route: Router,
         private alertify: AlertifyService,
         private authService: AuthService ) {}
    resolve(route: ActivatedRouteSnapshot): Observable<Messages[]> {
        return this.userService.getMessages(this.authService.decodeToken.nameid,
             this.pageNumber, this.pageSize, this.messageContainer).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving messages');
                this.route.navigate(['/home']);
                return of(null);
            })
        );
    }
}
