import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserComponent } from './users/user.component';
import { RoleComponent } from './roles/role.component';

import { AuthGuard } from 'src/app/shared/auth.guard';
const routes: Routes = [
  {
    path: '',
    redirectTo: 'users',
    pathMatch: 'full'
  },
  {
    path: 'users',
    component: UserComponent,
    data: {
      title: 'Người dùng',
      requiredPolicy: 'Permissions.Users.View',
    },
    canActivate: [AuthGuard],
  },
  {
    path: 'roles',
    component: RoleComponent,
    data: {
      title: 'Quyền',
      requiredPolicy: 'Permissions.Roles.View',
    },
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SystemRoutingModule {}
