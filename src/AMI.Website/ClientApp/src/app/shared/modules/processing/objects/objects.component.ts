import { Component, OnInit, AfterViewInit, Input } from '@angular/core';

import M from 'materialize-css';

@Component({
  selector: 'app-processing-objects',
  templateUrl: './objects.component.html'
})
export class ObjectsComponent implements OnInit, AfterViewInit {

  @Input() tableId: string;

  private isChecked: boolean = false;
  public objects = [];

  constructor() {
    this.initDemoObjects();
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    this.initDropdown();
  }

  private initDropdown(): void {
    var options = {};
    var elem = document.querySelector('#objectActionsDropdownButton');
    var instance = M.Dropdown.init(elem, options);
  }

  private initDemoObjects(): void {
    var object1 = {
      id: '19f06aa9-1856-4cc9-ba30-1ea0483fc154',
      originalFilename: 'SMIR.Brain.XX.O.MR_Flair.36620.mha',
      modifiedDate: '2019-05-21T14:05:25.3100000Z',
      latestTask: {
        id: '87619062-ec17-4e4a-ab08-4f87aeea4249',
        createdDate: '2019-05-21T14:05:26.1100000Z',
        modifiedDate: '2019-05-21T14:05:27.1900000Z',
        status: 'Queued',
        position: 12
      }
    };

    var object2 = {
      id: 'a8bfea94-614b-466c-9400-3ace2d5e4f06',
      originalFilename: 'SMIR.Brain.XX.O.CT.346124.nii',
      modifiedDate: '2019-04-17T07:52:41.4700000Z'
    };

    this.objects = [object1, object2];
  }

  private toggleCheckbox(): void {
    this.isChecked = !this.isChecked;
    if (this.objects) {
      this.objects.forEach(function (value, index, array) {
        let that = this as ObjectsComponent;
        value.isChecked = that.isChecked;
      }.bind(this));
    }
  }
}

