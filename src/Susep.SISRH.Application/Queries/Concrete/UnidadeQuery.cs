﻿using Dapper;
using Microsoft.Extensions.Configuration;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.Queries.RawSql;
using Susep.SISRH.Application.ViewModels;
using Susep.SISRH.Domain.Enums;
using SUSEP.Framework.Result.Abstractions;
using SUSEP.Framework.Result.Concrete;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Queries.Concrete
{
    public class UnidadeQuery : IUnidadeQuery
    {
        private readonly IConfiguration Configuration;

        public UnidadeQuery(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<IApplicationResult<IEnumerable<UnidadeViewModel>>> ObterAtivasAsync()
        {
            var result = new ApplicationResult<IEnumerable<UnidadeViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@situacaoAtiva", (int)SituacaoUnidadeEnum.Ativa, DbType.Int64, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<UnidadeViewModel>(UnidadeRawSqls.ObterAtivas, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterAtivasDadosComboAsync()
        {
            var result = new ApplicationResult<IEnumerable<DadosComboViewModel>>();

            //Obtém as unidades
            var dados = await ObterAtivasAsync();

            //Converte de UnidadeViewModel para DadosComboViewModel
            result.Result = dados.Result.Select(u => new DadosComboViewModel() { Id = u.UnidadeId.ToString(), Descricao = u.SiglaCompleta });

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<UnidadeViewModel>>> ObterComPlanoTrabalhoAsync()
        {
            var result = new ApplicationResult<IEnumerable<UnidadeViewModel>>();

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<UnidadeViewModel>(UnidadeRawSqls.ObterComPlanoTrabalho);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterSemCatalogoCadastradoComboAsync()
        {
            var result = new ApplicationResult<IEnumerable<DadosComboViewModel>>();

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<DadosComboViewModel>(UnidadeRawSqls.ObterSemCatalogoCadastrado);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }


        public async Task<IApplicationResult<IEnumerable<UnidadeViewModel>>> ObterComCatalogoCadastradoComboAsync()
        {
            var result = new ApplicationResult<IEnumerable<UnidadeViewModel>>();

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<UnidadeViewModel>(UnidadeRawSqls.ObterComCatalogoCadastrado);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<UnidadeViewModel>> ObterPorChaveAsync(Int64 unidadeId)
        {
            var result = new ApplicationResult<UnidadeViewModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@unidadeId", unidadeId, DbType.Int64, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryFirstOrDefaultAsync<UnidadeViewModel>(UnidadeRawSqls.ObterPorChave, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PessoaViewModel>>> ObterServidoresDisponiveisPGDPorChaveAsync(Int64 unidadeId)
        {
            var result = new ApplicationResult<IEnumerable<PessoaViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@unidadeId", unidadeId, DbType.Int64, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<PessoaViewModel>(UnidadeRawSqls.ObterPessoasDiretamenteAlocadasPorUnidade, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<DateTime>>> ObterFeriadosPorUnidadeAsync(Int64 unidadeId, DateTime dataInicio, DateTime dataFim)
        {
            var result = new ApplicationResult<IEnumerable<DateTime>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@unidadeId", unidadeId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@dataInicio", dataInicio, DbType.Date, ParameterDirection.Input);
            parameters.Add("@dataFim", dataFim, DbType.Date, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var feriados = await connection.QueryAsync<FeriadoViewModel>(UnidadeRawSqls.ObterFeriados, parameters);
                result.Result = feriados.Select(f => f.Data);

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterModalidadesExecucaoAsync(Int64 unidadeId)
        {
            var result = new ApplicationResult<IEnumerable<DadosComboViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@unidadeId", unidadeId, DbType.Int64, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<DadosComboViewModel>(UnidadeRawSqls.ObterModalidadesExecucaoPorUnidade, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterPessoasDadosComboAsync(Int64 unidadeId)
        {
            var result = new ApplicationResult<IEnumerable<DadosComboViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@unidadeId", unidadeId, DbType.Int64, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = (await connection.QueryAsync<PessoaViewModel>(UnidadeRawSqls.ObterPessoasDiretamenteAlocadasPorUnidade, parameters)).OrderBy(a => a.Nome);
                result.Result = dados.Select(it => new DadosComboViewModel() { Id = it.PessoaId.ToString(), Descricao = it.Nome });

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<PessoaViewModel>>> ObterPessoasAsync(Int64 unidadeId)
        {
            var result = new ApplicationResult<IEnumerable<PessoaViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@unidadeId", unidadeId, DbType.Int64, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<PessoaViewModel>(UnidadeRawSqls.ObterPessoasPorUnidade, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }
        public async Task<IApplicationResult<IEnumerable<PessoaViewModel>>> ObterChefesAsync(String siglaCompletaunidade)
        {
            var result = new ApplicationResult<IEnumerable<PessoaViewModel>>();

            var siglas = new List<string>();
            string parteAnterior = string.Empty;
            var partesSigla = siglaCompletaunidade.Split('/');
            foreach (var parteSigla in partesSigla)
            {
                parteAnterior = (String.IsNullOrEmpty(parteAnterior) ? "" : parteAnterior + "/") + parteSigla;
                siglas.Add(parteAnterior);
            }

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<PessoaViewModel>(UnidadeRawSqls.ObterChefesPorUnidade, new { siglas = siglas.ToArray() });
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterSubordinadasAsync(Int64 unidadeId)
        {
            var result = new ApplicationResult<IEnumerable<DadosComboViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@unidadeId", unidadeId, DbType.Int64, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<DadosComboViewModel>(UnidadeRawSqls.ObterSubordinadasPorUnidade, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<UnidadeViewModel>>> ObterPorChefeAsync(long pessoaId)
        {
            var result = new ApplicationResult<IEnumerable<UnidadeViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pessoaId", pessoaId, DbType.Int64, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var dados = await connection.QueryAsync<UnidadeViewModel>(UnidadeRawSqls.ObterPorChefe, parameters);
                result.Result = dados;

                connection.Close();
            }

            return result;
        }

        public async Task<IApplicationResult<IEnumerable<UnidadeViewModel>>> ObterEstruturaAtualAsync()
        {
            var result = new ApplicationResult<IEnumerable<UnidadeViewModel>>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@situacaoUnidadeAtiva", (int)SituacaoUnidadeEnum.Ativa, DbType.Int64, ParameterDirection.Input);

            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                
                using (var multi = await connection.QueryMultipleAsync(UnidadeRawSqls.EstruturaAtual, parameters))
                {
                    var unidades = multi.Read<UnidadeViewModel>().ToList();
                    var pessoas = multi.Read<PessoaViewModel>().ToList();

                    foreach (var unidade in unidades)
                        unidade.Pessoas = pessoas.Where(it => it.UnidadeId == unidade.UnidadeId);

                    result.Result = unidades;
                }
                
                connection.Close();
            }

            return result;
        }
    }
}
