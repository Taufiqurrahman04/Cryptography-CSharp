using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RijnaelManagedAlgorithm
{
    class Program
    {
        //--METHOD UNTUK MENENCRYPT
        public string Encrypt(string plaintext,byte[]key, byte[] iv)
        {
            string result = string.Empty;
            //--CREATE OBJEK RijndaelManaged 
            using (RijndaelManaged rj = new RijndaelManaged())
            {
                //--MEMANGGIL METHOD CreateEncryptor
                ICryptoTransform encryptor = rj.CreateEncryptor(key, iv);
                //--CREATE OBJEK MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    //--CREATE OBJEK CryptoStream
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        //--CONVERT STRING INPUT KE BYTE ARRAY
                        byte[] plaintextArray = new ASCIIEncoding().GetBytes(plaintext);
                        //--WRITE PLAINTEXTARRAY KE CRYPTO
                        cs.Write(plaintextArray, 0, plaintextArray.Length);
                        //--UPDATE DATA DI MEMORY DENGAN PLAINTEXARRAY DAN CLEAR PLAINTEXTARRAY
                        cs.FlushFinalBlock();
                        //--GET DATA DI MEMORY DAN CONVERT KE STRING
                        result = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            return result;
        }
        //--METHOD UNTUK MENDECRYPT
        public string Decrypt(string encrypt,byte[]key, byte[] iv)
        {
            string result = string.Empty;
            //--CREATE OBJEK RijndaelManaged
            using (RijndaelManaged rj = new RijndaelManaged())
            {
                //--MEMANGGIL METHOD CreateDecryptor
                ICryptoTransform decryptor = rj.CreateDecryptor(key, iv);
                //--CONVERT STRING ENCRYPT KE BYTE ARRAY
                byte[] encryptAray = Convert.FromBase64String(encrypt);
                //--CREATE MEMORY STREAM OBJEK
                using(MemoryStream ms = new MemoryStream(encryptAray))
                {
                    //--CREATE OBJEK CryptoStream 
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        //--CREATE EMPTY BYTE ARRAY UNTUK MENYIMPAN DECRYPT BYTE ARRAY
                        byte[] resultArray = new byte[encryptAray.Length];
                        //--COPY DECRYPT BYTE ARRAY DARI MEMORY
                        cs.Read(resultArray, 0, resultArray.Length);
                        //--CONVERT DECRYPT BYTE ARRAY KE STRING
                        result = new ASCIIEncoding().GetString(resultArray);
                    }
                }
            }
            return result;
        }
        static void Main(string[] args)
        {
            Program p = new Program();
            Console.Write("Input Text to Encrypt : ");
            string plaintext = Console.ReadLine();
            //--MEMANGGIL OBJEK RIJNDAEL MANAGED UNTUK GENERATE KEY DAN IV
            using(RijndaelManaged rj = new RijndaelManaged())
            {
                Console.WriteLine("------------------------------");
                //--GET ENCRYPT TEXT DAN PRINT
                string encrypt = p.Encrypt(plaintext, rj.Key, rj.IV);
                Console.WriteLine("Encrypt Text : "+encrypt);
                //--GET DECRYPT TEXT DAN PRINT
                Console.WriteLine("Decrypt Text : " + p.Decrypt(encrypt, rj.Key, rj.IV));
            }
        }
    }
}
