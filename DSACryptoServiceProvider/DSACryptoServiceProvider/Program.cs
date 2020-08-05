using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DSACryptoServiceProviderAlgorithm
{
    class Program
    {
        #region SIGN DATA
        //--METHOD UNTUK CREATE SIGNATURE DENGAN SIGN DATA
        static byte[] SignData(byte[] dataSign,string privateKey)
        {
            byte[] result = null;
            //--CREATE OBJEK DSACryptoServiceProvider 
            using (DSACryptoServiceProvider dsa = new DSACryptoServiceProvider())
            {
                //--SET PRIVATE KEY
                dsa.FromXmlString(privateKey);
                //--CREATE SIGNATURE DENGAN SIGN DATA
               result =  dsa.SignData(dataSign);
            }
            return result;
        }
        //--METHOD UNTUK VERIFY DATA
        static bool VerifyData(byte[] dataSign,byte[]SignData,string publicKey)
        {
            //--CREATE OBJEK DSACryptoServiceProvider
            using (DSACryptoServiceProvider dsa = new DSACryptoServiceProvider())
            {
                //--SET PUBLIC KEY
                dsa.FromXmlString(publicKey);
                //--VERIFY SIGNATURE
                return dsa.VerifyData(dataSign, SignData);
            }
        }
        //--METHOD UNTUK PENGGUNAAN SING DATA DAN VERIVY DATA 
        static void SignDataManager()
        {
            Console.Write("Input your sign : ");
            string dataSign = Console.ReadLine();
            //--CREATE OBJEK DSACryptoServiceProvider UNTUK GENERATE PUBLIK KEY DAN PRIVATE KEY
            using (DSACryptoServiceProvider dsa = new DSACryptoServiceProvider())
            {
                //--CONVERT STRING INPUT KE BYTE ARRAY
                byte[] arrayData = new UnicodeEncoding().GetBytes(dataSign);
                //--GET PUBLIC KEY
                string publicKey = dsa.ToXmlString(false);
                //--GET PRIVATE KEY
                string privateKey = dsa.ToXmlString(true);
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Private Key : " + privateKey);
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Public key : " + publicKey);
                //--MENGAMBIL SIGNATURE YANG DIBUAT DAN PRINT
                byte[] GetSignature = SignData(arrayData, privateKey);
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Signature yang telah di create : " + Convert.ToBase64String(GetSignature));
                Console.WriteLine("-------------------------------------------");
                //--VERIFY SIGNATURE YANG DI BUAT DENGAN YANG ASLI
                if (VerifyData(arrayData, GetSignature, publicKey))
                {
                    Console.WriteLine("This signature was verified.");
                    Console.WriteLine("Please Continue to encrypt this file");
                }
                else
                {
                    Console.WriteLine("This signature was verified.");
                    Console.WriteLine("Please re-send document");
                }
            }
        }
        #endregion

        #region CREATE SIGNATURE
        //--METHOD UNTUK CREATE SIGNATURE
        static byte[]CreateSignature(byte[]dataSign,string privateKey)
        {
            byte[] result = null;
            //--CREATE OBJEK DSACryptoServiceProvider
            using (DSACryptoServiceProvider dsa = new DSACryptoServiceProvider())
            {
                //--SET PRIVATE KEY
                dsa.FromXmlString(privateKey);
                //--CREATE FORMATTER UNTUK MENTRANSFER KEY
                DSASignatureFormatter formatter = new DSASignatureFormatter(dsa);
                //--SETTING ALGORITHM YANG DIPAKAI DENGAN SHA1
                formatter.SetHashAlgorithm("SHA1");
                //--CREATE SIGNATURE
               result= formatter.CreateSignature(dataSign);
            }
            return result;
        }
        //--METHOD UNTUK VERIVIKASI SIGNATURE
        static bool VerifySignature(byte[] dataSign, byte[] SignData, string publicKey)
        {
            //--CREATE OBJEK DSACryptoServiceProvider
            using (DSACryptoServiceProvider dsa = new DSACryptoServiceProvider())
            {
                //--SET PUBLIC KEY
                dsa.FromXmlString(publicKey);
                //--CREATE DEFORMATTER UNTUK MENTRANSFER KEY
                DSASignatureDeformatter deformatter = new DSASignatureDeformatter(dsa);
                //--SETTING HASH ALGORITHM UNTUK SHA1
                deformatter.SetHashAlgorithm("SHA1");
                //--VERIFY SIGNATURE
                return deformatter.VerifySignature(dataSign, SignData);
            }
        }
        
        //--METHOD UNTUK PENGGUNAAN CREATE SIGNATURE DAN VERIFY
        static void CreateSignatureManager()
        {
            using (DSACryptoServiceProvider dsa = new DSACryptoServiceProvider())
            {
                string publicKey = dsa.ToXmlString(false);
                string privateKey = dsa.ToXmlString(true);
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Private Key : " + privateKey);
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Public key : " + publicKey);
                //--MEMBUAT BYTE ARRAY DENGAN MAKSIMUM LENGTH 20 UNTUK SHA1
                byte[] arraySha = { 2, 1, 4, 7, 44, 23, 66, 22, 45, 67, 78, 90, 12, 43, 67, 89, 09, 35, 67, 12 };
                //--CREATE SIGNATURE UNTUK BYTE ARRAY DI ATAS DAN DAPATKAN SINGNATURE YANG DI CREATE
                byte[] GetCreateSignature = CreateSignature(arraySha, privateKey);
                Console.WriteLine("-------------------------------------------");
                
                Console.WriteLine("Signature yang telah di create : " + Convert.ToBase64String(GetCreateSignature));
                Console.WriteLine("-------------------------------------------");
                //--VERIFY SIGNATURE YANG DI CREATE DENGAN YANG DI BUAT
                if (VerifySignature(arraySha, GetCreateSignature, publicKey))
                {
                    Console.WriteLine("This signature was verified.");
                    Console.WriteLine("Please Continue to encrypt this file");
                }
                else
                {
                    Console.WriteLine("This signature was verified.");
                    Console.WriteLine("Please re-send document");
                }
            }
        }
        #endregion
        static void Main(string[] args)
        {
           // SignDataManager();
            CreateSignatureManager();
        }
    }
}
