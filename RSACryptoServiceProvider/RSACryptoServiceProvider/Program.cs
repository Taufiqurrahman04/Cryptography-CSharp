using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSACryptoServiceProviderAlgorithm
{
    class Program
    {
        //--METHODE UNTUK ENCRYPT
        static byte[] Encrypt(byte[] plaintext,string publicKey,RSAEncryptionPadding DoOAEPPadding)
        {
            byte[] result = null;
            //--CREATE OBJEK RSACryptoServiceProvider
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                //--SET PUBLIK KEY
                rsa.FromXmlString(publicKey);
                //--ENCRYPT DAN DAPATKAN BYTE ARRAY ENCRYPT TEXT
              result = rsa.Encrypt(plaintext, DoOAEPPadding);
            }
            return result;
        }
        //--METHOD UNTUK DECRYPT
        static byte[] Decrypt(byte[] encrypttext,string privateKey,RSAEncryptionPadding DoOAEPPadding)
        {
            byte[] result = null;
            //--CREATE OBJEK RSACryptoServiceProvider
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                //--SET PRIVSTE KEY
                rsa.FromXmlString(privateKey);
                //--DECRYPT DAN MENDAPATAKAN BYTE ARRAY DECRYPT TEXT
                result = rsa.Decrypt(encrypttext, DoOAEPPadding);
            }
            return result;
        }
        static void Main(string[] args)
        {
            Console.Write("Input Text To encrypt : ");
            string textinput =Console.ReadLine();
            //--CREATE OBJEK RSACryptoServiceProvider UNTUK GENERATE PUBLIC DAN PRIVATE KEY
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                //--CREATE PADDING YANG DI GUNAKAN UNTUK PROSES ENCRYPT DAN DECRYPT
                RSAEncryptionPadding padding = RSAEncryptionPadding.OaepSHA1;
                Console.WriteLine("-------------------------------------------------");
                //--GENERATE PRIVATE KEY
                string privateKey = rsa.ToXmlString(true);
                //--GENERATE PUBLIC KEY
                string publicKey = rsa.ToXmlString(false);
                //--PRINT PRIVATE KEY
                Console.WriteLine("Ini Private Key Untuk Mendecrypt : "+privateKey);
                Console.WriteLine("-------------------------------------------------");
                //--PRINT PUBLIC KEY
                Console.WriteLine("Ini Public Key Untuk Mengencrypt : " + publicKey);
                Console.WriteLine("-------------------------------------------------");
                //--MENDAPATKAN OUTPUT DAN CONVERT KE STRING ENCRYPT TEXT
               string encrypt = Convert.ToBase64String(Encrypt(new UnicodeEncoding().GetBytes(textinput), publicKey, padding));
                //--PRINT ENCRYPT TEXT
                Console.WriteLine("Encrypt Text : " + encrypt);
                //--MENDAPATKAN OUTPUT DECRYPT DAN PRINT
                Console.WriteLine("Decrypt Text : " + new UnicodeEncoding().GetString( Decrypt(Convert.FromBase64String(encrypt),privateKey, padding)));
            }
        }
    }
}
