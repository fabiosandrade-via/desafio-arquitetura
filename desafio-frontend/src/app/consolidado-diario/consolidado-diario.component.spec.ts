import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsolidadoComponent } from './consolidado-diario.component';

describe('ConsolidadoDiarioComponent', () => {
  let component: ConsolidadoComponent;
  let fixture: ComponentFixture<ConsolidadoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConsolidadoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConsolidadoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
