import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class BroadcastService {
    public httpError: BehaviorSubject<boolean>;

    constructor() {
        //initialize it to false
        this.httpError = new BehaviorSubject<boolean>(false);
    }
}