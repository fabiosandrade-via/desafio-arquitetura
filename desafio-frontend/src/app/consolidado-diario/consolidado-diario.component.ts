import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms'; 
import { ConsolidadoService } from './services/consolidado.service';

@Component({
  selector: 'app-consolidado-diario',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule], 
  templateUrl: './consolidado-diario.component.html',
  styleUrls: ['./consolidado-diario.component.css']
})

export class ConsolidadoComponent {
  consolidadoForm: FormGroup;
  lancamentos: any[] = [];
  acumulado: number = 0;
  mensagem: string = '';  

  constructor(
    private fb: FormBuilder,
    private consolidadoService: ConsolidadoService
  ) {
    this.consolidadoForm = this.fb.group({
      data: [null, Validators.required] 
    });
  }

  exibirTipoFormatado(tipo: string): string {
    return tipo === 'credito' ? 'Crédito' : tipo === 'debito' ? 'Débito' : tipo;
  }

  consultarConsolidado() {  
    const dataSelecionada: string = this.consolidadoForm.value.data;

    if (!dataSelecionada) {
      alert('Favor selecionar uma data.');
      return;
    }

    const [ano, mes, dia] = dataSelecionada.split('-');
    const dataFormatada = `${dia}/${mes}/${ano}`;

    console.log('Data formatada para envio:', dataFormatada);

    this.consolidadoService.consultarConsolidado(dataFormatada).subscribe({
      next: (response) => {
        console.log('Consolidado diário encontrado com sucesso!', response);
        this.lancamentos = response.body.lancamentos;
        this.acumulado = response.body.acumulado;
        this.mensagem = response.body.mensagem;        
      },
      error: (err) => {
        console.error('Erro ao consultar consolidado diário:', err);
      
        const errorMessage = err.error?.message || err.statusText || 'Erro desconhecido';
        alert(`Erro ao consultar consolidado diário: ${errorMessage}`);
      }
           
    });
  }  
}
