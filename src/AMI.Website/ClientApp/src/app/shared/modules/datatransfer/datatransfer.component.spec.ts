import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatatransferComponent } from './datatransfer.component';

describe('DatatransferComponent', () => {
  let component: DatatransferComponent;
  let fixture: ComponentFixture<DatatransferComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatatransferComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatatransferComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
