export interface Messages {
    id: number;
    senderId: number;
    recipientId: number;
    senderUserName: string;
    senderPhotoUrl: string;
    recipientUserName: string;
    recipientPhotoUrl: string;
    content: string;
    isRead: boolean;
    dateRead: Date;
    messageSend: Date;
}
