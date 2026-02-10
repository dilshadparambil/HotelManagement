import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageRoomTypesComponent } from './manage-room-types.component';

describe('ManageRoomTypesComponent', () => {
  let component: ManageRoomTypesComponent;
  let fixture: ComponentFixture<ManageRoomTypesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageRoomTypesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageRoomTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
