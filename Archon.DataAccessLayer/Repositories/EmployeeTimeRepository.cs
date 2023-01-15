using Archon.DataAccessLayer.Models;
using Archon.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Archon.DataAccessLayer.Repositories
{
    public class EmployeeTimeRepository : IEmployeeTimeRepository<IEmployeeTimeViewModel>, IPostRepository<IEmployeeTimeViewModel>, IGetRepository<IEmployeeTimeViewModel>
    {
        public async Task ClockInAsync(IEmployeeTimeViewModel viewModel)
        {
            try
            {
                viewModel.DateClockedIn = DateTime.Today;
                viewModel.ClockedInAt = DateTime.Now;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                SqlModel.SqlConnection.Close();
            }
        }
        //ADMINMONITORPAY WILL USE THE FOLLOWING TWO GETALLASYNC AND GETBYIDORUSERNAME
        public Task<IEnumerable<IEmployeeTimeViewModel>> GetAllAsync(IEmployeeTimeViewModel viewModel)
        {
            throw new NotImplementedException();

        }
        
        public Task GetByIdOrUsername(IEmployeeTimeViewModel viewModel, int id)
        {
            throw new NotImplementedException();
        }
        public async Task GetByIdOrUsername(IEmployeeTimeViewModel viewModel, string username)
        {
            try
            {
                SqlModel.SqlConnection.Open();

                SqlCommand clientCommand = new SqlCommand("SELECT * FROM dbo.EmployeeTime WHERE Username = @Username", SqlModel.SqlConnection);
                clientCommand.Parameters.AddWithValue("@Username", username);

                SqlDataReader clientReader = clientCommand.ExecuteReader();

                if (clientReader.Read())
                {
                    viewModel.Username = clientReader["Username"].ToString();
                    SqlModel.SqlConnection.Close();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("USERNAME NOT FOUND", "IN DATABASE", "OK");
                    SqlModel.SqlConnection.Close();
                }

                clientReader.Close();
                SqlModel.SqlConnection.Close();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("NOT YET", ex.Message, "OK");
                SqlModel.SqlConnection.Close();
            }
            finally
            {
                SqlModel.SqlConnection.Close();
            }
        }

        


        public async Task PostAsync(IEmployeeTimeViewModel viewModel)
        {


            if (viewModel.DateClockedOut != null && DateTime.Today > viewModel.DateClockedOut)
            {
                viewModel.TotalTimeClockedInToday = TimeSpan.Zero;
            }

            if (viewModel.DateClockedOut != null && DateTime.Today > viewModel.DateClockedOut.AddDays(7))
            {
                viewModel.TotalTimeClockedInThisWeek = TimeSpan.Zero;
            }


            try
            {
                viewModel.ClockedOutAt = DateTime.Now;
                await SqlModel.SqlConnection.OpenAsync();
                using (var command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = @"SELECT CAST([TotalTimeClockedInThisWeek] AS time) FROM [dbo].[EmployeeTime] WHERE [Username] = @Username AND [DateClockedIn] = @DateClockedIn";
                    command.Parameters.AddWithValue("@Username", viewModel.Username);
                    command.Parameters.AddWithValue("@DateClockedIn", DateTime.Today);
                    var totalTimeClockedInThisWeekResult = await command.ExecuteScalarAsync();
                    viewModel.TotalTimeClockedInThisWeek = totalTimeClockedInThisWeekResult == null ? viewModel.TotalTimeClockedInThisWeek = viewModel.TotalTimeClockedInToday : (TimeSpan)totalTimeClockedInThisWeekResult + viewModel.TotalTimeClockedInToday;
                }
                using (var command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = @"SELECT CAST([TotalTimeClockedInToday] AS time) FROM [dbo].[EmployeeTime] WHERE [Username] = @Username AND [DateClockedIn] = @DateClockedIn";
                    command.Parameters.AddWithValue("@Username", viewModel.Username);
                    command.Parameters.AddWithValue("@DateClockedIn", DateTime.Today);
                    var totalTimeClockedInTodayResult = await command.ExecuteScalarAsync();
                    viewModel.TotalTimeClockedInToday = totalTimeClockedInTodayResult == null ? viewModel.TotalTimeClockedInToday = viewModel.DurationOfClockIn : (TimeSpan)totalTimeClockedInTodayResult + viewModel.DurationOfClockIn;
                    
                }
                

                using (var command = SqlModel.SqlConnection.CreateCommand())
                {

                    command.CommandText = @"INSERT INTO [dbo].[EmployeeTime] ([Username],[DateClockedIn], [ClockedInAt], [DateClockedOut], [ClockedOutAt], [DurationOfClockIn], [TotalTimeClockedInToday], [TotalTimeClockedInThisWeek])
                           VALUES (@Username, @DateClockedIn, @ClockedInAt,@DateClockedOut, @ClockedOutAt, @DurationOfClockIn, @TotalTimeClockedInToday, @TotalTimeClockedInThisWeek)";
                    command.Parameters.AddWithValue("@Username", viewModel.Username);
                    command.Parameters.AddWithValue("@DateClockedIn", viewModel.DateClockedIn.ToShortDateString());
                    command.Parameters.AddWithValue("@ClockedInAt", viewModel.ClockedInAt.ToShortTimeString());
                    command.Parameters.AddWithValue("@DateClockedOut", viewModel.DateClockedOut.ToShortDateString());
                    command.Parameters.AddWithValue("@ClockedOutAt", viewModel.ClockedOutAt.ToShortTimeString());
                    command.Parameters.AddWithValue("@DurationOfClockIn", viewModel.DurationOfClockIn.ToString());
                    command.Parameters.AddWithValue("@TotalTimeClockedInToday", viewModel.TotalTimeClockedInToday.ToString());
                    command.Parameters.AddWithValue("@TotalTimeClockedInThisWeek", viewModel.TotalTimeClockedInThisWeek.ToString());

                    await command.ExecuteNonQueryAsync();
                }


                SqlModel.SqlConnection.Close();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }











        //
        ////
        //////
        /////////
        //ONLY ADMIN CAN UPDATE
        //using (var command = SqlModel.SqlConnection.CreateCommand())
        //{
        //    command.CommandText = @"UPDATE [dbo].[EmployeeTime]
        //                   SET [DateClockedIn] = @DateClockedIn, [ClockedInAt] = @ClockedInAt,
        //                       [DateClockedOut] = @DateClockedOut, [ClockedOutAt] = @ClockedOutAt,
        //                       [DurationOfClockIn] = @DurationOfClockIn,
        //                       [TotalTimeClockedInToday] = @TotalTimeClockedInToday,
        //                       [TotalTimeClockedInThisWeek] = @TotalTimeClockedInThisWeek
        //                   WHERE [Username] = @Username";
        //    command.Parameters.AddWithValue("@DateClockedIn", viewModel.DateClockedIn.ToShortDateString());
        //    command.Parameters.AddWithValue("@ClockedInAt", viewModel.ClockedInAt.ToShortTimeString());
        //    command.Parameters.AddWithValue("@DateClockedOut", viewModel.DateClockedOut.ToShortDateString());
        //    command.Parameters.AddWithValue("@ClockedOutAt", viewModel.ClockedOutAt.ToShortTimeString());
        //    command.Parameters.AddWithValue("@DurationOfClockIn", viewModel.DurationOfClockIn.ToString());
        //    command.Parameters.AddWithValue("@TotalTimeClockedInToday", viewModel.TotalTimeClockedInToday.ToString());
        //    command.Parameters.AddWithValue("@TotalTimeClockedInThisWeek", viewModel.TotalTimeClockedInThisWeek.ToString());
        //    command.Parameters.AddWithValue("@Username", viewModel.Username);

        //    await command.ExecuteNonQueryAsync();
        //}
        /////////
        //////
        ////
        //
        //command.CommandText = @"INSERT INTO [dbo].[EmployeeTime] (
        ////                            [Username],
        ////                            [DateClockedIn],
        ////                            [ClockedInAt],
        ////                            [DateClockedOut],
        ////                            [ClockedOutAt],
        ////                            [DurationOfClockIn],
        ////                            [TotalTimeClockedInToday],
        ////                            [TotalTimeClockedInThisWeek])
        ////                        VALUES (
        ////                            @Username,
        ////                            @DateClockedIn,
        ////                            @ClockedInAt,
        ////                            @DateClockedOut,
        ////                            @ClockedOutAt,
        ////                            @DurationOfClockIn,
        ////                            @TotalTimeClockedInToday,
        ////                            @TotalTimeClockedInThisWeek)";
        //            command.Parameters.AddWithValue("@DateClockedIn", viewModel.DateClockedIn.ToShortDateString());
        //            command.Parameters.AddWithValue("@ClockedInAt", viewModel.ClockedInAt.ToShortTimeString());
        //            command.Parameters.AddWithValue("@DateClockedOut", viewModel.DateClockedOut.ToShortDateString());
        //            command.Parameters.AddWithValue("@ClockedOutAt", viewModel.ClockedOutAt.ToShortTimeString());
        //            command.Parameters.AddWithValue("@DurationOfClockIn", viewModel.DurationOfClockIn.ToString());
        //            command.Parameters.AddWithValue("@TotalTimeClockedInToday", viewModel.TotalTimeClockedInToday.ToString());
        //            command.Parameters.AddWithValue("@TotalTimeClockedInThisWeek", viewModel.TotalTimeClockedInThisWeek.ToString());
        //            command.Parameters.AddWithValue("@Username", viewModel.Username);
        //            await command.ExecuteNonQueryAsync();
        //            SqlModel.SqlConnection.Close();

    }
    
}
