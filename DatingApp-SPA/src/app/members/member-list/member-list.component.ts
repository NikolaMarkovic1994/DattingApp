import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginationResult } from 'src/app/_model/pagination';
import { User } from '../../_model/user';
import { AlertifyService } from '../../_services/alertify.service';
import { UserService } from '../../_services/user.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  users: User[];
  user: User = JSON.parse(localStorage.getItem('user'));
  genderList = [{value: 'male', display: 'Male'}, {value: 'female', display: 'Female'}];
  userParams: any = {};
  pagination: Pagination;
  constructor(private userService: UserService, private alertufy: AlertifyService, private route: ActivatedRoute) {}



  ngOnInit() {
   // this.loadUsers();
   this.route.data.subscribe(data => {
     this.users = data['users'].result;
     this.pagination = data ['users'].pagination;

   });
   this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
   this.userParams.minAge = 18;
   this.userParams.maxAge = 99;

  }

  resetFilters() {
    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = 'age';
    this.loadUsers();
  }
  pageChanged(event: any): void {
    this.pagination.currentPage =  event.page;
    this.loadUsers();
    console.log(this.pagination.currentPage);
  }
  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage,
       this.userParams).subscribe((res: PaginationResult<User[]>) => {
      this.users = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertufy.error(error);
    });






  }

}
