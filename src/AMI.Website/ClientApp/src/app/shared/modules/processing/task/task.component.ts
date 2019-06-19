import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { MomentUtil } from '../../../../utils';
import { TaskStatus, TaskModel, ProcessObjectAsyncCommand, AxisType } from '../../../../clients/ami-api-client';

@Component({
  selector: 'app-processing-task',
  templateUrl: './task.component.html'
})
export class TaskComponent implements OnInit, AfterViewInit {

  @Input() task: TaskModel;

  public command: ProcessObjectAsyncCommand;
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
        if (this.task.command instanceof ProcessObjectAsyncCommand) {
          this.command = this.task.command;
          if (this.task.command.axisTypes) {
            var tempAxisTypes: string[] = [];
            for (var i = 0; i < this.task.command.axisTypes.length; i++) {
              tempAxisTypes.push(AxisType[this.task.command.axisTypes[i]]);
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
