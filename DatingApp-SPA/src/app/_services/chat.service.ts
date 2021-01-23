import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { MessageDto } from '../_model/MessageDto';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  baseUrl = environment.apiUrl + 'message-hub';
  hubUrl = environment.hubUrl;
  private hubConnectinon: HubConnection;

private receivedMessageObject: MessageDto = new MessageDto();
private sharedObj = new Subject<MessageDto>();
public messages: MessageDto[];

constructor(private http: HttpClient) {

  this.hubConnectinon = new  HubConnectionBuilder()
  .withUrl(this.hubUrl + 'echo', {
    accessTokenFactory: () => localStorage.getItem('token')
  })
  .withAutomaticReconnect()
  .build();


this.hubConnectinon.onclose(async () => {
await this.start();
});
this.hubConnectinon.on('ReceiveOne', (user, message) => { this.mapReceivedMessage(user, message); });
this.start();
}


// Strart the connection
public async start() {
try {
await this.hubConnectinon.start();
console.log('connected');
} catch (err) {
console.log(err);
setTimeout(() => this.start(), 5000);
}
}

private mapReceivedMessage(user: string, message: string): void {
this.receivedMessageObject.user = user;
this.receivedMessageObject.Content = message;
this.sharedObj.next(this.receivedMessageObject);
}

/* ****************************** Public Mehods **************************************** */

// Calls the controller method
public broadcastMessage(msgDto: any) {
return  this.http.post(this.baseUrl, msgDto).subscribe();
// tslint:disable-next-line: max-line-length
// this.hubConnectinon.invoke('SendMessage1', msgDto.user, msgDto.msgText).catch(err => console.error(err));    // This can invoke the server method named as "SendMethod1" directly.
}
getMessagesThread(id: number, recipientId: number ) {
  return this.http.get<MessageDto[]>(this.baseUrl + '/' + id + '/' + recipientId );
 }

public retrieveMappedObject(): Observable<MessageDto> {
return this.sharedObj.asObservable();
}

}
