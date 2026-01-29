using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using Microsoft.VisualBasic.FileIO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using ApiFinanceira.Data.DTO.Exportacao;

namespace ApiFinanceira.Services
{
    public enum FormatoExportacao { xlsx, csv }
    
    public class CsvColunaConfiguracao<T>
    {
        public string Cabecalho { get; set; }
        public Func<T, string> Formatador { get; set; }
    }

    public class ExportacaoService
    {
        public ResultadoExportacao ExportarPlanilha<T>(
            IEnumerable<T> dados, 
            List<CsvColunaConfiguracao<T>> config, 
            FormatoExportacao formatoArquivo = FormatoExportacao.xlsx) where T : class
        {
            var nomeBaseArquivo = $"exportacao_{DateTime.UtcNow:yyyyMMdd_HHmmss}";
            
            var arquivosCsv = GerarCsvEmChunks(dados, config);

            if (formatoArquivo == FormatoExportacao.xlsx)
            {
                var arquivosXlsx = arquivosCsv.Select(csv => ConverterCsvParaXlsx(csv)).ToList();

                if (arquivosXlsx.Count == 1)
                {
                    return new ResultadoExportacao
                    {
                        Conteudo = arquivosXlsx[0],
                        NomeArquivo = $"{nomeBaseArquivo}.xlsx",
                        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    };
                }

                var zipXlsx = GerarZip(arquivosXlsx, nomeBaseArquivo, "xlsx");

                return new ResultadoExportacao
                {
                    Conteudo = zipXlsx,
                    NomeArquivo = $"{nomeBaseArquivo}.zip",
                    ContentType = "application/zip"
                };
            }
            
            if (arquivosCsv.Count == 1)
            {
                return new ResultadoExportacao
                {
                    Conteudo = arquivosCsv[0],
                    NomeArquivo = $"{nomeBaseArquivo}.csv",
                    ContentType = "text/csv"
                };
            }

            var zipBytes = GerarZip(arquivosCsv, nomeBaseArquivo);

            return new ResultadoExportacao
            {
                Conteudo = zipBytes,
                NomeArquivo = $"{nomeBaseArquivo}.zip",
                ContentType = "application/zip"
            };
        }

        private List<byte[]> GerarCsvEmChunks<T>(IEnumerable<T> dados, List<CsvColunaConfiguracao<T>> config) where T : class
        {
            const int LIMITE_PADRAO = 1_000_000;
            var arquivos = new List<byte[]>();
            var buffer = new List<T>(LIMITE_PADRAO);

            foreach (var item in dados)
            {
                buffer.Add(item);

                if (buffer.Count >= LIMITE_PADRAO)
                {
                    arquivos.Add(GerarCsv(buffer, config));
                    buffer.Clear();
                }
            }

            if (buffer.Count > 0)
            {
                arquivos.Add(GerarCsv(buffer, config));
            }
             
            return arquivos;
        }

        private byte[] GerarCsv<T>(IEnumerable<T> rows, List<CsvColunaConfiguracao<T>> config)
        {
            var sb = new StringBuilder();
            
            sb.Append('\uFEFF'); 
            sb.AppendLine(string.Join(";", config.Select(c => EscapeCsvField(c.Cabecalho))));

            foreach (var row in rows)
            {
                var cells = config.Select(c => EscapeCsvField(c.Formatador(row)));
                sb.AppendLine(string.Join(";", cells));
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
        
        private static string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field)) return "";
            
            if (field.Contains(";") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                return $"\"{field.Replace("\"", "\"\"")}\"";
            }
            return field;
        }

        private static byte[] GerarZip(List<byte[]> arquivos, string nomeBaseArquivo, string extensao = "csv")
        {
            extensao = extensao.Trim().TrimStart('.');

            using var zipStream = new MemoryStream();
            using (var arquivoZip = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                for (int i = 0; i < arquivos.Count; i++)
                {
                    var sufixo = arquivos.Count > 1 ? $"_parte{(i + 1):000}" : "";
                    var nomeArquivoInterno = $"{nomeBaseArquivo}{sufixo}.{extensao}";
                    
                    var entradaZip = arquivoZip.CreateEntry(nomeArquivoInterno, CompressionLevel.Fastest);

                    using var streamEntradaZip = entradaZip.Open();
                    streamEntradaZip.Write(arquivos[i], 0, arquivos[i].Length);
                }
            }

            return zipStream.ToArray();
        }

        private static IEnumerable<string[]> LerCsv(byte[] csvBytes, char delimitador = ';')
        {
            var texto = Encoding.UTF8.GetString(csvBytes);

            using var reader = new StringReader(texto);
            using var parser = new TextFieldParser(reader);

            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(delimitador.ToString());
            parser.HasFieldsEnclosedInQuotes = true;

            bool primeiraLinha = true;

            while (!parser.EndOfData)
            {
                var campos = parser.ReadFields() ?? Array.Empty<string>();

                if (primeiraLinha && campos.Length > 0)
                {
                    campos[0] = campos[0].TrimStart('\uFEFF');
                    primeiraLinha = false;
                }

                yield return campos;
            }
        }

        private static byte[] ConverterCsvParaXlsx(byte[] csvBytes)
        {
            var linhas = LerCsv(csvBytes);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Dados");

            int linhaAtual = 1;

            foreach (var linha in linhas)
            {
                int colunaAtual = 1;
                foreach (var valor in linha)
                {
                    if (double.TryParse(valor, out double numero))
                    {
                        worksheet.Cell(linhaAtual, colunaAtual).Value = numero;
                    }
                    else
                    {
                        worksheet.Cell(linhaAtual, colunaAtual).Value = valor ?? string.Empty;
                    }
                    colunaAtual++;
                }
                linhaAtual++;
            }

            var ultimaColuna = worksheet.LastColumnUsed()?.ColumnNumber() ?? 0;
            if (ultimaColuna > 0)
            {
                worksheet.Range(1, 1, 1, ultimaColuna).Style.Font.Bold = true;
                worksheet.SheetView.FreezeRows(1);
                worksheet.Columns().AdjustToContents();
            }

            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return ms.ToArray();
        }
    }
}