import { EventEmitter, Injectable } from '@angular/core'
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr'

@Injectable()
export class commSignalRService {

  hubConnection: HubConnection;
  onMessageReceived = new EventEmitter<any>();

  constructor() {
    this.createConnection();
    this.registerEvents();
    this.startConnection();
  }

  sendMessage(message) {
    this.hubConnection.invoke('sendMessage', message);
  }

  private createConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl("/chat")
      .build();
  }

  private startConnection() {
    this.hubConnection.start().then(() => {
      console.log('Connection started');
    }).catch(err => {
      console.error(err);
      //setTimeout(this.startConnection(), 5000);
    });
  }

  private registerEvents() {
    this.hubConnection.on('receivemessage', (message: any) => {
      console.log('message received:' + message);
      this.onMessageReceived.emit(message);
    })
  }
}
