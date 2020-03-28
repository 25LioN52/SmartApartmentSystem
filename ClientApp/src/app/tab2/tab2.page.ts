import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ModuleStatus } from './moduleStatus';

@Component({
    selector: 'app-tab2',
    templateUrl: 'tab2.page.html',
    styleUrls: ['tab2.page.scss']
})
export class Tab2Page implements OnInit {
    expectedTemperature: number;
    actualTemperature: number;
    IsActive: boolean;
    isTrue: true;
    constructor(private http: HttpClient) {

    }
    ngOnInit() {
        this.http.get('/api/Boiler').subscribe((data: ModuleStatus) => {
            this.expectedTemperature = data.expectedStatus;
            this.actualTemperature = data.actualStatus;
            this.IsActive = data.isActive;
        });
    }
    change() {
        this.http.post(location.origin + '/api/Boiler/' + this.expectedTemperature, {}).subscribe(error => {console.log(error);});
    }
}
