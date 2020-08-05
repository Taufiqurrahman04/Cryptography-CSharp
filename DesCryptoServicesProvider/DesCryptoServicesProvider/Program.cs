using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DesCryptoServicesProviderAlgorith
{
    class Program
    {
        //--METHOD UNTUK MENGENCRYPT
        private string Encrypt(string plaintext, byte[]key,byte[] iv)
        {
            string result = string.Empty;
            //--MEMANGGIL OBJEK DESCryptoServiceProvider
            using (DESCryptoServiceProvider DesCrypto = new DESCryptoServiceProvider())
            {
                //--MEMANGGIL METHOD CreateEncryptor
                ICryptoTransform encryptor = DesCrypto.CreateEncryptor(key,iv);
                //--MEMANGGIL OBJEK MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    //---MEMANGGIL OBJEK CryptoStream 
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        //--MENGAMBIL BIT ARRAY DARI TEXT YANG AKAN DI ENCRYPT
                        byte[] inputArray = new ASCIIEncoding().GetBytes(plaintext);
                        //--WRITE INPUT ARRAY KE CRYPTO STREAM
                        cs.Write(inputArray, 0, inputArray.Length);
                        //--MENGUPDATE ARRAY YANG DI SIMPAN DI MEMORY STREAM DENGAN INPUT ARRAY DAN MENGOSONGKAN INPUT ARRAY
                        cs.FlushFinalBlock();
                        //--MENGAMBIL ARRAY DI MEMORY STREAM DAN CONVERT KE STRING BASE 64
                        result = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            return result;
        }
        //--METHOD UNTUK MENDECRYPT
        private string Decrypt(string encrypttext, byte[] key , byte[] iv)
        {
            
            string result = string.Empty;
            //--CREATE OBJEK DESCryptoServiceProvider 
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                //--CONVERT ENCRYPT TEXT KE BIT ARRAY
                byte[] encryptArray = Convert.FromBase64String(encrypttext);
                //--MEMANGGIL METHOD CreateDecryptor
                ICryptoTransform decryptor = des.CreateDecryptor(key,iv);
                //--CREATE OBJEK MemoryStream
                using (MemoryStream ms = new MemoryStream(encryptArray))
                {
                    //--CREATE OBJEK CryptoStream
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        //--CREATE EMPTY BIT ARRAY UNTUK MENAMPUNG DATA DECRYPT BIT ARRAY NYA
                        byte[] resultArray = new byte[encryptArray.Length];
                        //--READ DATA DARI STREAM
                        cs.Read(resultArray, 0, resultArray.Length);
                        //--CONVERT DATA BIT ARRAY KE STRING
                        result = new ASCIIEncoding().GetString(resultArray);
                    }
                }
            }
            return result;
        }
        static void Main(string[] args)
        {
            //--CREATE OBJEK PROGRAM
            Program p = new Program();
            //--CREATE OBJEK DESCryptoServiceProvider UNTUK MENGGENERATE KEY DAN IV
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {

                Console.Write("Input Text to Encrypt : ");
                //--INPUT TEXT YANG AKAN DI ENCRYPT
                string plaintext = Console.ReadLine();
                Console.WriteLine("---------------------------------");
                //--PANGGIL METHOD UNTUK MENGENCRYPT DAN PRIN HASILNYA
                string encrypt = p.Encrypt(plaintext, des.Key, des.IV);
                Console.WriteLine("Encrypt Text : " + encrypt);
                //--PANGGIL METHOD UNTUK DECRYPT DAN PRINT HASILNYA
                Console.WriteLine("Decrypt Text : " + p.Decrypt(encrypt, des.Key, des.IV));
            }
        }
    }
}
