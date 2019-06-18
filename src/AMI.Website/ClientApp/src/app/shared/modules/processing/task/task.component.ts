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
  }

  private initMaterialize(): void {

  }

  public showSpinner(): boolean {
    return this.task.status === TaskStatus.Queued || this.task.status === TaskStatus.Processing;
  }

  public displayTaskStatus(status: TaskStatus): string {
    return TaskStatus[status];
  }
}
