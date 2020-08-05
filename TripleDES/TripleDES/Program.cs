using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TripleDESAlgorithm
{
    class Program
    {
        //--METHOD UNTUK MENGENCRYPT
        public string Encrypt(string plaintex, byte[] key, byte[] iv)
        {
            string result = string.Empty;
            //--CREATE OBJEK TripleDESCryptoServiceProvider
            using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider())
            {
                //--PANGGIL METHOD CreateEncryptor
                ICryptoTransform encryptor = tripleDES.CreateEncryptor(key, iv);
                //--CREATE OBJEK MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    //--CREATE OBJEK CryptoStream
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        //--UBAH STRING PLAINTEXT KE BIT ARRAY
                        byte[] plaintexArray = new ASCIIEncoding().GetBytes(plaintex);
                        //--WRITE PLAIN TEXT KE CRYPTO
                        cs.Write(plaintexArray, 0, plaintexArray.Length);
                        //--UPDATE ENCRYPT BIT ARRAY DI STREAM DAN KOSONGKAN PLAINTEXT ARRAY
                        cs.FlushFinalBlock();
                        //--MENDAPATKAN ARRAY DALAM STRING DAN CONVERT KE STRING
                        result = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            return result;
        }
        //--METHOD UNTUK DECRYPT
        public string Decrypt(string encrypt, byte[] key, byte[] iv)
        {
            string resuit = string.Empty;
            //--CREATE OBJEK TripleDESCryptoServiceProvider
            using (TripleDESCryptoServiceProvider triple = new TripleDESCryptoServiceProvider())
            {
                //--PANGGIL METHOD CreateDecryptor
                ICryptoTransform decrytor = triple.CreateDecryptor(key, iv);
                //--MENDAPATKAN BIT ARRAY DARI ENCRYPT TEXT
                byte[] encryptArray = Convert.FromBase64String(encrypt);
                //--CREATE OBJEK MemoryStream
                using (MemoryStream ms = new MemoryStream(encryptArray))
                {
                    //--CREATE OBJEK CryptoStream
                    using (CryptoStream cs = new CryptoStream(ms, decrytor, CryptoStreamMode.Read))
                    {
                        //--CREATE EMPTY BIT ARRAY UNTUK MENAMPUNG BIT ARRAY DARI DECRYPT TEXT
                        byte[] resultArray = new byte[encryptArray.Length];
                        //--MENDAPATKAN DATA DECRYPT TEXT DARI STREAM
                        cs.Read(resultArray, 0, resultArray.Length);
                        resuit = new ASCIIEncoding().GetString(resultArray);
                    }
                }
            }
            return resuit;
        }
        static void Main(string[] args)
        {
            Program p = new Program();
            Console.Write("Input Text to Encrypt : ");
            string plaintex = Console.ReadLine();
            using(TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider())
            {
                Console.WriteLine("------------------------------------------");
                string encrypt = p.Encrypt(plaintex, tripleDES.Key, tripleDES.IV);
                Console.WriteLine("Encrypt Text : " + encrypt);
                Console.WriteLine("Decrypt Text : " + p.Decrypt(encrypt, tripleDES.Key, tripleDES.IV));
            }
        }
    }
}
