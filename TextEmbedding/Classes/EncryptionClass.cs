using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TextEmbedding
{
    class EncryptionClass
    {
        private static string Key = "aljwKDOj4n5k$%jfdapowencvertgxzj"; //32 CHARACTERS //Randomize this if you want a different crypto
        private static string IV = "oapvnJHEj3#5j!kf"; //16 CHARACTERS //Randomize this if you want a different crypto
        private static string return_data = "";

        public static string Encrypt(string text)
        {
            try
            {
                byte[] plaintextbytes = System.Text.ASCIIEncoding.ASCII.GetBytes(text);

                using (Aes aes = Aes.Create())
                {
                    aes.BlockSize = 128;
                    aes.KeySize = 256;
                    aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(Key);
                    aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV);
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Mode = CipherMode.CBC;
                    ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);
                    byte[] encrypted = crypto.TransformFinalBlock(plaintextbytes, 0, plaintextbytes.Length);
                    crypto.Dispose();

                    return_data = Convert.ToBase64String(encrypted);
                }

                return return_data;
            }

            catch (Exception ex)
            {             
                return return_data;
            }

            finally
            {
                return_data = "";
            }
        }

        public static string Decrypt(string encrypted)
        {
            try
            {
                byte[] encryptedbytes = Convert.FromBase64String(encrypted);

                using (Aes aes = Aes.Create())
                {
                    aes.BlockSize = 128;
                    aes.KeySize = 256;
                    aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(Key);
                    aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV);
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Mode = CipherMode.CBC;
                    ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
                    byte[] secret = crypto.TransformFinalBlock(encryptedbytes, 0, encryptedbytes.Length);
                    crypto.Dispose();

                    return_data = System.Text.ASCIIEncoding.ASCII.GetString(secret);
                }

                return return_data;
            }

            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());                
                return return_data;
            }

            finally
            {
                return_data = "";
            }
        }
    }
}