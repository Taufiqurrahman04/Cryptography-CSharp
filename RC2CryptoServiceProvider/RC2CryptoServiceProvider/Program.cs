using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RC2CryptoServiceProviderAlgorithm
{
    class Program 
    {
        //--METHOD UNTUK MENGECNRYPT
        public string Encrypt(string plainttext, byte[] key,byte[] iv)
        {
            string result = string.Empty;
            //--CREATE OBJEK RC2CryptoServiceProvider
            using (RC2CryptoServiceProvider rs2 = new RC2CryptoServiceProvider())
            {
                //--MEMANGGIL METHOD CreateEncryptor
                ICryptoTransform ecnryptor = rs2.CreateEncryptor(key, iv);
                //--CREATE OBJEK MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    //--CREATE OBJEK CryptoStream
                    using (CryptoStream cs = new CryptoStream(ms, ecnryptor, CryptoStreamMode.Write))
                    {
                        //--CONVERT STRING INPUT KE BYTE ARRAY
                        byte[] butArrayInput = new ASCIIEncoding().GetBytes(plainttext);
                        //--WRITE BYTE ARRAY INPUT KE CRYPTO
                        cs.Write(butArrayInput, 0, butArrayInput.Length);
                        //--UPDATE BYTE ARRAY DALAM MEMORY DENGAN BUT ARRAY INPUT DAN CLEAR BUT ARRAY INPUT
                        cs.FlushFinalBlock();
                        //--MENGAMBIL BYET ARRAY MEMORY DAN MENGKONVERT KE STRING
                        result = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            return result;
        }
        //--METHOD UNTUK MENDECRYPT
        public string Decrypt(string encrypt, byte[] Key, byte[] iv)
        {
            string result = string.Empty;
            //--CREATE OBJEK RC2CryptoServiceProvider
            using (RC2CryptoServiceProvider rC2 = new RC2CryptoServiceProvider())
            {
                //--MEMANGGIL METHOD CreateDecryptor
                ICryptoTransform decrypt = rC2.CreateDecryptor(Key, iv);
                //--CONVERT ENCRYPT TEXT KE BYTE ARRAY
                byte[] encryptArray = Convert.FromBase64String(encrypt);
                //--CREATE OBJEK MemoryStream 
                using (MemoryStream ms = new MemoryStream(encryptArray))
                {
                    //--CREATE OBJEK CryptoStream
                    using (CryptoStream cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read))
                    {
                        //--CREATE NEW BYTE ARRAY UNTUK MENYIMPAN DECRYPT BYTE ARRAY
                        byte[] resultArray = new byte[encryptArray.Length];
                        //--READ DECRYPT DI MEMORY KE RESULT ARRAY
                        cs.Read(resultArray, 0, resultArray.Length);
                        //--CONVERT BYTE ARRAY DECRYPT TEXT KE STRING
                        result = new ASCIIEncoding().GetString(resultArray);
                    }
                }
            }
            return result;
        }
        static void Main(string[] args)
        {
            Program p = new Program();
            Console.Write("Input Text To Encrypt : ");
            string plaintext = Console.ReadLine();
            //--CREATE OBJEK RC2CryptoServiceProvider UNTUK MENDAPATKAN KEY DAN IV SECARA AUTOMATIS
            using (RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider())
            {
                Console.WriteLine("------------------------------------");
                //--GET ENCRYPT TEXT DAN PRINT
                string encrypt = p.Encrypt(plaintext, rc2.Key, rc2.IV);
                Console.WriteLine("Encrypt text : " + encrypt);
                //--GET DECRYPT TEXT DAN PRINT
                Console.WriteLine("Decrypt text : " + p.Decrypt(encrypt, rc2.Key, rc2.IV));
            }
        }
    }
}
