using EFCore.Sample.Business.Enum;
using EFCore.Sample.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EFCore.Sample.ExternalApi.Extensions
{
    public class Validations
    {
        public static string CreateReference(TransactionReference reference)
        {
            string referenceField;

            if (reference.ReferenceType == Business.Enum.ReferenceType.ENOTA)
            {
                referenceField = $"ENOTA{reference.YearMonth}{reference.Document}";
            }
            else if (reference.ReferenceType == Business.Enum.ReferenceType.ENOTAPARTNER)
            {
                referenceField = $"ENOTAPARTNER{reference.YearMonth}{reference.Document}";
            }
            else
            {
                referenceField = $"{reference.YearMonth}{reference.Document}";
            }

            return referenceField;
        }

        public static string FormatReferenceDescription(string sentence, string document)
        {
            string reference = string.Empty;
            if (sentence == string.Empty)
            {
                reference = $"{ReferenceType.ENOTA.ToString()}202109{document}";
            }
            else
            {
                var onlyNumbersSentence = new String(sentence.Where(c => Char.IsNumber(c)).ToArray());

                if (IsCpf(onlyNumbersSentence) || IsCnpj(onlyNumbersSentence))
                {
                    reference = $"{ReferenceType.ENOTA.ToString()}202109{onlyNumbersSentence}";
                    var result = string.Concat(onlyNumbersSentence.ToArray().Reverse().TakeWhile(char.IsNumber).Reverse());
                }
                else {
                    reference = $"{ReferenceType.ENOTA.ToString()}202109{document}";
                }
            }

            return reference;
        }

        public static bool IsDocument(string input)
        {
            bool isValid = false;

            var stack = new Stack<char>();

            for (var i = input.Length - 1; i >= 0; i--)
            {
                if (!char.IsNumber(input[i]))
                {
                    break;
                }

                stack.Push(input[i]);
            }

            var result = new string(stack.ToArray());

            // Obtém CPF ou CNPJ da tela
            var cpfCnpj = result;

            if (IsCpf(cpfCnpj) || IsCnpj(cpfCnpj))
            {
                isValid = true;
            }

            return isValid;

        }

        // Função que valida CPF
        static Func<string, bool> IsCpf = cpf =>
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    return false;

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
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
        };

        // Função que Valida CNPJ
        static Func<string, bool> IsCnpj = cnpj =>
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
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
        };

        public static bool VerifyReference(string sentence, out ReferenceType referenceType)
        {
            referenceType = ReferenceType.ENOTA;
            var enotaPartner = RegexReferenceTye(ReferenceType.ENOTAPARTNER.ToString(), sentence);
            var enotaPre = RegexReferenceTye(ReferenceType.ENOTAPRE.ToString(), sentence);
            var enota = RegexReferenceTye(ReferenceType.ENOTA.ToString(), sentence);



            var result = (enotaPartner || enota || enotaPre);

            if (enotaPartner)
            {
                referenceType = ReferenceType.ENOTAPARTNER;
            }

            else if (enotaPre)
            {
                referenceType = ReferenceType.ENOTAPRE;
            }

            else if (enota)
            {
                referenceType = ReferenceType.ENOTA;
            }
            else
            {
                referenceType = ReferenceType.ENOTA;
            }


            return result;
        }

        public static bool RegexReferenceTye(string criteria, string sentence)
        {
            bool isMatch = false;

            var onlyLettersSentence = new String(sentence.Where(c => Char.IsLetter(c) && Char.IsUpper(c)).ToArray());
            isMatch = Regex.IsMatch(onlyLettersSentence, @$"\b{criteria}\b");

            return isMatch;

        }

        public static bool VerifyCallBack(string sentence)
        {
            var callBack = @"callbacks.safeweb.com.br";

            Regex ER = new Regex(callBack, RegexOptions.None);
            var urlCallBack = ER.IsMatch(sentence);

            return urlCallBack;

        }
    }
}

//public async Task<string> GetGoodbyeMessage()
//{
//    try
//    {
//        Console.WriteLine($"Circuit State: {_circuitBreakerPolicy.CircuitState}");
//        return await _circuitBreakerPolicy.ExecuteAsync<string>(async () =>
//        {
//            return await _safePayRepository.GetGoodbyeMessage();
//        });
//    }
//    catch (Exception ex)
//    {
//        return ex.Message;
//    }
//}
