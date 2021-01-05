import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginationResult } from '../_model/pagination';
import { User } from '../_model/user';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  likesParam: string;

  constructor(private authService: AuthService,
    private userService: UserService,
    private route: ActivatedRoute,
    private alertufy: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].results;
      this.pagination = data ['users'].pagination;

    });
    this.likesParam = 'Likers';
  }
  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage,
       null, this.likesParam).subscribe((res: PaginationResult<User[]>) => {
      this.users = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertufy.error(error);
    });

  }
  pageChanged(event: any): void {
    this.pagination.currentPage =  event.page;
    this.loadUsers();
    console.log(this.pagination.currentPage);
  }
}
