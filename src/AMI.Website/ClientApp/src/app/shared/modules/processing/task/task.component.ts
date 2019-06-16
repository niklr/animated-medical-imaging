import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { GuidUtil, MomentUtil } from '../../../../utils';
import { TaskStatus } from '../../../../clients/ami-api-client';

@Component({
  selector: 'app-processing-task',
  templateUrl: './task.component.html'
})
export class TaskComponent implements OnInit, AfterViewInit {

  @Input() task: any;

  constructor(private guidUtil: GuidUtil, public momentUtil: MomentUtil) {

  }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    this.initMaterialize();
  }

  private initMaterialize(): void {

  }

  public displayTaskStatus(status: TaskStatus): string {
    return TaskStatus[status];
  }
}
