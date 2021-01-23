import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import * as directMessagesAction from '../_store/directmessages.action';
import { OnlineUser } from '../_model/online-user';
import { DirectMessage } from '../_model/direct-message';
import { DirectMessagesState } from '../_store/directmessages.state';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-message-test',
  templateUrl: './message-test.component.html',
  styleUrls: ['./message-test.component.css']
})
export class MessageTestComponent implements OnInit {
  public message = '';
  public messages: string[] = [];
  public hubConnection: HubConnection;
  hubUrl = environment.hubUrl;


  constructor() {}


  ngOnInit() {
    this.hubConnection = new  HubConnectionBuilder()
  .withUrl(this.hubUrl + 'echo', {
    accessTokenFactory: () => localStorage.getItem('token')
  })
  .withAutomaticReconnect()
  .build();

    // this lines up with the method called by `SendAsync`
    this.hubConnection.on('NewMessage', (msg) => {
        this.messages.push(msg);
    });



    // this will start the long polling connection
    this.hubConnection.start()
        .then(() => { console.log('Connection started'); })
        .catch(err => { console.error(err); });
  }
  echo() {
    // this will call the method in the EchoHub
    this.hubConnection.invoke('Echo', this.message);
}

}
