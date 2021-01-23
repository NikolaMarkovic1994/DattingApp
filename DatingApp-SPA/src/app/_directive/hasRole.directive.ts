import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective {
@Input() appHasRole: string[];
isVisible = false;

  constructor(private viewContainerRef: ViewContainerRef,
    private templateRef: TemplateRef<any>,
    private authService: AuthService ) { }

  // tslint:disable-next-line: use-life-cycle-interface
  ngOnInit() {
    const userRoles = this.authService.decodeToken.role as Array<string>;

    if (!userRoles) {
      this.viewContainerRef.clear();
    }
    if (this.authService.roelMatch(this.appHasRole)) {
      this.isVisible = true;
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    } else {
      this.isVisible = false;
      this.viewContainerRef.clear();
    }
  }




}
