import { Component, OnDestroy, OnInit } from '@angular/core';
import { DialogService } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import {
  AdminApiRoyaltyApiClient,
  TransactionDto,
  TransactionDtoPagedResult,
} from '../../../api/admin-api.service.generated';

@Component({
  selector: 'app-transaction',
  templateUrl: './transactions.component.html',
})
export class TransactionComponent implements OnInit, OnDestroy {
  //System variables
  private ngUnsubscribe = new Subject<void>();
  public blockedPanel: boolean = false;

  //Paging variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number;

  public items: TransactionDto[] | undefined = [];
  public userName: string = '';
  public fromMonth: number = 1;
  public fromYear: number = new Date().getFullYear();
  public toMonth: number = 12;
  public toYear: number = new Date().getFullYear();
  constructor(
    private RoyaltyApiClient: AdminApiRoyaltyApiClient,
    public dialogService: DialogService
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit() {
    this.loadData();
  }
  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.pageSize = event.rows;
    this.loadData();
  }

  loadData() {
    this.toggleBlockUI(true);

    this.RoyaltyApiClient.getTransactionHistory(
      this.userName,
      this.fromMonth,
      this.fromYear,
      this.toMonth,
      this.toYear,
      this.pageIndex,
      this.pageSize
    )
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: TransactionDtoPagedResult) => {
          this.items = response.results;
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  private toggleBlockUI(enabled: boolean) {
    if (enabled == true) {
      this.blockedPanel = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
      }, 1000);
    }
  }
}
