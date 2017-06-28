using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace RelatedRows.Domain
{
    public class Flat
    {
        public static string Inflate(string data, string level)
        {
            AesManaged aes = null;
            MemoryStream memoryStream = null;
            CryptoStream cryptoStream = null;
            try
            {
                //Generate a Key based on a Password and HMACSHA1 pseudo-random number generator
                //Salt must be at least 8 bytes long
                //Use an iteration count of at least 1000
                Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(level.Split(new char[] { '|' })[0], Encoding.UTF8.GetBytes(level.Split(new char[] { '|' })[1]), 10000);

                //Create AES algorithm
                aes = new AesManaged();
                //Key derived from byte array with 32 pseudo-random key bytes
                aes.Key = rfc2898.GetBytes(32);
                //IV derived from byte array with 16 pseudo-random key bytes
                aes.IV = rfc2898.GetBytes(16);

                //Create Memory and Crypto Streams
                memoryStream = new MemoryStream();
                cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

                //Encrypt Data
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                cryptoStream.Write(dataBytes, 0, dataBytes.Length);
                cryptoStream.FlushFinalBlock();

                //Return Base 64 String
                return Convert.ToBase64String(memoryStream.ToArray());
            }
            finally
            {
                if (cryptoStream != null)
                    cryptoStream.Close();

                if (memoryStream != null)
                    memoryStream.Close();

                if (aes != null)
                    aes.Clear();
            }
        }


        public static string Deflate(string data, string level)
        {
            AesManaged aes = null;
            MemoryStream memoryStream = null;

            try
            {
                //Generate a Key based on a Password and HMACSHA1 pseudo-random number generator
                //Salt must be at least 8 bytes long
                //Use an iteration count of at least 1000
                Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(level.Split(new char[] { '|' })[0], Encoding.UTF8.GetBytes(level.Split(new char[] { '|' })[1]), 10000);

                //Create AES algorithm
                aes = new AesManaged();
                //Key derived from byte array with 32 pseudo-random key bytes
                aes.Key = rfc2898.GetBytes(32);
                //IV derived from byte array with 16 pseudo-random key bytes
                aes.IV = rfc2898.GetBytes(16);

                //Create Memory and Crypto Streams
                memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

                //Decrypt Data
                byte[] dataBytes = Convert.FromBase64String(data);
                cryptoStream.Write(dataBytes, 0, dataBytes.Length);
                cryptoStream.FlushFinalBlock();

                //Return Decrypted String
                byte[] decryptBytes = memoryStream.ToArray();

                //Dispose
                if (cryptoStream != null)
                    cryptoStream.Dispose();

                //Retval
                return Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
            }
            finally
            {
                if (memoryStream != null)
                    memoryStream.Dispose();

                if (aes != null)
                    aes.Clear();
            }
        }
    }
}
