using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BusinessProjectTracking.Helper
{
	public class CABusinessProjectTrackingCrypt
	{

		private enum EncryptMode { ENCRYPT, DECRYPT };
		public static string getHashSha256(string text, int length)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			SHA256Managed hashstring = new SHA256Managed();
			byte[] hash = hashstring.ComputeHash(bytes);
			string hashString = string.Empty;
			foreach (byte x in hash)
			{
				hashString += String.Format("{0:x2}", x);//convert to hex string 
			}
			if (length > hashString.Length)
				return hashString;
			else
				return hashString.Substring(0, length);
		}

		public static string EncryptString(string _plainText)
		{
			string _key = string.Empty;
			string _initVector = string.Empty;

			if (!string.IsNullOrEmpty(_plainText))
			{
				_initVector = ConfigurationManager.AppSettings["AuthKey16Digit"].ToString();
				_key = getHashSha256(ConfigurationManager.AppSettings["SecretKey"].ToString(), 32);

				return encryptDecrypt(_plainText, _key, EncryptMode.ENCRYPT, _initVector);
			}
			return ""; 
		}

		private static string encryptDecrypt(string _inputText, string _encryptionKey, EncryptMode _mode, string _initVector)
		{

			string _out = "";// output string
							 //_encryptionKey = MD5Hash (_encryptionKey);

			using (RijndaelManaged _rcipher = GetProvider(_encryptionKey, _initVector))
			{
				UTF8Encoding _enc = new UTF8Encoding();
				try
				{
					if (_mode.Equals(EncryptMode.ENCRYPT))
					{
						//encrypt
						byte[] plainText = _rcipher.CreateEncryptor().TransformFinalBlock(_enc.GetBytes(_inputText), 0, _inputText.Length);
						_out = Convert.ToBase64String(plainText);
					}
					if (_mode.Equals(EncryptMode.DECRYPT))
					{
						//decrypt
						byte[] plainText = _rcipher.CreateDecryptor().TransformFinalBlock(Convert.FromBase64String(_inputText), 0, Convert.FromBase64String(_inputText).Length);
						_out = _enc.GetString(plainText);
					}
				}
				catch (Exception ex) { throw; }
				finally { _rcipher.Dispose(); }
			}

			return _out;// return encrypted/decrypted string
		}

		private static RijndaelManaged GetProvider(string _encryptionKey, string _initVector)
		{
			byte[] _key, _pwd, _ivBytes, _iv;

			RijndaelManaged _rcipher = new RijndaelManaged();

			try
			{
				_rcipher.Mode = CipherMode.CBC;
				_rcipher.Padding = PaddingMode.PKCS7;
				_rcipher.KeySize = 256;
				_rcipher.BlockSize = 128;

				_key = new byte[32];
				_iv = new byte[_rcipher.BlockSize / 8]; //128 bit / 8 = 16 bytes
				_ivBytes = new byte[16];

				_pwd = Encoding.UTF8.GetBytes(_encryptionKey);
				_ivBytes = Encoding.UTF8.GetBytes(_initVector);

				int len = _pwd.Length;
				if (len > _key.Length)
				{
					len = _key.Length;
				}
				int ivLenth = _ivBytes.Length;
				if (ivLenth > _iv.Length)
				{
					ivLenth = _iv.Length;
				}

				Array.Copy(_pwd, _key, len);
				Array.Copy(_ivBytes, _iv, ivLenth);
				_rcipher.Key = _key;
				_rcipher.IV = _iv;

			}
			catch (Exception ex) { throw; }
			return _rcipher;
		}

	}
}