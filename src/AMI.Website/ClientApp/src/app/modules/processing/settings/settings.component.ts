import { Component, OnInit, AfterViewInit } from '@angular/core';
import { AxisType, ProcessObjectCommand } from '../../../clients/ami-api-client';
import { ObjectStore } from '../../../stores/object.store';

@Component({
  selector: 'app-processing-settings',
  templateUrl: './settings.component.html'
})
export class SettingsComponent implements OnInit, AfterViewInit {

  settings: ProcessObjectCommand;

  axisTypeContainers: AxisTypeContainer[] = [
    new AxisTypeContainer({ displayName: "X-Axis", enum: AxisType.X, checked: true }),
    new AxisTypeContainer({ displayName: "Y-Axis", enum: AxisType.Y, checked: true }),
    new AxisTypeContainer({ displayName: "Z-Axis", enum: AxisType.Z, checked: true })
  ];

  constructor(public objectStore: ObjectStore) {
    this.settings = this.objectStore.settings;
  }

  ngOnInit() {
    this.setAxisTypes();
  }

  ngAfterViewInit(): void {
    M.updateTextFields();
  }

  public setAxisTypes(): void {
    this.settings.axisTypes = [];
    for (var i = 0; i < this.axisTypeContainers.length; i++) {
      var axisTypeContainer = this.axisTypeContainers[i];
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

class AxisTypeContainer {
  displayName: string;
  enum: AxisType;
  checked: boolean;
  public constructor(init?: Partial<AxisTypeContainer>) {
    Object.assign(this, init);
  }
}
