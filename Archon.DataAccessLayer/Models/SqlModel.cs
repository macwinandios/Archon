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
            ClientQueryCommand = new SqlCommand(ClientQuery, SqlConnection);
            CaseQueryCommand = new SqlCommand(CaseQuery, SqlConnection);
            ReportQueryCommand = new SqlCommand(ReportQuery, SqlConnection);
        }

        public static string HashedString(string _stringToHash)
        {
            SHA1CryptoServiceProvider _sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();

            byte[] _stringBytes = Encoding.ASCII.GetBytes(_stringToHash);
            byte[] _encryptedBytes = _sHA1CryptoServiceProvider.ComputeHash(_stringBytes);
            return Convert.ToBase64String(_encryptedBytes);
        }
        public static string ServerName { get; set; } = "172.19.208.1";
        public static string DatabaseName { get; set; } = "Archon";
        //public static string DatabaseName { get; set; } = "CompanyDB";
        public static string ServerUsername { get; set; } = "Johnny";
        public static string ServerPassword { get; set; } = "Jan2023";
        //public static string LoginQuery { get; set; } = "Select * from dbo.Login";
        //public static string LoginCreate { get; set; } = "INSERT INTO dbo.Login VALUES (@Id, @Username, @Password)";


        public static string SqlConnectionString { get; set; } = $"Data Source = {ServerName}; Initial Catalog = {DatabaseName}; Persist Security Info = True; User ID = {ServerUsername}; Password = {ServerPassword}";
        public static string ClientQuery { get; set; } = "Select * from dbo.ClientInfoTable";
        public static string CaseQuery { get; set; } = "Select * from dbo.CaseInfoTable";
        public static string ReportQuery { get; set; } = "Select * from dbo.ReportInfoTable";
        public static string AssignedCasesQuery { get; set; } = "Select * from dbo.AssignedCasesInfoTable";

        public static string ClientInsertQuery { get; set; } = "INSERT INTO dbo.ClientInfoTable VALUES (@ClientName, @ClientEmail, @ClientPhoneNumber, @ClientAddress, @ClientMainContactName, @ClientMainContactPhoneNumber, @PricePerHour, @NumberOfHoursBilledToDate, @TotalAmountBilledToDate, @TotalPaidToDate, @BalanceDue, @ClientSince, @SelectedYesNoIDontKnowPicker)";
        public static string CaseInsertQuery { get; set; } = "INSERT INTO dbo.CaseInfoTable VALUES (@CaseId, @ClientName, @FirstName, @MiddleName, @LastName, @DateOfBirth, @PhoneNumber, @HomeAddress, @HomeDescription, @WorkAddress, @WorkDescription, @Height, @Weight, @EyeColor, @HairColor, @SelectedComplexion, @TattooDescription, @SelectedEthnicity, @SelectedLanguage, @SelectedAwareness, @FrequentedPlaces, @VehicleYear, @VehicleMake, @VehicleModel, @VehicleColor, @LicensePlateNumber, @LicensePlateState, @LicensePlateColor, @CaseDetails, @DateCaseInitialized, @DateOfSurveillance)";
        public static string ReportInsertQuery { get; set; } = "INSERT INTO dbo.ReportInfoTable VALUES (@CaseId, @ClientName, @StartTime, @EndTime, @StartLocation, @EndLocation, @SubjectVideoObtained, @MedicalDevicesUsed, @AddressWhereVideoWasObtained, @VehiclesPresentAtStartLocation, @NumberOfVehiclesAtStartLocation, @StartLocationIsSubjectsResidence, @AttemptedToConfirmSubjectWasHome, @SurveillancePosition,  @SubjectSex, @ApproximateHeight, @ApproximateWeight, @ApproximateBuild, @HairColor, @HairLength, @WhyWasSurveillanceEnded, @SurveillanceSummary, @VideoObtainedDetails, @VideoObtainedTime, @TypeOfVideoObtained,@SurveillanceDate, @TotalHoursOfSurveillance)";

        

        public static SqlConnection SqlConnection { get; set; }
        public static SqlDataReader SqlDataReader { get; set; }
        public static SqlDataReader ExecuteReader { get; set; }
        public static SqlCommand ClientQueryCommand { get; set; }
        public static SqlCommand CaseQueryCommand { get; set; }
        public static SqlCommand ReportQueryCommand { get; set; }


    }
}

