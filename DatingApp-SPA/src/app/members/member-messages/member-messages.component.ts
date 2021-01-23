import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { tap } from 'rxjs/operators';
import { Messages } from 'src/app/_model/messages';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm: NgForm;
  @Input() recipientId: number;
  messages: Messages[];


  newMessage: any = {};
  constructor(public userService: UserService, private alertufy: AlertifyService , private authService: AuthService ) { }
  public hubConnection: HubConnection;
  hubUrl = environment.hubUrl;
  ngOnInit() {



    this.loadMessages();
    //
   // this.loatMessageTest();


  }

  loatMessageTest() {

    this.userService.createHubConnection(this.recipientId);
     console.log(this.userService.messageThread$ );
  }



  loadMessages() {
    const currUserId = +this.authService.decodeToken.nameid; // + pretvara anz u number
    this.userService.getMessagesThread(this.authService.decodeToken.nameid, this.recipientId)
    .pipe(
      tap(messages => {
        for (let index = 0; index < messages.length; index++) {
          if (messages[index].isRead === false && messages[index].recipientId === currUserId ) {
            this.userService.markAsRead(currUserId, messages[index].id);
          }

        }
      })
    )
    .subscribe(messages => {
      this.messages = messages;
    }, error => {
      this.alertufy.error(error);
    });
  }
  sendMessage() {
    this.newMessage.recipientId = this.recipientId;
    // this.userService.sendMessage(this.authService.decodeToken.nameid, this.newMessage).subscribe((messages: Messages) => {
    //   this.messages.unshift(messages);
    //   this.newMessage.content = '';
    // }, error => {
    //   this.alertufy.error(error);
    // });
    // this.userService.testMessageGest().then();
    this.userService.sendMessage( this.newMessage, this.authService.decodeToken.nameid).then((messages: Messages[]) => {});
    console.log(this.userService.testMessageGest().then((messages: Messages[]) => {}));
    // this.userService.sendMessage(this.authService.decodeToken.nameid, this.newMessage).then((messages: Messages) => {});
      //   this.messages.unshift(messages);
      //   this.newMessage.content = '';
      // }, error => {
      //   this.alertufy.error(error);
      // });
  }
  sendMessage1() {
    this.newMessage.recipientId = this.recipientId;
    this.userService.sendMessage1( this.authService.decodeToken.nameid, this.newMessage).then(() => {
      this.messageForm.reset();
    });
  }

}
