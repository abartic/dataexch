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

  public stockinfos: StockInfo[];

  

  constructor(private msgService: commSignalRService, private modalService: NgbModal, http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.messageHistory = [];
    http.get<StockInfo[]>(baseUrl + 'stockinfo').subscribe(result => {
      this.stockinfos = result;
    }, error => console.error(error));
  }

  ngOnInit() {
    this.msgService.onMessageReceived.subscribe((message) => {
      //this.messageHistory.push(message);
      const modalRef = this.modalService.open(LockDialog);
      modalRef.componentInstance.message = this.message;
    });
  }

  reqLock() {
    this.message = 'Price changed!';
    this.msgService.sendMessage(this.message);
  }

}


interface StockInfo {
  date: string;
  openPrice: number;
  closePrice: number;
  summary: string;
}
