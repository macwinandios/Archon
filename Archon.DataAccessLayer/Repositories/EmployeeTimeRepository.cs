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
    public class EmployeeTimeRepository : IEmployeeTimeRepository<IEmployeeTimeViewModel>,IRepository<IEmployeeTimeViewModel>
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
        }

        public async Task DeleteAsync(IEmployeeTimeViewModel viewModel)
        {
            try
            {
                if (viewModel.Id == null)
                {
                    await Application.Current.MainPage.DisplayAlert("YOU MUST ENTER A VALID ID", "TRY AGAIN", "OK");
                    return;
                }
                await SqlModel.SqlConnection.OpenAsync();

                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM dbo.HoursAndPay WHERE Id =   @Id";
                    command.Parameters.AddWithValue("@Id", viewModel.Id);
                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        command.CommandText = "DELETE FROM dbo.HoursAndPay WHERE Id =   @Id";

                        await command.ExecuteNonQueryAsync();

                        await Application.Current.MainPage.DisplayAlert("SUCCESSFULLY DELETED", "CLICK OK TO CONTINUE", "OK");
                        viewModel.Id = null;
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("INVALID ID", "ID NOT FOUND IN DATABASE", "OK");
                        viewModel.Id = null;
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("NOT YET", ex.Message, "OK");
            }
            finally
            {
                SqlModel.SqlConnection.Close();
            }
        }
        public async Task<IEnumerable<IEmployeeTimeViewModel>> GetAllAsync(IEmployeeTimeViewModel viewModel)
        {
            try
            {
                await SqlModel.SqlConnection.OpenAsync();
                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.HoursAndPay";
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        viewModel.HoursAndPayCollection.Clear();
                        while (await reader.ReadAsync())
                        {
                            viewModel.HoursAndPayCollection.Insert(0, new EmployeeTimeModel
                            {
                                Username = reader["Username"].ToString(),
                                HourlyWage = Convert.ToSingle(reader["HourlyWage"]),
                                DateClockedIn = Convert.ToDateTime(reader["DateClockedIn"]),
                                ClockedInAt = Convert.ToDateTime(reader["ClockedInAt"]),
                                DateClockedOut = Convert.ToDateTime(reader["DateClockedOut"]),
                                ClockedOutAt = Convert.ToDateTime(reader["ClockedOutAt"]),
                                DurationOfClockIn = TimeSpan.Parse((string)reader["DurationOfClockIn"]),
                                TotalTimeClockedInToday = TimeSpan.Parse((string)reader["TotalTimeClockedInToday"]),
                                TotalTimeClockedInThisWeek = TimeSpan.Parse((string)reader["TotalTimeClockedInThisWeek"]),
                                TotalWagesEarnedThisWeek = Convert.ToSingle(reader["TotalWagesEarnedThisWeek"]),
                                Id = Convert.ToInt32(reader["Id"])
                            });
                        }
                        reader.Close();
                        SqlModel.SqlConnection.Close();
                    }
                }
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
            return (IEnumerable<IEmployeeTimeViewModel>)viewModel.HoursAndPayCollection;
        }
        public async Task GetByIdOrUsername(IEmployeeTimeViewModel viewModel, int id)
        {
            try
            {
                if (viewModel.Id == null)
                {
                    await Application.Current.MainPage.DisplayAlert("YOU MUST ENTER A VALID ID", "TRY AGAIN", "OK");
                    return;
                }
                await SqlModel.SqlConnection.OpenAsync();
                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.HoursAndPay WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        viewModel.HoursAndPayCollection.Clear();
                        while (await reader.ReadAsync())
                        {
                            viewModel.HoursAndPayCollection.Add(new EmployeeTimeModel
                            {

                                Username = reader["Username"].ToString(),
                                HourlyWage = Convert.ToSingle(reader["HourlyWage"]),
                                DateClockedIn = Convert.ToDateTime(reader["DateClockedIn"]),
                                ClockedInAt = Convert.ToDateTime(reader["ClockedInAt"]),
                                DateClockedOut = Convert.ToDateTime(reader["DateClockedOut"]),
                                ClockedOutAt = Convert.ToDateTime(reader["ClockedOutAt"]),
                                DurationOfClockIn = TimeSpan.Parse((string)reader["DurationOfClockIn"]),
                                TotalTimeClockedInToday = TimeSpan.Parse((string)reader["TotalTimeClockedInToday"]),
                                TotalTimeClockedInThisWeek = TimeSpan.Parse((string)reader["TotalTimeClockedInThisWeek"]),
                                TotalWagesEarnedThisWeek = Convert.ToSingle(reader["TotalWagesEarnedThisWeek"]),
                                Id = Convert.ToInt32(reader["Id"])
                            });
                        }
                        reader.Close();
                        SqlModel.SqlConnection.Close();
                    }
                }
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
        public async Task GetByIdOrUsername(IEmployeeTimeViewModel viewModel, string username)
        {
            try
            {
                await SqlModel.SqlConnection.OpenAsync();
                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    
                    command.CommandText = "SELECT * FROM dbo.HoursAndPay WHERE Username = @Username";
                    command.Parameters.AddWithValue("@Username", username);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    { 
                        viewModel.HoursAndPayCollection.Clear();
                        while (await reader.ReadAsync())
                        {
                            viewModel.HoursAndPayCollection.Add(new EmployeeTimeModel
                            {

                                Username = reader["Username"].ToString(),
                                HourlyWage = Convert.ToSingle(reader["HourlyWage"]),
                                DateClockedIn = Convert.ToDateTime(reader["DateClockedIn"]),
                                ClockedInAt = Convert.ToDateTime(reader["ClockedInAt"]),
                                DateClockedOut = Convert.ToDateTime(reader["DateClockedOut"]),
                                ClockedOutAt = Convert.ToDateTime(reader["ClockedOutAt"]),
                                DurationOfClockIn = TimeSpan.Parse((string)reader["DurationOfClockIn"]),
                                TotalTimeClockedInToday = TimeSpan.Parse((string)reader["TotalTimeClockedInToday"]),
                                TotalTimeClockedInThisWeek = TimeSpan.Parse((string)reader["TotalTimeClockedInThisWeek"]),
                                TotalWagesEarnedThisWeek = Convert.ToSingle(reader["TotalWagesEarnedThisWeek"]),
                                Id = Convert.ToInt32(reader["Id"])
                            });
                        }
                        reader.Close();
                        SqlModel.SqlConnection.Close();
                    }
                }
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
            try
            {
                viewModel.DateClockedOut = DateTime.Today;
                //GENERATE DATE FOR TESTING
                //viewModel.DateClockedOut = new DateTime(2023, 2, 14);

                viewModel.ClockedOutAt = DateTime.Now;
                await SqlModel.SqlConnection.OpenAsync();
                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    // First query the database to get the previous time entries for the current user
                    command.CommandText = "SELECT DurationOfClockIn, TotalTimeClockedInToday, TotalTimeClockedInThisWeek, TotalWagesEarnedThisWeek, Username, DateClockedOut FROM HoursAndPay WHERE Username = @Username AND DateClockedIn = @DateClockedIn AND DateClockedOut = @DateClockedOut";
                    command.Parameters.AddWithValue("@Username", viewModel.Username);
                    command.Parameters.AddWithValue("@DateClockedIn", viewModel.DateClockedIn.ToShortDateString());
                    command.Parameters.AddWithValue("@DateClockedOut", viewModel.DateClockedOut.ToShortDateString());
                    
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        viewModel.HourlyWage = 10;
                        viewModel.TotalTimeClockedInToday = viewModel.DurationOfClockIn;
                        TimeSpan totalDuration = new TimeSpan();
                        TimeSpan totalTimeClockedInThisWeek = new TimeSpan();
                        if (totalTimeClockedInThisWeek.Equals(TimeSpan.Zero))
                        {
                            viewModel.TotalTimeClockedInThisWeek = viewModel.TotalTimeClockedInToday;
                        }
                        //If user's Clock-In <= 11:59.59pm and Clock-Out >= 12:00:01am:Add the time to Clock-In Date.
                        if(viewModel.ClockedOutAt.Date == viewModel.ClockedInAt.Date.AddDays(1))
                        {
                            viewModel.ClockedOutAt.Date.AddDays(-1);
                        }
                        while (await reader.ReadAsync())
                        {
                            var username = reader.GetString(4);
                            if (username == viewModel.Username)
                            {
                                var durationInTable = TimeSpan.Parse(reader.GetString(0));
                                var timeDayInTable = TimeSpan.Parse(reader.GetString(1));
                                var timeWeekInTable = TimeSpan.Parse(reader.GetString(2));
                                var wagesInTable = float.Parse(reader.GetString(3));
                                var dateClockedOutInTable = DateTime.Parse(reader.GetString(5));

                                totalDuration = totalDuration.Add(durationInTable);
                                totalTimeClockedInThisWeek = totalTimeClockedInThisWeek.Add(timeDayInTable);
                                viewModel.TotalTimeClockedInToday = totalDuration + viewModel.DurationOfClockIn;
                                if(dateClockedOutInTable.Date.AddDays(7) == viewModel.DateClockedOut)
                                {
                                    viewModel.TotalTimeClockedInThisWeek = TimeSpan.Zero;
                                }
                                viewModel.TotalTimeClockedInThisWeek = totalDuration + viewModel.DurationOfClockIn;
                            }
                        }
                        viewModel.DurationOfClockIn = totalDuration;

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
            finally
            {
                SqlModel.SqlConnection.Close();
            }
        }
        public async Task PutAsync(IEmployeeTimeViewModel viewModel)
        {
            if (viewModel.Id == null)
            {
                await Application.Current.MainPage.DisplayAlert("Must Enter a Valid Id", "Try Again", "OK");
                return;
            }
            try
            {
                if (viewModel.UpdatedClockOutTime > viewModel.UpdatedClockInTime || viewModel.UpdatedClockOutTime == null && viewModel.UpdatedClockInTime != null || viewModel.UpdatedClockInTime == null && viewModel.UpdatedClockOutTime != null)
                {

                    await SqlModel.SqlConnection.OpenAsync();

                    using (var command = SqlModel.SqlConnection.CreateCommand())
                    {
                        command.CommandText = "SELECT ClockedInAt, ClockedOutAt, DurationOfClockIn, TotalTimeClockedInToday, TotalTimeClockedInThisWeek, TotalWagesEarnedThisWeek FROM [dbo].[HoursAndPay] WHERE DateClockedOut = @DateClockedOut AND Username = @Username";
                        command.Parameters.AddWithValue("@DateClockedOut", viewModel.DateClockedOut);
                        command.Parameters.AddWithValue("@Username", viewModel.Username);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            TimeSpan totalDurationInTable = new TimeSpan();
                            TimeSpan totalTimeCLockedInTodayIntable = new TimeSpan();
                            TimeSpan totalTimeClockedInThisWeekInTable = new TimeSpan();
                            float totalwages = new float();
                            while (await reader.ReadAsync())
                            {
                                DateTime clockedInAtInTable = DateTime.Parse(reader.GetString(0));
                                DateTime clockedOutAtInTable = DateTime.Parse(reader.GetString(1));
                                var durationOfClockInInTable = TimeSpan.Parse(reader.GetString(2));
                                var timeClockedInTodayInTable = TimeSpan.Parse(reader.GetString(3));
                                var timeClockedInThisWeekInTable = TimeSpan.Parse(reader.GetString(4));
                                var totalWagesEarnedThisWeekInTable = float.Parse(reader.GetString(5));


                                totalDurationInTable += durationOfClockInInTable;
                                totalTimeCLockedInTodayIntable += timeClockedInTodayInTable;
                                totalTimeClockedInThisWeekInTable += timeClockedInThisWeekInTable;
                                totalwages += totalWagesEarnedThisWeekInTable;


                                if (viewModel.UpdatedClockOutTime != null && viewModel.UpdatedClockInTime != null)
                                {
                                    viewModel.DurationOfClockIn = viewModel.UpdatedClockOutTime - viewModel.UpdatedClockInTime;
                                }

                                if (durationOfClockInInTable != viewModel.UpdatedClockOutTime - clockedInAtInTable.TimeOfDay)
                                {
                                    viewModel.DurationOfClockIn = viewModel.UpdatedClockOutTime - clockedInAtInTable.TimeOfDay;
                                }
                                if (durationOfClockInInTable != viewModel.UpdatedClockInTime + clockedOutAtInTable.TimeOfDay)
                                {
                                    viewModel.DurationOfClockIn = viewModel.UpdatedClockInTime + clockedOutAtInTable.TimeOfDay;
                                }

                                viewModel.DurationOfClockIn = viewModel.UpdatedClockOutTime - viewModel.UpdatedClockInTime;

                                viewModel.TotalTimeClockedInToday = viewModel.DurationOfClockIn + totalTimeCLockedInTodayIntable - timeClockedInTodayInTable;

                                viewModel.TotalTimeClockedInThisWeek = viewModel.DurationOfClockIn + totalTimeCLockedInTodayIntable - timeClockedInTodayInTable;

                            }
                            reader.Close();

                            command.Parameters.Clear();

                            command.CommandText = @"UPDATE [dbo].[HoursAndPay]
                               SET [ClockedInAt] = ISNULL(@ClockedInAt, ClockedInAt), [ClockedOutAt] = ISNULL(@ClockedOutAt, ClockedOutAt), [DurationOfClockIn] = ISNULL(@DurationOfClockIn, DurationOfClockIn),[TotalTimeClockedInToday] = ISNULL(@TotalTimeClockedInToday, TotalTimeClockedInToday),[TotalTimeClockedInThisWeek] = ISNULL(@TotalTimeClockedInThisWeek, TotalTimeClockedInThisWeek),[TotalWagesEarnedThisWeek] = ISNULL(@TotalWagesEarnedThisWeek, TotalWagesEarnedThisWeek) WHERE Id = @Id";

                            command.Parameters.AddWithValue("@Id", viewModel.Id);
                            command.Parameters.AddWithValue("@ClockedInAt", viewModel.UpdatedClockInTime == TimeSpan.Zero ? (object)DBNull.Value : viewModel.UpdatedClockInTime.ToString());
                            command.Parameters.AddWithValue("@ClockedOutAt", viewModel.UpdatedClockOutTime == TimeSpan.Zero ? (object)DBNull.Value : viewModel.UpdatedClockOutTime.ToString());
                            command.Parameters.AddWithValue("@DurationOfClockIn", viewModel.DurationOfClockIn == TimeSpan.Zero ? (object)DBNull.Value : viewModel.DurationOfClockIn.ToString());
                            command.Parameters.AddWithValue("@TotalTimeClockedInToday", viewModel.TotalTimeClockedInToday == TimeSpan.Zero ? (object)DBNull.Value : viewModel.TotalTimeClockedInToday.ToString());
                            command.Parameters.AddWithValue("@TotalTimeClockedInThisWeek", viewModel.TotalTimeClockedInThisWeek == TimeSpan.Zero ? (object)DBNull.Value : viewModel.TotalTimeClockedInThisWeek.ToString());
                            command.Parameters.AddWithValue("@TotalWagesEarnedThisWeek", viewModel.TotalWagesEarnedThisWeek == 0 ? (object)DBNull.Value : viewModel.TotalWagesEarnedThisWeek.ToString());


                            await command.ExecuteNonQueryAsync();


                            await Application.Current.MainPage.DisplayAlert("SUCCESS!!", "UPDATED TIME", "OK");
                        }
                    }
                    SqlModel.SqlConnection.Close();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("TIME ERROR", "CLOCK-OUT TIME MUST BE GREATER THAN CLOCK-IN TIME", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("ERROR", ex.Message, "OK");
            }
            finally
            {
                SqlModel.SqlConnection.Close();
            }
        }
    }
}
