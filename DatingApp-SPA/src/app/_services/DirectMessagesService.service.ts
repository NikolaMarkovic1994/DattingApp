import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection } from '@microsoft/signalr';
import signalR = require('@microsoft/signalr');
import { Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { OnlineUser } from '../_model/online-user';
import * as directMessagesActions from '../_store/directmessages.action';

@Injectable({
  providedIn: 'root'
})
export class DirectMessagesServiceService {

  hubUrl = environment.hubUrl;
  private _hubConnection: HubConnection;
  private headers: HttpHeaders | undefined;

  isAuthorizedSubscription: Subscription | undefined;
  isAuthorized = false;

  constructor(
      private store: Store<any>,

  ) {
      this.headers = new HttpHeaders();
      this.headers = this.headers.set('Content-Type', 'application/json');
      this.headers = this.headers.set('Accept', 'application/json');

      this.init();
  }

  sendDirectMessage(message: string, userId: string): string {

      if (this._hubConnection) {
          this._hubConnection.invoke('SendDirectMessage', message, userId);
      }
      return message;
  }

  leave(): void {
      if (this._hubConnection) {
          this._hubConnection.invoke('Leave');
      }
  }

  join(): void {
      console.log('send join');
      if (this._hubConnection) {
          this._hubConnection.invoke('Join');
      }
  }

  private init() {

                  this.initHub();

  }

  private initHub() {
      console.log('initHub');



      this._hubConnection = new signalR.HubConnectionBuilder()
          .withUrl(this.hubUrl + 'messages-test', {
            accessTokenFactory: () => localStorage.getItem('token')
          })
          .configureLogging(signalR.LogLevel.Information)
          .build();

      this._hubConnection.start().catch(err => console.error(err.toString()));

      this._hubConnection.on('NewOnlineUser', (onlineUser: OnlineUser) => {
          console.log('NewOnlineUser received');
          console.log(onlineUser);
          this.store.dispatch(new directMessagesActions.ReceivedNewOnlineUser(onlineUser));
      });

      this._hubConnection.on('OnlineUsers', (onlineUsers: OnlineUser[]) => {
          console.log('OnlineUsers received');
          console.log(onlineUsers);
          this.store.dispatch(new directMessagesActions.ReceivedOnlineUsers(onlineUsers));
      });

      this._hubConnection.on('Joined', (onlineUser: OnlineUser) => {
          console.log('Joined received');
          this.store.dispatch(new directMessagesActions.JoinSent());
          console.log(onlineUser);
      });

      this._hubConnection.on('SendDM', (message: string, onlineUser: OnlineUser) => {
          console.log('SendDM received');
          this.store.dispatch(new directMessagesActions.ReceivedDirectMessage(message, onlineUser));
      });

      this._hubConnection.on('UserLeft', (name: string) => {
          console.log('UserLeft received');
          this.store.dispatch(new directMessagesActions.ReceivedUserLeft(name));
      });

}
}
