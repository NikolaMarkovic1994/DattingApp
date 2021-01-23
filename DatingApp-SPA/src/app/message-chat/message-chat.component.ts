import { Component, OnInit } from '@angular/core';
import { MessageDto } from '../_model/MessageDto';
import { Messages } from '../_model/messages';
import { ChatService } from '../_services/chat.service';
import { PresenceService } from '../_services/presence.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-message-chat',
  templateUrl: './message-chat.component.html',
  styleUrls: ['./message-chat.component.css']
})
export class MessageChatComponent implements OnInit {
  msgDto: MessageDto = new MessageDto();
  msgInboxArray: MessageDto[] = [];
  msgInboxArray1: MessageDto[] = [];
  messages: Messages[];

  constructor(private chatService: ChatService, public presence: PresenceService, private userService: UserService) {}

  ngOnInit(): void {
    this.chatService.retrieveMappedObject().subscribe( (receivedObj: MessageDto) => { this.addToInbox(receivedObj); });
    // calls the service method to get the new messages sent
  }



  send(): void {
    if (this.msgDto) {
      if (this.msgDto.user.length === 0 || this.msgDto.user.length === 0) {
        window.alert('Both fields are required.');
        return;
      } else {
        this.chatService.broadcastMessage(this.msgDto);
        console.log(this.msgDto);                   // Send the message via a service
      }
    }
  }

  addToInbox(obj: MessageDto) {
    const newObj = new MessageDto();



    newObj.user = obj.user;
    newObj.Content = obj.Content;
    newObj.RecipientId = obj.RecipientId;
    newObj.SenderId = obj.SenderId;

    this.msgInboxArray.push(newObj);



  }
}
