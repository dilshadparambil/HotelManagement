import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageHotelsComponent } from './manage-hotels.component';

describe('ManageHotelsComponent', () => {
  let component: ManageHotelsComponent;
  let fixture: ComponentFixture<ManageHotelsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageHotelsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageHotelsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
