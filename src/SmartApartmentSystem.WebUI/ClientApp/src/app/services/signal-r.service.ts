import { Injectable, EventEmitter } from '@angular/core';
import * as signalR from "@microsoft/signalr";

@Injectable({
    providedIn: 'root'
})
export class SignalRService {
    private hubConnection: signalR.HubConnection
    onChanged = new EventEmitter<boolean>();

    public startConnection = () => {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl('/signalr')
            .build();

        this.hubConnection
            .start()
            .then(() => {
                console.log('Connection started');
            })
            .catch(err => {
                console.log('Error while starting connection: ' + err);
            })

        this.hubConnection.onclose = (exc) => { console.log(exc); };
    }

    public addTransferChartDataListener = () => {
        this.hubConnection.on('statusesChanged', (data) => {
            console.log(data);
            this.onChanged.emit();
        });
    }
}