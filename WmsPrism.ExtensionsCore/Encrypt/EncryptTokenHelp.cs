using System;
using System.Collections.Generic;
using System.Text;

namespace WmsPrism.Extensions.Encrypt
{
    public static class EncryptTokenHelp
    {
        private static readonly string IVStringForInviteFriend = "dABhAGwAZQA=";
        private static readonly string IVStringForPassword = "YgBhAHMAZQA=";
        private static readonly string KeyStringForInviteFriend = "cABvAG8AaAA=";
        private static readonly string KeyStringForPassword = "cABlAHIAcwBvAG4AcgBlAHMAdQBtAGUA";
		/// <summary>
		/// 解密字符串
		/// </summary>
		/// <param name="encryptType">加密类型</param>
		/// <param name="encryptedString">需加密内容</param>
		/// <param name="ivString"></param>
		/// <param name="keyString"></param>
		/// <returns></returns>
		public static string Decrypt(SymmetricEncryptType encryptType, string encryptedString, string ivString, string keyString)
		{
			return new SymmetricEncrypt(encryptType)
			{
				IVString = ivString,
				KeyString = keyString
			}.Decrypt(encryptedString);
		}
		/// <summary>
		/// 按外部方式解密字符串
		/// </summary>
		/// <param name="encryptedString"></param>
		/// <returns></returns>
		public static string DecryptTokenForInviteFriend(string encryptedString)
		{
			string str = string.Empty;
			try
			{
				str = EncryptTokenHelp.Decrypt(SymmetricEncryptType.DES, encryptedString, EncryptTokenHelp.IVStringForInviteFriend, EncryptTokenHelp.KeyStringForInviteFriend);
			}
			catch
			{
			}
			return str;
		}
		/// <summary>
		/// 按密码方式解密字符串
		/// </summary>
		/// <param name="encryptedString"></param>
		/// <returns></returns>
		public static string DecryptTokenForPassword(string encryptedString)
		{
			return EncryptTokenHelp.Decrypt(SymmetricEncryptType.TripleDES, encryptedString, EncryptTokenHelp.IVStringForPassword, EncryptTokenHelp.KeyStringForPassword);
		}
		/// <summary>
		/// 按验证码方式解密字符串
		/// </summary>
		/// <param name="encryptedString"></param>
		/// <returns></returns>
		public static string DecryptTokenForVerifyCode(string encryptedString)
		{
			string str = string.Empty;
			try
			{
				str = EncryptTokenHelp.Decrypt(SymmetricEncryptType.DES, encryptedString, EncryptTokenHelp.IVStringForInviteFriend, EncryptTokenHelp.KeyStringForInviteFriend);
			}
			catch
			{
			}
			return str;
		}
		/// <summary>
		/// 加密字符串
		/// </summary>
		/// <param name="encryptType">加密类型</param>
		/// <param name="originalString">字符串</param>
		/// <param name="ivString"></param>
		/// <param name="keyString"></param>
		/// <returns></returns>
		private static string Encrypt(SymmetricEncryptType encryptType, string originalString, string ivString, string keyString)
		{
			return new SymmetricEncrypt(encryptType)
			{
				IVString = ivString,
				KeyString = keyString
			}.Encrypt(originalString);
		}
		/// <summary>
		/// 按外部接口方式加密字符串
		/// </summary>
		/// <param name="originalString"></param>
		/// <returns></returns>
		public static string EncryptTokenForInviteFriend(string originalString)
		{
			string str = string.Empty;
			try
			{
				str = EncryptTokenHelp.Encrypt(SymmetricEncryptType.DES, originalString, EncryptTokenHelp.IVStringForInviteFriend, EncryptTokenHelp.KeyStringForInviteFriend);
			}
			catch
			{
			}
			return str;
		}
		/// <summary>
		/// 按密码方式加密字符串
		/// </summary>
		/// <param name="originalString"></param>
		/// <returns></returns>
		public static string EncryptTokenForPassword(string originalString)
		{
			return EncryptTokenHelp.Encrypt(SymmetricEncryptType.TripleDES, originalString, EncryptTokenHelp.IVStringForPassword, EncryptTokenHelp.KeyStringForPassword);
		}
		/// <summary>
		/// 按验证码方式加密字符串
		/// </summary>
		/// <param name="originalString"></param>
		/// <returns></returns>
		public static string EncryptTokenForVerifyCode(string originalString)
		{
			string str = string.Empty;
			try
			{
				str = EncryptTokenHelp.Encrypt(SymmetricEncryptType.DES, originalString, EncryptTokenHelp.IVStringForInviteFriend, EncryptTokenHelp.KeyStringForInviteFriend);
			}
			catch
			{
			}
			return str;
		}
		/// <summary>
		/// Des方式加密
		/// </summary>
		/// <param name="originalString">原始字符串</param>
		/// <returns>如果不为空，则为加密后的字符串</returns>
		public static string DesEncrypt(this string originalString)
		{
			string str = string.Empty;
			try
			{
				str = EncryptTokenHelp.Encrypt(SymmetricEncryptType.DES, originalString, EncryptTokenHelp.IVStringForInviteFriend, EncryptTokenHelp.KeyStringForInviteFriend);
			}
			catch
			{
			}
			return str;
		}
		/// <summary>
		/// Des方式解密
		/// </summary>
		/// <param name="encryptedString">加密后的字符串</param>
		/// <returns>如果不为空，则为解密后的字符串</returns>
		public static string DesDecrypt(this string encryptedString)
		{
			string str = string.Empty;
			try
			{
				str = EncryptTokenHelp.Decrypt(SymmetricEncryptType.DES, encryptedString, EncryptTokenHelp.IVStringForInviteFriend, EncryptTokenHelp.KeyStringForInviteFriend);
			}
			catch
			{
			}
			return str;
		}
		/// <summary>
		/// 获取MD5 32位加密字符串
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string MD5(this string str)
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
			bytes = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(bytes);
			string str2 = "";
			for (int i = 0; i < bytes.Length; i++)
			{
				str2 += bytes[i].ToString("x").PadLeft(2, '0');
			}
			return str2;
		}
		/// <summary>
		/// 获取MD5 16位加密字符串
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string MD5_16(this string str)
		{
			return str.MD5().Substring(8, 16);
		}

	}

	public enum SymmetricEncryptType : byte
	{
		DES,
		RC2,
		Rijndael,
		TripleDES
	}
}
