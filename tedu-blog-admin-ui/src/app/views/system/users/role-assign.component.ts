import { Component, OnInit, EventEmitter, OnDestroy } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { forkJoin, Subject, takeUntil } from 'rxjs';
import { AdminApiRoleApiClient, AdminApiUserApiClient, RoleDto, UserDto } from 'src/app/api/admin-api.service.generated';

@Component({
  templateUrl: 'role-assign.component.html',
})
export class RoleAssignComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();

  // Default
  public blockedPanelDetail: boolean = false;
  public title: string;
  public btnDisabled = false;
  public saveBtnName: string;
  public closeBtnName: string;
  public availableRoles: string[] = [];
  public seletedRoles: string[] = [];
  formSavedEventEmitter: EventEmitter<any> = new EventEmitter();

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private userApiClient: AdminApiUserApiClient,
    private roleApiclient: AdminApiRoleApiClient
  ) { }

  ngOnDestroy(): void {
    if (this.ref) {
      this.ref.close();
    }
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit() {
    var roles = this.roleApiclient.getAllRoles();

    forkJoin({
      roles,
    })
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (repsonse: any) => {
          var roles = repsonse.roles as RoleDto[];
          roles.forEach(element => {
            this.availableRoles.push(element.name);
          });
          this.loadDetail(this.config.data.id);
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
    this.saveBtnName = 'Cập nhật';
    this.closeBtnName = 'Hủy';
  }
  loadRoles() {
    this.toggleBlockUI(true);
    this.roleApiclient
      .getAllRoles()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: RoleDto[]) => {
          response.forEach(element => {
            this.availableRoles.push(element.name);
          });
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }
  loadDetail(id: any) {
    this.toggleBlockUI(true);
    this.userApiClient
      .getUserById(id)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: UserDto) => {
          this.seletedRoles = response.roles;
          this.availableRoles = this.availableRoles.filter(x => !this.seletedRoles.includes(x));
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }
  saveChange() {
    this.toggleBlockUI(true);

    this.saveData();
  }

  private saveData() {
    this.userApiClient
      .assignRolesToUser(this.config.data.id, this.seletedRoles)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
        this.toggleBlockUI(false);
        this.ref.close();
      });
  }

  private toggleBlockUI(enabled: boolean) {
    if (enabled == true) {
      this.btnDisabled = true;
      this.blockedPanelDetail = true;
    } else {
      setTimeout(() => {
        this.btnDisabled = false;
        this.blockedPanelDetail = false;
      }, 1000);
    }
  }
}
