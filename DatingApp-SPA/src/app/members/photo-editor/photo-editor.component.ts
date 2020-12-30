import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Photo } from 'src/app/_model/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Output() getMemberPhotoChange = new EventEmitter<string>();
   uploader: FileUploader ;
   hasBaseDropZoneOver = false;
   baseURl = environment.apiUrl;
   curentMain: Photo;




  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.initalizeUploader();
  }
    fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initalizeUploader() {
    this.uploader = new FileUploader({
      url: this.baseURl + 'users/' + this.authService.decodeToken.nameid + '/photos',
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response); // pretvara token is sringa u objekat
        const photo = {
          id: res.id,
          url: res.url,
          description: res.description,
          dateAdded: res.dateAdded,
          isMain: res.isMain,
        };
        this.photos.push(photo);
        if (photo.isMain) {
          this.authService.changeMemberPhoto(photo.url);
          this.authService.curentUser.photoUrl = photo.url;
          localStorage.setItem('user', JSON.stringify(this.authService.curentUser));
        }

      }
    };

  }
  setManiP(photo: Photo) {
            this.userService.setMainPhoto(this.authService.decodeToken.nameid, photo.id).subscribe(() => {
      this.curentMain = this.photos.filter(p => p.isMain === true)[0]; // vraca listu slika gde je ismain=true [0] vraca samo jednu skiku
      this.curentMain.isMain = false;
      photo.isMain = true;
      this.authService.changeMemberPhoto(photo.url);
      this.authService.curentUser.photoUrl = photo.url;
      localStorage.setItem('user', JSON.stringify(this.authService.curentUser));
        }, error => {
      this.alertify.error(error);

    });
  }
  deletePhoto( id: number) {
    this.alertify.confirm('Da li zelite da obrisete sliku', () => {

      this.userService.deletePhoto(this.authService.decodeToken.nameid, id).subscribe(() => {
        this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
        this.alertify.success('Slika obrisana');
      }, error => {
        this.alertify.error('Doslo je do greske');
      });

        });
   // this.alertify.confirm('Dali sete sigurni da zelit da obrisete sliku', () => {
      // this.userService.delatePhot(this.authService.decodeToken.nameid, id).subscribe(() => {
      //   this.photos.slice(this.photos.findIndex(p => p.id === id), 1);
      //   this.alertify.success('Slika obrisana');
      // }, error => {
      //   this.alertify.error('Doslo je do greske');
      // });
    // });
    // this.userService.delatePhot(this.authService.decodeToken.nameid, id).subscribe(() => {
    //       this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
    //       this.alertify.success('Slika obrisana');
    //     }, error => {
    //            this.alertify.error('Doslo je do greske AAAAAAAAAAs');
    //          });
  }
}
