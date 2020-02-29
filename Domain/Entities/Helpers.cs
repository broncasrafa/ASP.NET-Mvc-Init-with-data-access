using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using HtmlAgilityPack;

namespace Domain.Entities
{
    public static class Helpers
    {
        /// <summary>
        /// Encripta a string
        /// </summary>
        /// <param name="value">valor da string</param>
        /// <returns>retorna a string encriptada</returns>
        public static string Encriptar(object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return Criptografia.Encriptar(value.ToString());
        }

        /// <summary>
        /// Desencripta a string
        /// </summary>
        /// <param name="value">valor da string encriptada</param>
        /// <returns>retorna a string desencripta</returns>
        public static string Desencriptar(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return Criptografia.Desencriptar(value.ToString());
        }

        /// <summary>
        /// remover o que não for digito na string
        /// </summary>
        /// <param name="value">valor para remover o que não for digito</param>
        /// <returns>retorna uma string com somente digitos</returns>
        public static string OnlyDigits(string value)
        {
            if (String.IsNullOrEmpty(value)) return null;

            string result = string.Empty;
            foreach (char c in value)
            {
                if (char.IsDigit(c))
                    result += c;
            }
            return result;
        }

        /// <summary>
        /// remove da string tudo o que não for letras e numeros
        /// </summary>
        /// <param name="value">valor da string</param>
        /// <returns>uma string com somente digitos e letras</returns>
        public static string OnlyLetterOrDigits(string value)
        {
            string result = string.Empty;
            foreach (char c in value)
            {
                if (char.IsLetterOrDigit(c))
                    result += c;
            }
            return result;
        }

        /// <summary>
        /// Verifica se a string é somente digitos
        /// </summary>
        /// <param name="value">valor da string</param>
        /// <returns>true se for somente digitos na string</returns>
        public static bool IsDigitOnly(string value)
        {
            foreach (char c in value)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Verifica se a string é somente numeros
        /// </summary>
        /// <param name="value">valor da string</param>
        /// <returns>retorna true se for somente numeros</returns>
        public static bool IsNumeric(string value)
        {
            double _out;
            return double.TryParse(value, out _out);
        }

        /// <summary>
        /// Verifica se a string tem somente numeros
        /// </summary>
        /// <param name="value">valor da string</param>
        /// <returns>retorna true se a string tiver somente numeros</returns>
        public static bool VerificarSomenteNumeros(string value)
        {
            if (String.IsNullOrEmpty(value)) return false;

            var temLetras = Regex.Matches(value, @"[a-zA-Z]").Count > 0;

            if (temLetras) return false;

            var numeros = OnlyDigits(value);

            return IsNumeric(numeros);
        }

        /// <summary>
        /// Verifica se o cpf é valido
        /// </summary>
        /// <param name="value">o valor do cpf</param>
        /// <returns>retorna true se o cpf for valido</returns>
        public static bool IsValidCpf(string value)
        {
            var cpf = value;
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();

            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        /// <summary>
        /// Verifica se o cnpf é valido
        /// </summary>
        /// <param name="value">o valor do cnpf</param>
        /// <returns>retorna true se o cnpf for valido</returns>
        public static bool IsValidCnpf(string value)
        {
            var cnpj = value;
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;

            cnpj = cnpj.Trim();

            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            tempCnpj = cnpj.Substring(0, 12);

            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCnpj = tempCnpj + digito;

            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

        /// <summary>
        /// Verifica se o pis/pasep é valido
        /// </summary>
        /// <param name="value">o valor do pis/pasep</param>
        /// <returns>retorna true se o pis/pasep for valido</returns>
        public static bool IsValidPisPasep(string value)
        {
            var pis = value;
            int[] multiplicador = new int[10] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;

            if (pis.Trim().Length != 11)
                return false;

            pis = pis.Trim();
            pis = pis.Replace("-", "").Replace(".", "").PadLeft(11, '0');

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(pis[i].ToString()) * multiplicador[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            return pis.EndsWith(resto.ToString());
        }

        /// <summary>
        /// Verifica se é uma data valida
        /// </summary>
        /// <param name="value">string da data</param>
        /// <returns>true se for uma data valida</returns>
        public static bool IsDateTime(string value)
        {
            DateTime datetime;
            return DateTime.TryParse(value, out datetime);
        }

        /// <summary>
        /// Verifica se é um e-mail valido
        /// </summary>
        /// <param name="value">valor da string</param>
        /// <returns>retorna true se for um e-mail valido</returns>
        public static bool IsValidEmail(string value)
        {
            if (String.IsNullOrEmpty(value)) return false;

            string pattern = @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";
            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return regex.IsMatch(value);
        }

        /// <summary>
        /// Converte stream para array de bytes
        /// </summary>
        /// <param name="path">caminho do arquivo</param>
        /// <returns>retorna os bytes</returns>
        public static byte[] StreamToBytes(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (Stream input = File.OpenRead(Path.Combine(path)))
            {
                using (var ms = new MemoryStream())
                {
                    input.CopyTo(ms);
                    ms.Close();
                    input.Close();
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Formatar a string de telefone, tanto fixo quanto celular.
        /// </summary>
        /// <param name="value">valor da string de telefone</param>
        /// <returns>string de telefone formatada</returns>
        public static string FormatarTelefone(string value)
        {
            if (String.IsNullOrEmpty(value)) return null;

            value = RemoverFormatacaoTelefone(value);
            return value.Trim().Length <= 10 ? string.Format("{0:(##) ####-####}", Convert.ToInt32(value)) : string.Format("{0:(##) #####-####}", Convert.ToUInt64(value));
        }

        /// <summary>
        /// Remove a formatação do telefone
        /// </summary>
        /// <param name="value">valor da string de telefone</param>
        /// <returns>string de telefone sem formatação</returns>
        public static string RemoverFormatacaoTelefone(string value)
        {
            if (String.IsNullOrEmpty(value)) return null;

            return value.Replace("(", "")
                        .Replace(")", "")
                        .Replace("-", "")
                        .Trim();
        }

        /// <summary>
        /// Formata a string de CNPJ ou CPF
        /// </summary>
        /// <param name="value">o valor da string de cnpj ou cpf</param>
        /// <returns>retorna a string de cnpj ou cpf formatada</returns>
        public static string FormatarCpfCnpj(string value)
        {
            if (String.IsNullOrEmpty(value)) return null;

            value = RemoverFormatacaoCpfCnpj(value);

            if(value.Length > 11)
                return Convert.ToUInt64(value).ToString(@"00\.000\.000/0000-00");
            else
                return Convert.ToUInt64(value).ToString(@"000\.000\.000\-00");
        }

        /// <summary>
        /// Remove a formatação do cnpj
        /// </summary>
        /// <param name="value">valor da string de cnpj</param>
        /// <returns>string de cnpj sem formatação</returns>
        public static string RemoverFormatacaoCpfCnpj(string value)
        {
            if (String.IsNullOrEmpty(value)) return null;

            return value.Replace(".", "")
                        .Replace(".", "")
                        .Replace("/", "")
                        .Replace("-", "")
                        .Trim();
        }

        /// <summary>
        /// Formatar a string de valor monetário sem R$
        /// </summary>
        /// <param name="value">o valor decimal para formatar</param>
        /// <returns>retorna a string de valor monetário sem R$</returns>
        public static string FormatarValorMonetario(decimal? value)
        {
            if (value == null) return null;

            return string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N2}", Convert.ToDecimal(value.ToString().Replace(".", ",")));
        }

        public static string DigitosFormatados(string value)
        {
            if (String.IsNullOrEmpty(value)) return null;

            if (value.Length > 3)
                value = string.Format("{0:0,0}", int.Parse(value, CultureInfo.GetCultureInfo("pt-BR").NumberFormat));

            return value;
        }

        /// <summary>
        /// Obter o mês do ano por extenso
        /// </summary>
        /// <param name="mes">valor do mês</param>
        /// <returns>retorna o mês especificado do ano por extenso</returns>
        public static string MesPorExtenso(int? mes)
        {
            if (mes == null) return null;
            if (String.IsNullOrEmpty(mes.ToString())) return null;
            if (mes == 0) return null;

            string mesExtenso = string.Empty;
            switch (mes)
            {
                case 1: mesExtenso = "Janeiro"; break;
                case 2: mesExtenso = "Fevereiro"; break;
                case 3: mesExtenso = "Março"; break;
                case 4: mesExtenso = "Abril"; break;
                case 5: mesExtenso = "Maio"; break;
                case 6: mesExtenso = "Junho"; break;
                case 7: mesExtenso = "Julho"; break;
                case 8: mesExtenso = "Agosto"; break;
                case 9: mesExtenso = "Setembro"; break;
                case 10: mesExtenso = "Outubro"; break;
                case 11: mesExtenso = "Novembro"; break;
                case 12: mesExtenso = "Dezembro"; break;
                default:
                    break;
            }
            return mesExtenso;
        }

        /// <summary>
        /// Obter o dia da semana por extenso
        /// </summary>
        /// <param name="dayOfWeek">data</param>
        /// <returns>retorna a string do dia da semana da data especificada</returns>
        public static string DiaSemanaPorExtenso(DateTime dayOfWeek)
        {
            if (dayOfWeek == DateTime.MinValue) return null;

            string result = string.Empty;

            switch (dayOfWeek.DayOfWeek)
            {
                case DayOfWeek.Sunday: result = "Domingo"; break;
                case DayOfWeek.Monday: result = "Segunda-Feira"; break;
                case DayOfWeek.Tuesday: result = "Terça-Feira"; break;
                case DayOfWeek.Wednesday: result = "Quarta-Feira"; break;
                case DayOfWeek.Thursday: result = "Quinta-Feira"; break;
                case DayOfWeek.Friday: result = "Sexta-Feira"; break;
                case DayOfWeek.Saturday: result = "Sábado"; break;
            }

            return result;
        }

        /// <summary>
        /// Obter o dia da semana por extenso
        /// </summary>
        /// <param name="data">o valor da data</param>
        /// <returns>retorna dia da semana por extenso</returns>
        public static string DataMesPorExtenso(DateTime data)
        {
            if (data == DateTime.MinValue) return null;

            return $"{data.Day} de {MesPorExtenso(data.Month).ToLower()}";
        }

        /// <summary>
        /// Obter o ultimo dia do mes e ano especificados
        /// </summary>
        /// <param name="ano">valor do ano</param>
        /// <param name="mes">valor do mes</param>
        /// <returns>retorna a data do ultimo dia do mes</returns>
        public static DateTime? DataUltimoDiaDoMes(int ano, int mes)
        {
            if (ano <= 0) return null;
            if (mes <= 0 && mes > 12) return null;

            var dia = DateTime.DaysInMonth(ano, mes);
            return new DateTime(ano, mes, dia);
        }

        /// <summary>
        /// Obter o ultimo dia do mes e ano especificados
        /// </summary>
        /// <param name="ano">valor do ano</param>
        /// <param name="mes">valor do mes</param>
        /// <returns>retorna a ultimo dia do mes</returns>
        public static int? UltimoDiaDoMes(int ano, int mes)
        {
            if (ano <= 0) return null;
            if (mes <= 0 && mes > 12) return null;

            return DateTime.DaysInMonth(ano, mes);
        }

        /// <summary>
        /// Obter a data por extenso igual medias sociais
        /// </summary>
        /// <param name="data">o valor da data</param>
        /// <returns>retorna a data por extenso igual medias sociais</returns>
        public static string GetDataPorExtensoSocialMedias(DateTime data)
        {
            if (data == DateTime.MinValue) return null;

            int totalDias = (DateTime.Now.Date - data.Date).Days;

            if (data.Date == DateTime.Now.Date)
                return $"hoje às {data.ToString("HH:mm")}";
            else if (data.Date == DateTime.Now.Date.AddDays(-1).Date)
                return $"ontem às {data.ToString("HH:mm")}";

            return $"{DataMesPorExtenso(data)} às {data.ToString("HH:mm")}";
        }

        /// <summary>
        /// Obter a string do corpo do e-mail em formato HTML
        /// </summary>
        /// <param name="body">o texto do corpo da mensagem do email</param>
        /// <param name="hasImageToAttach">se tem imagem para anexar ao corpo do e-mail</param>
        /// <param name="imageAttachment">objeto de imagem anexo ao corpo do email</param>
        /// <returns>retorna a string do corpo do e-mail em formato HTML</returns>
        public static string GetHtmlBodyEmail(string body, bool hasImageToAttach = false, EmailImageAttachment imageAttachment = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <meta http-equiv='X-UA-Compatible' content='ie=edge'>
                <title></title>    
	            <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css' />
                <style>
                </style>
            </head>

            <body>
                <div class='container'>
                    <div class='row'>
                        <div class='col-md-12'>{body}</div>
                    </div>");
            if (hasImageToAttach)
            {
                sb.AppendLine("<p>");

                if(imageAttachment != null)
                {
                    sb.AppendLine($"<img src=\"{imageAttachment.Filepath}\"  />");
                }

                sb.AppendLine("<p>");
            }
            sb.AppendLine(@"
                </div>
            </body>
            </html>");

            return sb.ToString();
        }

        /// <summary>
        /// remover todas as tags Html da string
        /// </summary>
        /// <param name="value">o valor da string</param>
        /// <returns>retorna a string sem tags html</returns>
        public static string RemoverHtmlTags(string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.Load(value);
            return htmlDoc.DocumentNode.InnerText;
        }

        /// <summary>
        /// Obter o memorystream de uma string base64
        /// </summary>
        /// <param name="imageBase64">valor da string base64</param>
        /// <returns>retorna o memorystream de uma string base64</returns>
        public static MemoryStream GetStreamFromBase64String(string imageBase64)
        {
            if (String.IsNullOrEmpty(imageBase64))
                throw new ArgumentNullException("imageBase64");

            if (imageBase64.IndexOf(',') > 0)
                imageBase64 = imageBase64.Substring(imageBase64.IndexOf(',') + 1);

            byte[] bytes = Convert.FromBase64String(imageBase64);
            var ms = new MemoryStream(bytes);
            return ms;
        }

        /// <summary>
        /// Obter o memorystream de um array de bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static MemoryStream GetStreamFromByteArray(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            return new MemoryStream(bytes);
        }
    }
}
