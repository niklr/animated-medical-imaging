import { Component, OnInit, AfterViewInit } from '@angular/core';
import { AxisType, ProcessObjectCommand } from '../../../../clients/ami-api-client';

import M from 'materialize-css';

@Component({
  selector: 'app-process-object-settings',
  templateUrl: './process-settings.component.html'
})
export class ProcessSettingsComponent implements OnInit, AfterViewInit {

  settings: ProcessObjectCommand = new ProcessObjectCommand(); 

  axisTypeContainers: AxisTypeContainer[] = [
    new AxisTypeContainer({ displayName: "X-Axis", enum: AxisType.X, checked: true }),
    new AxisTypeContainer({ displayName: "Y-Axis", enum: AxisType.Y, checked: true }),
    new AxisTypeContainer({ displayName: "Z-Axis", enum: AxisType.Z, checked: true })
  ];

  constructor() {
    this.settings.desiredSize = 250;
    this.settings.amountPerAxis = 10;
    this.settings.grayscale = true;
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    M.updateTextFields();
  }

  toggleAxisTypeContainer(axisTypeContainer: AxisTypeContainer, $event) {
    axisTypeContainer.checked = !axisTypeContainer.checked;
    this.settings.axisTypes = [];
    for (var i = 0; i < this.axisTypeContainers.length; i++) {
      var axisTypeContainer = this.axisTypeContainers[i];
      if (axisTypeContainer.checked) {
        this.settings.axisTypes.push(axisTypeContainer.enum);
      }
    }
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
