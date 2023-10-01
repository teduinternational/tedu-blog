import { Component, OnInit, EventEmitter, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { AdminApiRoleApiClient, PermissionDto, RoleClaimsDto } from 'src/app/api/admin-api.service.generated';

@Component({
    templateUrl: 'permission-grant.component.html',
})
export class PermissionGrantComponent implements OnInit, OnDestroy {
    private ngUnsubscribe = new Subject<void>();

    // Default
    public blockedPanelDetail: boolean = false;
    public form: FormGroup;
    public title: string;
    public btnDisabled = false;
    public saveBtnName: string;
    public closeBtnName: string;
    public permissions: RoleClaimsDto[] = [];
    public selectedPermissions: RoleClaimsDto[] = [];
    public id: string;
    formSavedEventEmitter: EventEmitter<any> = new EventEmitter();

    constructor(
        public ref: DynamicDialogRef,
        public config: DynamicDialogConfig,
        private roleService: AdminApiRoleApiClient,
        private fb: FormBuilder
    ) { }

    ngOnDestroy(): void {
        if (this.ref) {
            this.ref.close();
        }
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    ngOnInit() {
        this.buildForm();
        this.loadDetail(this.config.data.id);
        this.saveBtnName = 'Cập nhật';
        this.closeBtnName = 'Hủy';
    }

    loadDetail(roleId: string) {
        this.toggleBlockUI(true);
        this.roleService
            .getAllRolePermissions(roleId)
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe({
                next: (response: PermissionDto) => {
                    this.permissions = response.roleClaims;
                    this.buildForm();
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
        var roleClaims: RoleClaimsDto[] = [];
        for (let index = 0; index < this.permissions.length; index++) {
            const isGranted = this.selectedPermissions.filter((x) => x.value == this.permissions[index].value).length > 0;

            roleClaims.push(new RoleClaimsDto({
                type: this.permissions[index].type,
                selected: isGranted,
                value: this.permissions[index].value
            }));
        }
        var updateValues = new PermissionDto({
            roleId: this.config.data.id,
            roleClaims: roleClaims,
        });
        this.roleService
            .savePermission(updateValues)
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe(() => {
                this.toggleBlockUI(false);
                this.ref.close(this.form.value);
            });
    }

    buildForm() {
        this.form = this.fb.group({});
        //Fill value
        for (let index = 0; index < this.permissions.length; index++) {
            const permission = this.permissions[index];
            if (permission.selected) {
                this.selectedPermissions.push(new RoleClaimsDto({
                    selected: true,
                    displayName: permission.displayName,
                    type: permission.type,
                    value: permission.value
                }));
            }
        }
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
