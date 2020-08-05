using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AesCryptoServiceProviderAlgorithm
{
    class Program
    {

        //--METHOD UNTUK MENGENCRYPT
        public static string CaraMenEncrypt(string plantext, byte[] key, byte[] IV)
        {
            string result = string.Empty;
            //--CREATE OBJEK AesCryptoServiceProvider
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Key = key;
                aes.IV = IV;
                //--PANGGIL METHOD CreateEncryptor
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                //--CREATE OBJEK MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    //--CREATE OBJEK CryptoStream
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        //--CREATE OBJEK StreamWriter
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            //--WRITE PLANT TEXT KE MEMORY
                            sw.Write(plantext);


                        }
                        //--MENGAMBIL ENCRYPT TEXT DI MEMORY STREAM DAN CONVERT KE STRING BASE-64
                        result = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            return result;
        }
        //--METHOD UNTUK MENDECRYPT
        public static string CaraMenDecrypt(byte[] textEncrypt, byte[] key, byte[] IV)
        {
            string result = string.Empty;
            //--CREATE OBJEK AesCryptoServiceProvider
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Key = key;
                aes.IV = IV;
                //--PANGGIL METHOD CreateDecryptor
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                //--CREATE OBJEK MemoryStream
                using (MemoryStream ms = new MemoryStream(textEncrypt))
                {
                    //--CREATE OBJEK CryptoStream
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        //--CREATE OBJEK StreamReader
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            result = sr.ReadToEnd();
                        }
                    }
                }
            }
            return result;
        }
        //--METHOD MEMBUAT BYTE ARRAY KEY
        public static byte[] GetKey(string key)
        {
            //--KEY INI BISA DI CONVERT STRING KE BYTE ARRAY ATAU BISA DENGAN MENGENCRYPT KEMBALI STRING KEY,BISA METHOD DENGAN MENGECRYPT LAGI KEY,
            //--DAN BISA JUGA DENGAN MENGGUNAKAN KEY YANG TELAH DI SEDIAKAN OLEH C#
            //--KALI INI SAYA MEMAKAI HASH UNTUK MENDAPATKAN BYTE ARRAY DARI KEY KARENA KALAU MEMAKAI YANG TELAH DISEDIAKAN C# KITA PERLU TAHU RANDOM KEY
            //--YANG DISEDIAKAN C# AGAR BISA DI DECRYPT MELALUI APLIKASI LAIN
            SHA256 sHA256 = SHA256.Create();

            return sHA256.ComputeHash(Encoding.UTF8.GetBytes(key));
        }
        static void Main(string[] args)
        {
            Console.Write("TEXT TO ENCRYPT                 : ");
            string plantext = Console.ReadLine();
            Console.Write("ENTER KEY TO ENCRYPT AND DECRYPT: ");
            string key = Console.ReadLine();
            //--CREATE OBJEK AesCryptoServiceProvider UNTUK MENDAPATKAN INITILIZE VECTOR YANG TELAH DI SEDIAKAN C#..
            //--INITILIZE VEKTOR INI JUGA BISA DI BUAT
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                string encrypt = CaraMenEncrypt(plantext, GetKey(key), aes.IV);
                Console.WriteLine("HASIL ENCRYPT              : " + encrypt);
                //--DECRYPT KEMBALI TEXT YANG DI ENCRYPT TADI
                Console.WriteLine("HASIL DECRYPT TEXT ENCRYPT : " + CaraMenDecrypt(Convert.FromBase64String(encrypt), GetKey(key), aes.IV));
            }
        }
    }
}
