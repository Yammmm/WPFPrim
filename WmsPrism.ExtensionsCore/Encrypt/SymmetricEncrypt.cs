using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Extensions.Encrypt
{
	public class SymmetricEncrypt
	{
		private SymmetricEncryptType mbyteEncryptionType;
		private System.Security.Cryptography.SymmetricAlgorithm mCSP;
		private string mEncryptedString;
		private string mOriginalString;
		public System.Security.Cryptography.SymmetricAlgorithm CryptoProvider
		{
			get
			{
				return this.mCSP;
			}
			set
			{
				this.mCSP = value;
			}
		}
		public string EncryptedString
		{
			get
			{
				return this.mEncryptedString;
			}
			set
			{
				this.mEncryptedString = value;
			}
		}
		public SymmetricEncryptType EncryptionType
		{
			get
			{
				return this.mbyteEncryptionType;
			}
			set
			{
				if (this.mbyteEncryptionType != value)
				{
					this.mbyteEncryptionType = value;
					this.mOriginalString = string.Empty;
					this.mEncryptedString = string.Empty;
					this.SetEncryptor();
				}
			}
		}
		public byte[] IV
		{
			get
			{
				return this.mCSP.IV;
			}
			set
			{
				this.mCSP.IV = value;
			}
		}
		public string IVString
		{
			get
			{
				return System.Convert.ToBase64String(this.mCSP.IV);
			}
			set
			{
				this.mCSP.IV = System.Convert.FromBase64String(value);
			}
		}
		public byte[] key
		{
			get
			{
				return this.mCSP.Key;
			}
			set
			{
				this.mCSP.Key = value;
			}
		}
		public string KeyString
		{
			get
			{
				return System.Convert.ToBase64String(this.mCSP.Key);
			}
			set
			{
				this.mCSP.Key = System.Convert.FromBase64String(value);
			}
		}
		public string OriginalString
		{
			get
			{
				return this.mOriginalString;
			}
			set
			{
				this.mOriginalString = value;
			}
		}
		public SymmetricEncrypt()
		{
			this.mbyteEncryptionType = SymmetricEncryptType.DES;
			this.SetEncryptor();
		}
		public SymmetricEncrypt(SymmetricEncryptType encryptionType)
		{
			this.mbyteEncryptionType = encryptionType;
			this.SetEncryptor();
		}
		public SymmetricEncrypt(SymmetricEncryptType encryptionType, string originalString)
		{
			this.mbyteEncryptionType = encryptionType;
			this.mOriginalString = originalString;
			this.SetEncryptor();
		}
		public string Decrypt()
		{
			System.Security.Cryptography.ICryptoTransform transform = this.mCSP.CreateDecryptor(this.mCSP.Key, this.mCSP.IV);
			byte[] buffer = System.Convert.FromBase64String(this.mEncryptedString);
			System.IO.MemoryStream stream = new System.IO.MemoryStream();
			System.Security.Cryptography.CryptoStream stream2 = new System.Security.Cryptography.CryptoStream(stream, transform, System.Security.Cryptography.CryptoStreamMode.Write);
			stream2.Write(buffer, 0, buffer.Length);
			stream2.FlushFinalBlock();
			stream2.Close();
			this.mOriginalString = System.Text.Encoding.Unicode.GetString(stream.ToArray());
			return this.mOriginalString;
		}
		public string Decrypt(string encryptedString)
		{
			this.mEncryptedString = encryptedString;
			return this.Decrypt();
		}
		public string Decrypt(string encryptedString, SymmetricEncryptType encryptionType)
		{
			this.mEncryptedString = encryptedString;
			this.mbyteEncryptionType = encryptionType;
			return this.Decrypt();
		}
		public string Encrypt()
		{
			string empty;
			if (string.IsNullOrEmpty(this.mOriginalString))
			{
				empty = string.Empty;
			}
			else
			{
				System.Security.Cryptography.ICryptoTransform transform = this.mCSP.CreateEncryptor(this.mCSP.Key, this.mCSP.IV);
				byte[] bytes = System.Text.Encoding.Unicode.GetBytes(this.mOriginalString);
				System.IO.MemoryStream stream = new System.IO.MemoryStream();
				System.Security.Cryptography.CryptoStream stream2 = new System.Security.Cryptography.CryptoStream(stream, transform, System.Security.Cryptography.CryptoStreamMode.Write);
				stream2.Write(bytes, 0, bytes.Length);
				stream2.FlushFinalBlock();
				stream2.Close();
				this.mEncryptedString = System.Convert.ToBase64String(stream.ToArray());
				empty = this.mEncryptedString;
			}
			return empty;
		}
		public string Encrypt(string originalString)
		{
			this.mOriginalString = originalString;
			return this.Encrypt();
		}
		public string Encrypt(string originalString, SymmetricEncryptType encryptionType)
		{
			this.mOriginalString = originalString;
			this.mbyteEncryptionType = encryptionType;
			return this.Encrypt();
		}
		public string GenerateIV()
		{
			this.mCSP.GenerateIV();
			return System.Convert.ToBase64String(this.mCSP.IV);
		}
		public string GenerateKey()
		{
			this.mCSP.GenerateKey();
			return System.Convert.ToBase64String(this.mCSP.Key);
		}
		private void SetEncryptor()
		{
			switch (this.mbyteEncryptionType)
			{
				case SymmetricEncryptType.DES:
					{
						this.mCSP = new System.Security.Cryptography.DESCryptoServiceProvider();
						break;
					}
				case SymmetricEncryptType.RC2:
					{
						this.mCSP = new System.Security.Cryptography.RC2CryptoServiceProvider();
						break;
					}
				case SymmetricEncryptType.Rijndael:
					{
						this.mCSP = new System.Security.Cryptography.RijndaelManaged();
						break;
					}
				case SymmetricEncryptType.TripleDES:
					{
						this.mCSP = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
						break;
					}
			}
			this.mCSP.GenerateKey();
			this.mCSP.GenerateIV();
		}
	}
}
