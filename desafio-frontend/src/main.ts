import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { LancamentoComponent } from './app/lancamento/lancamento.component';
import { ConsolidadoComponent } from './app/consolidado-diario/consolidado-diario.component';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter([
      { path: 'lancamento', component: LancamentoComponent },
      { path: 'consolidado-diario', component: ConsolidadoComponent }
    ]),
    provideHttpClient()
  ]
}).catch(err => console.error(err));

