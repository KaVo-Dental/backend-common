using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Common
{

	public static class HashHelper
	{
		public static string ComputeHash(string pin, string salt)
		{
			if (string.IsNullOrEmpty(pin) || string.IsNullOrEmpty(salt))
				return "";

			string combined = pin + salt; //pin+salt or salt+pin

			using (SHA256 sha256 = SHA256.Create())
			{
				byte[] bytes = Encoding.UTF8.GetBytes(combined);
				byte[] hashBytes = sha256.ComputeHash(bytes);

				//Rückgabe als Base64
				return Convert.ToBase64String(hashBytes);
			}
		}
	}



	public static class StringCipher
	{
		public static string pw = "secr3t9271834126378" + "19623687nmacfbsjkldfsb";

		public static string Encrypt(string clearText)
		{
			string EncryptionKey = pw;
			byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x48, 0x75, 0x62, 0x6f, 0x22, 0x4a, 0x63, 0x61, 0x06, 0x56, 0x46, 0x11, 0xaf }, 100_000, HashAlgorithmName.SHA256);

				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					clearText = Convert.ToBase64String(ms.ToArray());
				}
			}
			return clearText;
		}
		public static string Decrypt(string cipherText)
		{
			string EncryptionKey = pw;
			cipherText = cipherText.Replace(" ", "+");
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x48, 0x75, 0x62, 0x6f, 0x22, 0x4a, 0x63, 0x61, 0x06, 0x56, 0x46, 0x11, 0xaf }, 100_000, HashAlgorithmName.SHA256);
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cs.Write(cipherBytes, 0, cipherBytes.Length);
						cs.Close();
					}
					cipherText = Encoding.Unicode.GetString(ms.ToArray());
				}
			}
			return cipherText;
		}

	}
}
