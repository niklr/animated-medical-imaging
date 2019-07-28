import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { MomentUtil } from '../../../utils';
import { TaskStatus, TaskModel, ProcessObjectCommand, AxisType } from '../../../clients/ami-api-client';

@Component({
  selector: 'app-processing-task',
  templateUrl: './task.component.html'
})
export class TaskComponent implements OnInit, AfterViewInit {

  @Input() task: TaskModel;

  public command: ProcessObjectCommand;
  public displayAxisTypes: string;

  constructor(public momentUtil: MomentUtil) {
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    this.initCommand();
    this.initMaterialize();
  }

  private initCommand(): void {
    setTimeout(() => {
      if (this.task) {
        if (this.task.command instanceof ProcessObjectCommand) {
          this.command = this.task.command;
          if (this.task.command.axisTypes) {
            const tempAxisTypes: string[] = [];
            for (const axisType of this.task.command.axisTypes) {
              tempAxisTypes.push(AxisType[axisType]);
            }
            this.displayAxisTypes = tempAxisTypes.join(', ');
          }
        }
      }
    });
  }

  private initMaterialize(): void {
  }

  public showSpinner(): boolean {
    if (this.task.status) {
      switch (this.task.status) {
        case TaskStatus.Queued:
        case TaskStatus.Processing:
          return true;
        default:
          return false;
      }
    }
    return false;
  }

  public displayTaskStatus(): string {
    if (this.task.status) {
      return TaskStatus[this.task.status];
    } else {
      return TaskStatus[TaskStatus.Created];
    }
  }
}
