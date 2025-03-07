import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms'; 
import { LancamentoService, Lancamento } from './services/lancamento.service';
import { Validators } from '@angular/forms';

 @Component({
   selector: 'app-lancamento',
   standalone: true,
   imports: [CommonModule, ReactiveFormsModule], 
   templateUrl: './lancamento.component.html',
   styleUrls: ['./lancamento.component.css']
 })

export class LancamentoComponent {
  lancamentoForm: FormGroup;
  lancamentos: Lancamento[] = [];

  constructor(
    private fb: FormBuilder,
    private lancamentoService: LancamentoService
  ) {

    this.lancamentoForm = this.fb.group({
      tipo: ['', Validators.required],
      valor: ['', [Validators.required, Validators.min(0.01)]], 
      descricao: ['', Validators.required],
      data: ['', Validators.required] 
    });
  }

  adicionarLancamento() {
    if (this.lancamentoForm.invalid) {
      alert('Preencha todos os campos antes de adicionar!');
      return;
    }

    const novoLancamento = this.lancamentoForm.value;
    this.lancamentos.push(novoLancamento);
    this.lancamentoForm.reset();
  }

  salvarLancamentos() {   
    this.lancamentoService.salvarLancamentos(this.lancamentos).subscribe({
      next: (response) => {
        console.log('Lançamentos salvos com sucesso!', response);
        alert('Lançamentos enviados para processamento de fluxo de caixa com sucesso!');
        this.lancamentos = []; 
      },
      error: (err) => {
        console.error('Erro ao salvar lançamentos:', err);
        alert('Erro ao enviar os lançamentos. Tente novamente.');
      }
    });
  }
}
