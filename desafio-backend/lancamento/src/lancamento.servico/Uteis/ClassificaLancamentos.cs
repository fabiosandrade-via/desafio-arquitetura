using lancamento.dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lancamento.servico.Uteis
{
    internal class ClassificaLancamentos
    {
        private readonly List<LancamentoEntity> _lancamentos;
        public ClassificaLancamentos(List<LancamentoEntity> lancamentos)
        {
            _lancamentos = lancamentos;
        }
        public Task<List<LancamentoGrupoEntity>> RetornaLancamentosAgrupados()
        {
            List<LancamentoGrupoEntity> lancamentosAgrupados = new();

            var datasUnicasOrdenadas = _lancamentos
                .Where(l => l.DataLancamento != null)
                .Select(l => l.DataLancamento!.Value)
                .Distinct()
                .OrderBy(data => data)
                .ToList();

            foreach (var data in datasUnicasOrdenadas)
            {
                LancamentoGrupoEntity lancamentoGrupo = new LancamentoGrupoEntity()
                {
                    Id = data,
                    Lancamentos = _lancamentos.Where(l => l.DataLancamento == data).ToList()
                };
                lancamentosAgrupados.Add(lancamentoGrupo);
            }

            return Task.FromResult(lancamentosAgrupados);
        }
    }
}
