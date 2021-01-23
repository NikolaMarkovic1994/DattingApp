import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Messages } from '../_model/messages';
import { PaginationResult } from '../_model/pagination';
import { User } from '../_model/user';
import { BehaviorSubject } from 'rxjs';
import { Group } from '../_model/group';


@Injectable({
  providedIn: 'root'
})
export class UserService {
   baseUrl = environment.apiUrl + 'users/';
   hubUrl = environment.hubUrl;
   private hubConnectinon: HubConnection;
   private messageThreadSource = new BehaviorSubject<Messages[]>([]);

    messageThread$ = this.messageThreadSource.asObservable();




constructor(private http: HttpClient) { }





createHubConnection( otherUserName: number) {
  this.hubConnectinon = new  HubConnectionBuilder()
  .withUrl(this.hubUrl + 'messages?user=' + otherUserName + '', {
    accessTokenFactory: () => localStorage.getItem('token')
  })
  .withAutomaticReconnect()
  .build();

  this.hubConnectinon.start()
  .catch(error => console.log(error));


  this.hubConnectinon.on('ReciveMessageThread', messages => {
     this.messageThreadSource.next(messages);

  });

  this.hubConnectinon.on('NewMessage', message => {
    this.messageThread$.pipe(take(1)).subscribe(messages => {
      this.messageThreadSource.next([...messages, message]);
    });
  });
  this.hubConnectinon.on('UpdatedGroup', (group: Group) => {
    if (group.connections.some(x => x.username === 'sonja')) {
      this.messageThread$.pipe(take(1)).subscribe(messages => {
        messages.forEach(message => {
          if (!message.dateRead) {
            message.dateRead = new Date(Date.now());
          }
        });
        this.messageThreadSource.next([...messages]);
      });
    }
  });


}

async testMessageGest() {
  return this.hubConnectinon.invoke('GetMessages')
  .catch(error => console.log(error));
}

stopHubConnection() {
  this.hubConnectinon.stop().catch(error => console.log(error));
 }













getUsers(page?, itemsPerPage?, userParams?, likesParams?): Observable<PaginationResult<User[]>> {
  const paginateResulet: PaginationResult<User[]> = new PaginationResult<User[]>();
  let params = new HttpParams();
  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page);
    params = params.append('pageSize', itemsPerPage);

  }
  if (userParams != null) {
    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

  }
  if (likesParams === 'Likers') {
    params = params.append('likers', 'true');
  }
  if (likesParams === 'Likees') {
    params = params.append('likees', 'true');
  }
  return this.http.get<User[]>(this.baseUrl + 'userss', {observe : 'response', params})
  .pipe(
    map(response => {
      paginateResulet.result = response.body;
      if (response.headers.get('Pagination') != null) {
        paginateResulet.pagination = JSON.parse(response.headers.get('Pagination'));

      }
      return paginateResulet;
    })
  );
}
getUser(id): Observable<User> {
  return this.http.get<User>(this.baseUrl + id);
}
upadteUser(id: number, userName: string, user: User) {
  return this.http.put(this.baseUrl + id + '/' + userName, user);
}
setMainPhoto(userId: number, id: number) {
  return this.http.post(this.baseUrl + userId + '/photos/' + id + '/ismain', {});
}

deletePhoto(userId: number , id: number) {
  return this.http.delete(this.baseUrl + userId + '/photos/' + id);

}
sendLike(id: number , recipientId: number) {
  return this.http.post(this.baseUrl +  id + '/like/' + recipientId, {});
  // http post mora da posanje nesto u telo alo je mosze moslati prazno {}

}

getMessages(id: number, page?, itemsPerPage?, messageContainer?) {
  const paginateResult: PaginationResult<Messages[]> = new PaginationResult<Messages[]>();
  let params = new HttpParams();

  params = params.append('MessageContainer', messageContainer);
  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page);
    params = params.append('pageSize', itemsPerPage);

  }
  return this.http.get<Messages[]>(this.baseUrl +  id + '/messages' , {observe : 'response', params})
  .pipe(
    map(response => {
      paginateResult.result = response.body;
      if (response.headers.get('Pagination') != null) {
        paginateResult.pagination = JSON.parse(response.headers.get('Pagination'));

      }
      return paginateResult;
    })
  );

  // da bi dobili paginate motamo koristiti get + <> naglasiti koji tip pofataka uyimamo iy mazae
  // yato pravimo interfejs koji ima iste parametre kao i klasa sa istim imenom u Models folderu u APi-u

}
getMessagesThread(id: number, recipientId: number ) {
 return this.http.get<Messages[]>(this.baseUrl +  id + '/messages/tread/' + recipientId );
}
/**
 *
 *
 */
async sendMessage(id: number , message: Messages) {
  // return this.http.post(this.baseUrl +  id + '/messages', message); //
  return this.hubConnectinon.invoke('SendMessage',  message, id)
  .catch(error => console.log(error));
}
async sendMessage1(id: number , message: Messages) {
  return this.hubConnectinon.invoke('SendMessage',  id, message)
    .catch(error => console.log(error));
}
/**
 *
 *
 *
 */
deleteMessage(id: number, userId: number) {
  return this.http.post(this.baseUrl +  userId + '/messages/' + id, {});
  // { } za post metodu gde se saljen neki objekat
 }
 markAsRead(userId: number, messageId: number) {
  return this.http.post(this.baseUrl +  userId + '/messages/' + messageId + '/read', {})
  .subscribe();

 }

}
