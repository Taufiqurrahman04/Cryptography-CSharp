using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HashingCSharp
{
    class Program
    {
        //--METHOD UNTUK MENDAPATKAN ENCRYPT TEXT DENGAN MENGGUNAKAN HASH ALGORITHM IN C#
        public static Tuple<string,string> GetHashMethode(HashAlgorithm cipher,string input)
        {
            //--UNTUK HASHING PLANTTEXT(STRING INPUT) MAKA STRING TERSEBUT DI UBAH KE BETUK BYTE ARRAY
            byte[] inputInByteArray = Encoding.UTF8.GetBytes(input);

            //--MEMANGGIL METHOD COMPUTE HASH UNTUK MENDAPATKAN ENCRYPT TEXT DARI INPUT, YANG KELUARANNYA BERUPA BYTE ARRAY
            byte[] cipherText = cipher.ComputeHash(inputInByteArray);

            //--CREATE STRING BUILDER MENYIMPAN NILAI ASLI ENCRYPAN YANG TERSIMPAN DIDALAM BYTE ARRAY

            StringBuilder getByteArray = new StringBuilder();
            getByteArray.Clear();
            foreach(var item in cipherText)
            {
                //--GET ITEM DI DALAM BYTE ARRAY DAN DISIMPAN KE STRING BUILDER
                getByteArray.Append(item);
            }
            //--CONVERT KE STRING BASE 64 UNTUK MENDAPATKAN TEXT ENCRYPAN DALAM STRING BASE 64
            return new Tuple<string, string>(getByteArray.ToString(), Convert.ToBase64String(cipherText));
        }
        //--PRINT OUTPUT UNTUK DITAMPILKAN DI CONSOLE
        private static void PrintOutput(Tuple<string,string> output,string cipher)
        {
            Console.WriteLine("--------------------Menggunakan Algorithm : "+cipher);
            Console.WriteLine("Read data dari byte Array                                  : " + output.Item1);
            Console.WriteLine("Read data dari byte Array yang di convert ke string base-64 : " + output.Item2);
        }
        static void Main(string[] args)
        {
            //--INPUT STRING
            Console.Write("Input Text To encrypt : ");
            string input = Console.ReadLine();

            //--MD5 ALGORITHM
            MD5 mD5 = MD5.Create();
            PrintOutput(GetHashMethode(mD5, input), "MD5");
            //--RIPEMD160 ALGORIHM
            RIPEMD160 rIPEMD160 = RIPEMD160.Create();
            PrintOutput(GetHashMethode(rIPEMD160, input), "RIPEMD160");
            //--SHA1 ALGORITHM
            SHA1 sHA1 = SHA1.Create();
            PrintOutput(GetHashMethode(sHA1, input), "SHA1");
            //--SHA256 ALGHORITHM
            SHA256 sHA256 = SHA256.Create();
            PrintOutput(GetHashMethode(sHA256, input), "SHA256");
            //--SHA384 ALGORITHM
            SHA384 sHA384 = SHA384.Create();
            PrintOutput(GetHashMethode(sHA384, input), "SHA384");
            //--SHA512 ALGORITHM
            SHA512 sHA512 = SHA512.Create();
            PrintOutput(GetHashMethode(sHA512, input), "SHA512");
        }
    }
}
