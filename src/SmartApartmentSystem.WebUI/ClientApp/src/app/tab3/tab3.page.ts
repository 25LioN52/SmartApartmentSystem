import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-tab3',
  templateUrl: 'tab3.page.html',
  styleUrls: ['tab3.page.scss']
})
export class Tab3Page implements OnInit {

    @ViewChild('tempChart', { static: false }) tempChart;
    @ViewChild('floorChart', { static: false }) floorChart;

    tempBars: any;
    floorBars: any;
    colorArray: any;
    labels: string[];
    temperature: number[];
    floor: number[];
    constructor(private http: HttpClient) {
        this.labels = ['12 am', '1 am', '2 am', '3 am', '4 am', '5 am',
            '6 am', '7 am', '8 am', '9 am', '10 am', '11 am',
            '12 pm', '1 pm', '2 pm', '3 pm', '4 pm', '5 pm',
            '6 pm', '7 pm', '8 pm', '9 pm', '10 pm', '11 pm'];
    }

    ngOnInit() {
    }

    ionViewDidEnter() {
        this.http.get('/api/History/Temperature').subscribe((data: number[]) => {
            this.temperature = data;
            this.createTemperatureChart();
        });
        this.http.get('/api/History/Floor').subscribe((data: number[]) => {
            this.floor = data;
            this.createFloorChart();
        });
    }

    createTemperatureChart() {
        this.tempBars = new Chart(this.tempChart.nativeElement, {
            type: 'line',
            data: {
                labels: this.labels.slice(0, this.temperature.length),
                datasets: [{
                    label: 'Temperature in C',
                    data: this.temperature,//[10, 3.8, 5, 6.9, 6.9, 7.5, 10, 17, 10, 5, 5, 15, 16, 3.8, 5, 6.9, 6.9, 7.5, 10, 17, 10, 5, 5, 15],
                    backgroundColor: 'rgba(0, 0, 0, 0)', // array should have same number of elements as number of dataset
                    borderColor: 'rgb(255, 73, 97)',// array should have same number of elements as number of dataset
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: false
                        }
                    }]
                }
            }
        });
    }
    createFloorChart() {
        this.floorBars = new Chart(this.floorChart.nativeElement, {
            type: 'line',
            data: {
                labels: this.labels.slice(0, this.floor.length),
                datasets: [{
                    label: 'Turned On or Off',
                    data: this.floor,//[10, 3.8, 5, 6.9, 6.9, 7.5, 10, 17, 10, 5, 5, 15, 16, 3.8, 5, 6.9, 6.9, 7.5, 10, 17, 10, 5, 5, 15],
                    backgroundColor: 'rgba(0, 0, 0, 0)', // array should have same number of elements as number of dataset
                    borderColor: 'rgb(255, 73, 97)',// array should have same number of elements as number of dataset
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            max: 1,
                            min: 0,
                            stepSize: 1
                        }
                    }]
                }
            }
        });
    }

}
