import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { ValuesComponent } from './values/values.component';
import { NavComponent } from './nav/nav.component';
import { FormsModule } from '@angular/forms';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AlertifyService } from './_services/alertify.service';
import { BsDropdownModule } from 'ngx-bootstrap';


@NgModule({
  declarations: [
    AppComponent,
      ValuesComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent
   ],
  imports: [
    BrowserModule,
    HttpClientModule, // da bi smo mogli da koristimo dodatneopcije
    FormsModule,
    BsDropdownModule.forRoot()
  ],
  providers: [
    AuthService,
    ErrorInterceptorProvider,
    AlertifyService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
