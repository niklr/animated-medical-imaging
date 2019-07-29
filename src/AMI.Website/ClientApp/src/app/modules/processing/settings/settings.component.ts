import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ObjectService } from '../../../services/object.service';
import { ObjectStore } from '../../../stores/object.store';
import { AxisType, ProcessObjectCommand } from '../../../clients/ami-api-client';

class AxisTypeContainer {
  displayName: string;
  enum: AxisType;
  checked: boolean;
  public constructor(init?: Partial<AxisTypeContainer>) {
    Object.assign(this, init);
  }
}

@Component({
  selector: 'app-processing-settings',
  templateUrl: './settings.component.html'
})
export class SettingsComponent implements OnInit, AfterViewInit {

  public settings: ProcessObjectCommand;

  public axisTypeContainers: AxisTypeContainer[] = [
    new AxisTypeContainer({ displayName: 'X-Axis', enum: AxisType.X, checked: true }),
    new AxisTypeContainer({ displayName: 'Y-Axis', enum: AxisType.Y, checked: true }),
    new AxisTypeContainer({ displayName: 'Z-Axis', enum: AxisType.Z, checked: true })
  ];

  constructor(public objectStore: ObjectStore, public objectService: ObjectService) {
    this.settings = this.objectStore.settings;
  }

  ngOnInit() {
    this.setAxisTypes();
  }

  ngAfterViewInit(): void {
    M.updateTextFields();
  }

  public showProcessButton(): boolean {
    return this.objectStore.count > 0;
  }

  public setAxisTypes(): void {
    this.settings.axisTypes = [];
    for (const axisTypeContainer of this.axisTypeContainers) {
      if (axisTypeContainer.checked) {
        this.settings.axisTypes.push(axisTypeContainer.enum);
      }
    }
  }

  public toggleAxisTypeContainer(axisTypeContainer: AxisTypeContainer, $event) {
    axisTypeContainer.checked = !axisTypeContainer.checked;
    this.setAxisTypes();
  }
}
