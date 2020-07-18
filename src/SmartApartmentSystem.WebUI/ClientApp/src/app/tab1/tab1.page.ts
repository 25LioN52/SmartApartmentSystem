import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ModuleStatus } from './moduleStatus';

@Component({
  selector: 'app-tab1',
  templateUrl: 'tab1.page.html',
  styleUrls: ['tab1.page.scss']
})
export class Tab1Page implements OnInit {
    expectedTemperature: number;
    actualTemperature: number;
    IsActive: boolean;
    isTrue: true;
    IsWaterOn: boolean;
    IsFloorOn: boolean;
    constructor(private http: HttpClient) {

    }
    ngOnInit() {
        this.http.get('/api/Boiler').subscribe((data: ModuleStatus) => {
            this.expectedTemperature = data.expectedStatus;
            this.actualTemperature = data.actualStatus;
            this.IsActive = data.isActive;
        });
        this.http.get('/api/Floor').subscribe((data: ModuleStatus) => {
            this.IsFloorOn = data.isActive;
        });
        this.IsWaterOn = true;
    }
    tempChange() {
        this.http.post(location.origin + '/api/Boiler/' + this.expectedTemperature, {}).subscribe(error => { console.log(error); });
    }
    waterChange() {
        this.http.post(location.origin + '/api/Water/' + this.IsWaterOn, {}).subscribe(error => { console.log(error); });
    }
    floorChange() {
        this.http.post(location.origin + '/api/Floor/' + this.IsFloorOn, {}).subscribe(error => { console.log(error); });
    }
}
