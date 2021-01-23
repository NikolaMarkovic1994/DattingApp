import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { AlertifyService } from './alertify.service';
import { User } from '../_model/user';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  userConested: User;
  hubUrl = environment.hubUrl;
  private hubConnectinon: HubConnection;
  private onlineUsersSource = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSource.asObservable();



constructor(private alertfy: AlertifyService) { }

createHubConnection(user: User) {
  this.hubConnectinon = new  HubConnectionBuilder()
  .withUrl(this.hubUrl + 'presence', {
    accessTokenFactory: () => localStorage.getItem('token')
  })
  .withAutomaticReconnect()
  .build();
    this.userConested = user;
  this.hubConnectinon.start()
  .catch(error => console.log(error));


  this.hubConnectinon.on('UserIsOnline', username => {
    this.alertfy.success(username + ' Has conested');
  });

  this.hubConnectinon.off('UserIsOffline', username => {
    this.alertfy.message(username + ' Has Deconested');
  });
  this.hubConnectinon.on('GetOnlineUsers', (username: string[]) => {
    this.onlineUsersSource.next(username);
  });
}
/**
 *
 *
 *
 *
 */
createHubConnectionTest(user: User, token: string) {
  this.hubConnectinon = new  HubConnectionBuilder()
  .withUrl(this.hubUrl + 'presence', {
    accessTokenFactory: () => token
  })
  .withAutomaticReconnect()
  .build();


  this.hubConnectinon.start()
  .catch(error => console.log(error));


  this.hubConnectinon.on('UserIsOnline', userName => {
    this.alertfy.success(userName + ' Has conested');
  });

  this.hubConnectinon.off('UserIsOffline', Name => {
    this.alertfy.message(Name + ' Has Deconested');
  });
}

stopHubConnection() {
 this.hubConnectinon.stop().catch(error => console.log(error));
}

}
