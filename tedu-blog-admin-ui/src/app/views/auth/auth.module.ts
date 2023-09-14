import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthRoutingModule } from './auth-routing.module';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { Page404Component } from './page404/page404.component';
import { Page500Component } from './page500/page500.component';
import { ButtonModule, CardModule, FormModule, GridModule } from '@coreui/angular';
import { IconModule } from '@coreui/icons-angular';


@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    Page404Component,
    Page500Component
  ],
  imports: [
    CommonModule,
    AuthRoutingModule,
    CardModule,
    ButtonModule,
    GridModule,
    IconModule,
    FormModule
  ]
})
export class AuthModule {
}
