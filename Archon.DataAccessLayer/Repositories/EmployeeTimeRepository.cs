using Archon.DataAccessLayer.Models;
using Archon.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
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
            if (viewModel.ClockedOutAt == null || viewModel.ClockedInAt == null)
            {
                await Application.Current.MainPage.DisplayAlert("MUST CLOCK IN", "THEN CLOCK OUT", "OK");
            }
            try
            {
                viewModel.ClockedOutAt = DateTime.Now;
                await SqlModel.SqlConnection.OpenAsync();
                using (var command = SqlModel.SqlConnection.CreateCommand())
                {
                    // First query the database to get the previous time entries for the current user

                    command.CommandText = "SELECT DurationOfClockIn, TotalTimeClockedInToday, TotalTimeClockedInThisWeek, TotalWagesEarnedThisWeek, Username FROM HoursAndPay WHERE Username = @Username AND DateClockedIn = @DateClockedIn AND DateClockedOut = @DateClockedOut";
                    command.Parameters.AddWithValue("@Username", viewModel.Username);
                    command.Parameters.AddWithValue("@DateClockedIn", viewModel.DateClockedIn.ToShortDateString());
                    command.Parameters.AddWithValue("@DateClockedOut", viewModel.DateClockedOut.ToShortDateString());
                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        viewModel.TotalTimeClockedInToday = viewModel.DurationOfClockIn;
                        TimeSpan totalDuration = new TimeSpan();
                        TimeSpan totalTimeThisWeek = new TimeSpan();
                        float hourlyWage = 10;
                        float totalWages = new float();

                        while (await reader.ReadAsync())
                        {
                            var username = reader.GetString(4);
                            if (username == viewModel.Username)
                            {
                                var duration = TimeSpan.Parse(reader.GetString(0));
                                var timeDay = TimeSpan.Parse(reader.GetString(1));
                                var timeWeek = TimeSpan.Parse(reader.GetString(2));
                                var wages = float.Parse(reader.GetString(3));

                                totalDuration = totalDuration.Add(duration);
                                totalTimeThisWeek = totalTimeThisWeek.Add(timeDay);
                                totalWages += wages;

                                viewModel.TotalTimeClockedInToday = totalDuration + viewModel.DurationOfClockIn;
                                viewModel.TotalTimeClockedInThisWeek = totalTimeThisWeek + viewModel.TotalTimeClockedInToday;
                                //Calculate total wages earned this week
                                //viewModel.HourlyWage = hourlyWage;
                                viewModel.TotalWagesEarnedThisWeek = (float)Math.Round(wages + (viewModel.TotalTimeClockedInThisWeek.TotalHours * viewModel.HourlyWage), 2);
                            }
                        }
                        viewModel.DurationOfClockIn = totalDuration;
                        viewModel.HourlyWage = hourlyWage;
                        viewModel.TotalWagesEarnedThisWeek = (float)Math.Round(viewModel.TotalTimeClockedInThisWeek.TotalHours * viewModel.HourlyWage, 2);
                    }
                    command.Parameters.Clear();

                    // Insert the updated values into the database
                    command.CommandText = @"INSERT INTO [dbo].[HoursAndPay] ([Username],[DateClockedIn], [ClockedInAt], [DateClockedOut], [ClockedOutAt], [DurationOfClockIn], [TotalTimeClockedInToday], [TotalTimeClockedInThisWeek], [HourlyWage],[TotalWagesEarnedThisWeek])
                            VALUES (@Username, @DateClockedIn, @ClockedInAt,@DateClockedOut, @ClockedOutAt, @DurationOfClockIn, @TotalTimeClockedInToday, @TotalTimeClockedInThisWeek, @HourlyWage, @TotalWagesEarnedThisWeek)";
                    command.Parameters.AddWithValue("@Username", viewModel.Username);
                    command.Parameters.AddWithValue("@DateClockedIn", viewModel.DateClockedIn.ToShortDateString());
                    command.Parameters.AddWithValue("@ClockedInAt", viewModel.ClockedInAt.ToShortTimeString());
                    command.Parameters.AddWithValue("@DateClockedOut", viewModel.DateClockedOut.ToShortDateString());
                    command.Parameters.AddWithValue("@ClockedOutAt", viewModel.ClockedOutAt.ToShortTimeString());
                    command.Parameters.AddWithValue("@DurationOfClockIn", viewModel.DurationOfClockIn.ToString());
                    command.Parameters.AddWithValue("@TotalTimeClockedInToday", viewModel.TotalTimeClockedInToday.ToString());
                    command.Parameters.AddWithValue("@TotalTimeClockedInThisWeek", viewModel.TotalTimeClockedInThisWeek.ToString());
                    command.Parameters.AddWithValue("@HourlyWage", viewModel.HourlyWage.ToString());
                    command.Parameters.AddWithValue("@TotalWagesEarnedThisWeek", viewModel.TotalWagesEarnedThisWeek.ToString());
                    await command.ExecuteNonQueryAsync();
                }

                SqlModel.SqlConnection.Close();
            }

            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }


    //THE FOLLOWING WILL BE USED BY ADMIN IN THE ADMINMONITORPAY REPOSITORY. SAVING IT HERE.


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



    // THE FOLLOWING USES SYSTEM.DATA IN ORDER TO USE ADD INSTEAD OF ADDWITHVALUE
    //WILL IMPLEMENT SOON.


    // update existing user
    //            //    using (var updateCommand = SqlModel.SqlConnection.CreateCommand())
    //            //    {
    //            //        updateCommand.CommandText = @"UPDATE EmployeeTime SET ClockedOutAt = @ClockedOutAt, DurationOfClockIn = DurationOfClockIn + @DurationOfClockIn, TotalTimeClockedInToday = TotalTimeClockedInToday + @TotalTimeClockedInToday, TotalTimeClockedInThisWeek = TotalTimeClockedInThisWeek + @TotalTimeClockedInThisWeek" +
    //            //        "WHERE Username = @Username";

    //            //        updateCommand.Parameters.Add("@ClockedOutAt", SqlDbType.Time).Value = viewModel.ClockedOutAt.TimeOfDay;
    //            //        updateCommand.Parameters.Add("@DurationOfClockIn", SqlDbType.Time).Value = viewModel.DurationOfClockIn.TotalHours;
    //            //        updateCommand.Parameters.Add("@TotalTimeClockedInToday", SqlDbType.Time).Value = viewModel.TotalTimeClockedInToday.TotalHours;
    //            //        updateCommand.Parameters.Add("@TotalTimeClockedInThisWeek", SqlDbType.Time).Value = viewModel.TotalTimeClockedInThisWeek;
    //            //    updateCommand.Parameters.Add("@TotalTimeClockedInToday", SqlDbType.Time).Value = viewModel.TotalTimeClockedInToday.TotalHours;
    //            //        updateCommand.Parameters.Add("@TotalTimeClockedInThisWeek", SqlDbType.Time).Value = viewModel.TotalTimeClockedInThisWeek.TotalHours;
    //            //        //updateCommand.Parameters.Add("@TotalEarnedThisWeek", SqlDbType.Money).Value = viewModel.TotalEarnedThisWeek;
    //            //        //updateCommand.Parameters.Add("@HourlyWage", SqlDbType.Money).Value = viewModel.HourlyWage;
    //            //        await updateCommand.ExecuteNonQueryAsync();
    //            //    }
    //            //}



    //                using (var insertCommand = SqlModel.SqlConnection.CreateCommand())
    //                {
    //                    insertCommand.CommandText = @"INSERT INTO [dbo].[EmployeeTime] ([Username],[DateClockedIn], [ClockedInAt], [DateClockedOut], [ClockedOutAt], [DurationOfClockIn], [TotalTimeClockedInToday], [TotalTimeClockedInThisWeek])
    //                                                   VALUES (@Username, @DateClockedIn, @ClockedInAt, @DateClockedOut, @ClockedOutAt, @DurationOfClockIn, @TotalTimeClockedInToday, @TotalTimeClockedInThisWeek)";

    //                    insertCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = viewModel.Username;
    //                    insertCommand.Parameters.Add("@DateClockedIn", SqlDbType.VarChar).Value = viewModel.DateClockedIn.Date.ToShortDateString();
    //                    insertCommand.Parameters.Add("@ClockedInAt", SqlDbType.VarChar).Value = viewModel.ClockedInAt.TimeOfDay.ToString();
    //                    insertCommand.Parameters.Add("@DateClockedOut", SqlDbType.VarChar).Value = viewModel.DateClockedOut.Date.ToShortDateString();
    //                    insertCommand.Parameters.Add("@ClockedOutAt", SqlDbType.VarChar).Value = viewModel.ClockedOutAt.TimeOfDay;
    //                    insertCommand.Parameters.Add("@DurationOfClockIn", SqlDbType.VarChar).Value = viewModel.DurationOfClockIn.TotalHours;
    //                    insertCommand.Parameters.Add("@TotalTimeClockedInToday", SqlDbType.VarChar).Value = viewModel.TotalTimeClockedInToday.TotalHours;
    //                    insertCommand.Parameters.Add("@TotalTimeClockedInThisWeek", SqlDbType.VarChar).Value = viewModel.TotalTimeClockedInThisWeek.TotalHours;
    //                    //insertCommand.Parameters.Add("@TotalEarnedThisWeek", SqlDbType.Money).Value = viewModel.TotalWagesEarnedThisWeek;
    //                    //insertCommand.Parameters.Add("@HourlyWage", SqlDbType.Money).Value = viewModel.HourlyWage;
    //                    await insertCommand.ExecuteNonQueryAsync();
    //}

    ///

}
