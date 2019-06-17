import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { MomentUtil } from '../../../../utils';
import { TaskStatus, TaskModel, ProcessObjectAsyncCommand } from '../../../../clients/ami-api-client';

@Component({
  selector: 'app-processing-task',
  templateUrl: './task.component.html'
})
export class TaskComponent implements OnInit, AfterViewInit {

  @Input() task: TaskModel;

  constructor(public momentUtil: MomentUtil) {
  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    this.initMaterialize();
    console.log(this.task);
    let asdf = this.task.command as ProcessObjectAsyncCommand;
    console.log(asdf);
  }

  private initMaterialize(): void {

  }

  public displayTaskStatus(status: TaskStatus): string {
    return TaskStatus[status];
  }
}
