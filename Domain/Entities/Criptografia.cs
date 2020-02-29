using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Entities
{
    public class Criptografia
    {
        //Vetor de inicialização de criptografia
        private static readonly byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
        private static string KEY_DES = "12345678";

        public static string Encriptar(string texto)
        {
            //"12345678"
            byte[] key = Encoding.UTF8.GetBytes(KEY_DES);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = Encoding.UTF8.GetBytes(texto);

            MemoryStream ms = new MemoryStream();

            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Desencriptar(string texto)
        {
            byte[] inputByteArray;

            try
            {
                byte[] key = Encoding.UTF8.GetBytes(KEY_DES);

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                inputByteArray = Convert.FromBase64String(texto);
                MemoryStream ms = new MemoryStream();

                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                Encoding encoding = Encoding.UTF8;

                return encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
