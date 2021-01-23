import { Routes } from '@angular/router';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { HomeComponent } from './home/home.component';
import { ListComponent } from './list/list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessageTestComponent } from './message-test/message-test.component';
import { MessageChatComponent } from './message-chat/message-chat.component';

import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUsavedChangesGuard } from './_guards/prevent-usaved-changes.guard';
import { ListsResolver } from './_resolvers/lists.resolver';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';
/*

Put kooji aplikacija treba da ide kada kliknemo neki od dole navedenih opcija

*/
export const appRoutes: Routes = [
    {path: '', component: HomeComponent},
    {path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
    {path: 'members', component: MemberListComponent,  resolve: {users: MemberListResolver}},
    {path: 'members/:id', component: MemberDetailComponent, resolve: {user: MemberDetailResolver}},
    {path: 'member/edit', component: MemberEditComponent ,
    resolve: {user: MemberEditResolver}, canDeactivate: [PreventUsavedChangesGuard]},
    {path: 'messages', component: MessagesComponent, resolve: {messages: MessagesResolver}},
    // resolve koji se resolver poyiva pri kada se klikne na message na stranici
    {path: 'list', component: ListComponent, resolve: { users: ListsResolver} },
    {path: 'messages-test', component: MessageTestComponent},
    {path: 'messages-chat', component: MessageChatComponent},


    {path: 'admin', component: AdminPanelComponent, data: {roles: ['Admin', 'Moderator'] }},
 ]
    },

    {path: '**', redirectTo: '', pathMatch: 'full'},
];
