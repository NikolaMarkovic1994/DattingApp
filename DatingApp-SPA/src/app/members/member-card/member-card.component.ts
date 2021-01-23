import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/_model/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { PresenceService } from 'src/app/_services/presence.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() user: User;

  constructor(private authService: AuthService
    , private userService: UserService
    , private alertify: AlertifyService
    , public presence: PresenceService) { }

  ngOnInit() {
  }
  sendLike(id: number) {
    this.userService.sendLike(this.authService.decodeToken.nameid, id).subscribe(u => {
      this.alertify.success('You liked:' + this.user.userName);
    }, error => {
      this.alertify.error(error);
    });
  }
}
