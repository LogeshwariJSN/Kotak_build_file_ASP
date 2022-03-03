using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KMBL
{
    namespace StepupAuthentication
    {
        namespace CoreComponents
        {
            public class KotakCryptography
            {
                // Encrypt Function
                public string Encrypt(string input, string password)
                {
                    // Getting the bytes of the string
                    byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                    // Hashing the password with SHA256
                    passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                    byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

                    string result = Convert.ToBase64String(bytesEncrypted);

                    return result;
                }

                // Decrypt Function
                public string Decrypt(string input, string password)
                {
                    // Getting the bytes of the string
                    byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    passwordBytes = SHA256.Create().ComputeHash(passwordBytes);


                    byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

                    string result = Encoding.UTF8.GetString(bytesDecrypted);


                    return result;
                }

                // Core Encrypt Function
                public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
                {
                    byte[] encryptedBytes = null;

                    // Setting salt value, should be atleast 8 bytes.
                    byte[] saltBytes = Encoding.UTF8.GetBytes("kaluppu is a rock salt");

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (AesManaged AES = new AesManaged())
                        {
                            AES.KeySize = 256;
                            AES.BlockSize = 128;

                            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                            AES.Key = key.GetBytes(AES.KeySize / 8);
                            AES.IV = key.GetBytes(AES.BlockSize / 8);

                            AES.Mode = CipherMode.CBC;

                            using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                                cs.Close();
                            }
                            encryptedBytes = ms.ToArray();
                        }
                    }

                    return encryptedBytes;
                }

                // Core Decrypt Function
                public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
                {
                    byte[] decryptedBytes = null;

                    // Set your salt here, change it to meet your flavor:
                    // The salt bytes must be at least 8 bytes.
                    byte[] saltBytes = Encoding.UTF8.GetBytes("kaluppu is a rock salt");

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (AesManaged AES = new AesManaged())
                        {
                            AES.KeySize = 256;
                            AES.BlockSize = 128;

                            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                            AES.Key = key.GetBytes(AES.KeySize / 8);
                            AES.IV = key.GetBytes(AES.BlockSize / 8);

                            AES.Mode = CipherMode.CBC;

                            using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                                cs.Close();
                            }
                            decryptedBytes = ms.ToArray();
                        }
                    }

                    return decryptedBytes;
                }

            }
        }
    }
}
