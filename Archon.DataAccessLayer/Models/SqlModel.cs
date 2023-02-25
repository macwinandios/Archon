using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Archon.DataAccessLayer.Models
{
    public class SqlModel
    {
        static SqlModel()
        {
            SqlConnection = new SqlConnection(SqlConnectionString);
        }

        public static string HashedString(string _stringToHash)
        {
            SHA1CryptoServiceProvider _sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();

            byte[] _stringBytes = Encoding.ASCII.GetBytes(_stringToHash);
            byte[] _encryptedBytes = _sHA1CryptoServiceProvider.ComputeHash(_stringBytes);
            return Convert.ToBase64String(_encryptedBytes);
        }
        public static string ServerName { get; set; } = "archonsqldatabase.database.windows.net";
        public static string DatabaseName { get; set; } = "ArchonAzure";
        public static string ServerUsername { get; set; } = "Johnny";
        public static string ServerPassword { get; set; } = "Jan2023!";
        public static string SqlConnectionString { get; set; } = $"Data Source = {ServerName}; Initial Catalog = {DatabaseName}; Persist Security Info = True; User ID = {ServerUsername}; Password = {ServerPassword}";
        
        public static SqlConnection SqlConnection { get; set; }


    }
}

