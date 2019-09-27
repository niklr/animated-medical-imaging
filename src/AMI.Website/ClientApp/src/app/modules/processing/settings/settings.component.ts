import { Component, OnInit, AfterViewInit } from '@angular/core';
import { EnumContainer } from '../../../models/enum-container.model';
import { ObjectService } from '../../../services/object.service';
import { ObjectStore } from '../../../stores/object.store';
import { AxisType, ProcessObjectCommand } from '../../../clients/ami-api-client';

@Component({
  selector: 'app-processing-settings',
  templateUrl: './settings.component.html'
})
export class SettingsComponent implements OnInit, AfterViewInit {

  public settings: ProcessObjectCommand;

  public axisTypeContainers: EnumContainer[] = [
    new EnumContainer({ displayName: 'X-Axis', enum: AxisType.X, checked: true }),
    new EnumContainer({ displayName: 'Y-Axis', enum: AxisType.Y, checked: true }),
    new EnumContainer({ displayName: 'Z-Axis', enum: AxisType.Z, checked: true })
  ];

  constructor(public objectStore: ObjectStore, public objectService: ObjectService) {
    this.settings = this.objectStore.settings;
  }

  ngOnInit() {
    this.setAxisTypes();
  }

  ngAfterViewInit(): void {
    this.initMaterialize();
  }

  private initMaterialize(): void {
    setTimeout(() => {
      M.updateTextFields();
      M.Range.init(document.querySelector('#delay'), {});
    });
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

  public toggleAxisTypeContainer(axisTypeContainer: EnumContainer, $event) {
    axisTypeContainer.checked = !axisTypeContainer.checked;
    this.setAxisTypes();
  }
}
