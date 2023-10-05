import { Directive, ElementRef, Input, OnInit } from '@angular/core';
import { TokenStorageService } from './../services/token-storage.service';
@Directive({
    selector: '[appPermission]'
})
export class PermissionDirective implements OnInit {
    @Input() appPolicy: string;

    constructor(private el: ElementRef, private tokenService: TokenStorageService) {

    }
    ngOnInit() {
        var loggedInUser = this.tokenService.getUser();
        if (loggedInUser) {
            var listPermission = loggedInUser.permissions;
            if (listPermission != null && listPermission != ''
                && listPermission.filter(x => x == this.appPolicy).length > 0) {
                this.el.nativeElement.style.display = "";
            } else {
                this.el.nativeElement.style.display = "none";
            }
        }
        else {
            this.el.nativeElement.style.display = "none";
        }
    }
}