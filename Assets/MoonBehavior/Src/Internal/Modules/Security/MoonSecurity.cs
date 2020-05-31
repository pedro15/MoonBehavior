using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace MoonBehavior.Security
{
    /// <summary>
    /// Security class to safe send data
    /// </summary>
    public static class MoonSecurity
    {
        /// <summary>
        /// AES-256 Key (you can reemplaze it with your own )
        /// </summary>
        private const string PassKey = "6595330E877E72A04313E2CA9932AC1F";

        /// <summary>
        /// AES-256 Salt (you can reemplaze it with your own )
        /// </summary>
        private const string SaltKey = "BA5CD1DB040B6B24";
        
        /// <summary>
        /// Encrypts a byte array data with AES encryption
        /// </summary>
        /// <param name="bytesToBeEncrypted">Byte array data</param>
        /// <returns>Encrypted data</returns>
        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted )
        {
            byte[] encryptedBytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    byte[] salt = Encoding.ASCII.GetBytes(SaltKey);
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(PassKey, salt);
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

        /// <summary>
        /// Decrypts a byte array data previusly encrypted with AES encryption
        /// </summary>
        /// <param name="bytesToBeDecrypted">byte data</param>
        /// <returns>Decrypted data</returns>
        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted)
        {
            byte[] decryptedBytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    byte[] salt = Encoding.ASCII.GetBytes(SaltKey);
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(PassKey, salt);
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