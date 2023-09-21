import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

import { IconSetService } from '@coreui/icons-angular';
import { iconSubset } from './icons/icon-subset';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  template: `
  <p-toast position="top-center"></p-toast>
  <p-confirmDialog
  header="Xác nhận"
  acceptLabel="Có"
  rejectLabel="Không"
  icon="pi pi-exclamation-triangle"
></p-confirmDialog>
  <router-outlet></router-outlet>`,
})
export class AppComponent implements OnInit {
  title = 'Tedu Blog Admin UI';

  constructor(
    private router: Router,
    private titleService: Title,
    private iconSetService: IconSetService
  ) {
    titleService.setTitle(this.title);
    // iconSet singleton
    iconSetService.icons = { ...iconSubset };
  }

  ngOnInit(): void {
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
    });
  }
}
