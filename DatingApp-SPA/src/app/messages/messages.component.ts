import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Messages } from '../_model/messages';
import { Pagination, PaginationResult } from 'src/app/_model/pagination';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Messages[];
  pagination: Pagination;
  messageContainer = 'Unread';

  constructor(private authService: AuthService
    , private userService: UserService
    , private alertify: AlertifyService
    , private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.messages = data['messages'].result;
      this.pagination = data['messages'].pagination;
    });
  }
  loadMessages() {
    this.userService.getMessages(this.authService.decodeToken.nameid, this.pagination.currentPage,
       this.pagination.itemsPerPage, this.messageContainer)
       .subscribe((paginateResulet: PaginationResult<Messages[]>) => {
        this.messages = paginateResulet.result;
        this.pagination = paginateResulet.pagination;
      }, error => {
        this.alertify.error(error);
      });
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadMessages();
}
deleteMessage(id: number) {
  this.alertify.confirm('Dali ste sigurni da zalite da obrisete poruku', () => {
    this.userService.deleteMessage(id, this.authService.decodeToken.nameid).subscribe(() => {
      this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
      this.alertify.success('Poruka je izbrisana');
    }, error  => {
      this.alertify.error('Doslo je do greske');
    });
  });
}
  /* VAZNO!!!!!!!!!!! KOD SUB metode TIP MESSAGE MOGA BITI ISTI KAO TIP U AUTH.SERVICE

  getMessages(id: number, page?, itemsPerPage?, messageContainer?) {
  const paginateResult: PaginationResult<Messages[]> = new PaginationResult<Messages[]>();
  let params = new HttpParams();

  MORA SE PAZITI NA IMPORT

  */
}
