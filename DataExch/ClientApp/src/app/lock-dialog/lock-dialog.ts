
import { Component, Inject, Injectable, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'lock-dialog-wnd',
  templateUrl: './lock-dialog.html'
})
export class LockDialog {

  @Input() message;
  constructor(public activeModal: NgbActiveModal) { }
}
