import { Component, OnInit, Inject } from '@angular/core';
import { commSignalRService } from '../services/comm.signalr.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LockDialog } from '../lock-dialog/lock-dialog';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  message: any;
  messageHistory: any[];

  public forecasts: WeatherForecast[];

  

  constructor(private chatService: commSignalRService, private modalService: NgbModal, http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.messageHistory = [];
    http.get<WeatherForecast[]>(baseUrl + 'weatherforecast').subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }

  ngOnInit() {
    this.chatService.onMessageReceived.subscribe((message) => {
      this.messageHistory.push(message);
    });
  }

  reqLock() {
    this.chatService.sendMessage(this.message);
    this.message = 'lock req';

    const modalRef = this.modalService.open(LockDialog);
    modalRef.componentInstance.message = this.message;
  }

}


interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
