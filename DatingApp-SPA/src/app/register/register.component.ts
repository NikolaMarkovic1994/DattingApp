import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from '../_model/user';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  user: User;
  regForm: FormGroup;
  constructor(private authService: AuthService, private router: Router,
     private alertify: AlertifyService , private fb: FormBuilder) { }
  // pozivama AuthService kako bi smo koristili u ovoj komponenti

  ngOnInit() {
    // this.regForm = new FormGroup({
    //   username: new FormControl('', Validators.required),
    //   pssword: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
    //   confirmPssword: new FormControl('', Validators.required)

    // }, this.passMatchValidator);
    this.createRegForm();
  }
  createRegForm() {
    this.regForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      pssword: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPssword: ['', Validators.required]
    }, {validator: this.passMatchValidator});
  }
  passMatchValidator(g: FormGroup) {
    return g.get('pssword').value === g.get('confirmPssword').value ? null : {'mismatch': true};
  }
  registar() {
    if ( this.regForm.valid) {
      this.user = Object.assign({}, this.regForm.value);
      // klonira vrednost reForm u prazan objekat i dodeljuje ga user
      this.authService.register(this.user).subscribe(() => {
      this.alertify.success('REG SUCCCC');
      }, error => {
        this.alertify.error(error);
      }
      // , () => {
      //   var model:any;
      //   // this.authService.loging(this.user).subscribe(() => {
      //   //   this.router.navigate(['/members']);
      //   // });
      // }
      );
    }
    // // console.log(this.model);
    // this.authService.register(this.model).subscribe(() => {
    //   this.alertify.success('REG SUCCCC');
    // }, error => {
    //   this.alertify.error(error);

    // });
    console.log(this.regForm.value);
  }
  cansel() {
    this.cancelRegister.emit(false);

  }

}
