import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Lancamento {
  tipo: string;
  valor: number;
  descricao: string;
  data: string;
}

@Injectable({
  providedIn: 'root'
})
export class LancamentoService {
  private apiUrl = 'http://localhost:8080/api/lancamentos'; //'http://localhost:5238/api/lancamentos';

  constructor(private http: HttpClient) {}

  salvarLancamentos(lancamentos: Lancamento[]): Observable<any> { 
    return this.http.post(this.apiUrl, JSON.stringify(lancamentos), {
      headers: { 'Content-Type': 'application/json' },
      observe: 'response'
    });
  }  
}
