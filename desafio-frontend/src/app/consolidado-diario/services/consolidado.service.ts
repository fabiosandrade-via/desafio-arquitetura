import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConsolidadoService {
  private apiUrl = 'http://localhost:8081/api/lancamentos/consolidado' //'http://localhost:5244/api/lancamentos/consolidado';

  constructor(private http: HttpClient) {}

  consultarConsolidado(data: string): Observable<any> {  
    const params = new HttpParams().set('DataConsolidado', data);

    return this.http.get(this.apiUrl, { 
      headers: { 'Content-Type': 'application/json' },
      params: params,
      observe: 'response'
    });
  }  
}
