import { OnlineUser } from '../_model/online-user';
import { DirectMessage } from '../_model/direct-message';

export interface DirectMessagesStateContainer {
    onlineUsers: OnlineUser[];
    directMessages: DirectMessage[];
    connected: boolean;
}

export interface DirectMessagesState {
    dm: DirectMessagesStateContainer;
    onlineUsers: OnlineUser[];
    directMessages: DirectMessage[];
    connected: boolean;
}
