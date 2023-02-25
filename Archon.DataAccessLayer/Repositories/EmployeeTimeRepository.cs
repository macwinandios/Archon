using Archon.DataAccessLayer.Models;
using Archon.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Archon.DataAccessLayer.Repositories
{
    public class EmployeeTimeRepository : IEmployeeTimeRepository<IEmployeeTimeModel>,IRepository<IEmployeeTimeModel>
    {
        public async Task ClockInAsync(IEmployeeTimeModel viewModel)
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

        public async Task DeleteAsync(IEmployeeTimeModel viewModel)
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
        public async Task<IEnumerable<IEmployeeTimeModel>> GetAllAsync(IEmployeeTimeModel viewModel)
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
            return viewModel.HoursAndPayCollection;
        }
        public async Task GetByIdOrUsername(IEmployeeTimeModel viewModel, int id)
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
        public async Task GetByIdOrUsername(IEmployeeTimeModel viewModel, string username)
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
        public async Task PostAsync(IEmployeeTimeModel viewModel)
        {
            try
            {
                viewModel.DateClockedOut = DateTime.Today;
                viewModel.ClockedOutAt = DateTime.Now;

                await SqlModel.SqlConnection.OpenAsync();

                // Calculate the start and end dates of the current week
                var currentDate = viewModel.DateClockedOut;
                var startOfWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
                var endOfWeek = startOfWeek.AddDays(7);

                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    // First query the database to get the previous time entries for the current user
                    command.CommandText = "SELECT DurationOfClockIn, TotalTimeClockedInToday, TotalTimeClockedInThisWeek, TotalWagesEarnedThisWeek, Username, DateClockedOut FROM HoursAndPay WHERE Username = @Username AND DateClockedIn >= @StartOfWeek AND DateClockedIn <= @EndOfWeek";
                    command.Parameters.AddWithValue("@Username", viewModel.Username);
                    command.Parameters.AddWithValue("@StartOfWeek", startOfWeek.ToShortDateString());
                    command.Parameters.AddWithValue("@EndOfWeek", endOfWeek.ToShortDateString());

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        viewModel.HourlyWage = 10;
                        viewModel.TotalTimeClockedInToday = viewModel.DurationOfClockIn;

                        TimeSpan totalTimeClockedThisWeek = TimeSpan.Zero;
                        TimeSpan totalDuration = TimeSpan.Zero;
                        TimeSpan totalDurationForCurrentDay = new TimeSpan();
                        float totalWagesEarnedThisWeek = 0;

                        while (await reader.ReadAsync())
                        {
                            var durationInTable = TimeSpan.Parse(reader.GetString(0));
                            var timeDayInTable = TimeSpan.Parse(reader.GetString(1));
                            var timeWeekInTable = TimeSpan.Parse(reader.GetString(2));
                            var wagesInTable = float.Parse(reader.GetString(3));
                            totalDuration += durationInTable;
                            totalTimeClockedThisWeek += timeDayInTable;
                            totalWagesEarnedThisWeek += wagesInTable;

                            // Add the duration to the total duration for the current day
                            if (viewModel.DateClockedIn.Date == DateTime.Parse(reader.GetString(5)).Date)
                            {
                                totalDurationForCurrentDay += durationInTable;
                            }
                        }
                        viewModel.TotalTimeClockedInThisWeek = totalDuration + viewModel.TotalTimeClockedInToday;

                        viewModel.TotalTimeClockedInToday += totalDurationForCurrentDay;

                        viewModel.TotalWagesEarnedThisWeek = totalWagesEarnedThisWeek + viewModel.HourlyWage * (float)totalTimeClockedThisWeek.TotalHours;
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

        public async Task GetTimeWorkedAsync(IEmployeeTimeModel viewModel, DateTime startOfWeek, DateTime endOfWeek)
        {
            if (endOfWeek < startOfWeek)
            {
                await Application.Current.MainPage.DisplayAlert("Dates were entered incorrectly", "End date must be after Start date", "OK");
                return;
            }
            TimeSpan maxDateRange = TimeSpan.FromDays(7);
            if (endOfWeek - startOfWeek > maxDateRange)
            {
                await Application.Current.MainPage.DisplayAlert("Dates were entered incorrectly", "Date range must be less than 7 days", "OK");
                return;
            }
            if (endOfWeek == null || startOfWeek == null)
            {
                await Application.Current.MainPage.DisplayAlert("Dates were entered incorrectly", "You must enter two dates.  Date range must be less than 7 days", "OK");
                return;
            }

            try
            {
                await SqlModel.SqlConnection.OpenAsync();

                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT DurationOfClockIn, TotalTimeClockedInToday, TotalTimeClockedInThisWeek, TotalWagesEarnedThisWeek FROM HoursAndPay WHERE Username = @Username AND DateClockedOut BETWEEN @startOfWeek AND @endOfWeek";
                    command.Parameters.AddWithValue("@Username", viewModel.Username);
                    command.Parameters.AddWithValue("@startOfWeek", startOfWeek);
                    command.Parameters.AddWithValue("@endOfWeek", endOfWeek);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        viewModel.HourlyWage = 10;
                        TimeSpan totalTimeClockedThisWeek = TimeSpan.Zero;
                        TimeSpan totalDuration = TimeSpan.Zero;
                        float totalWagesEarnedThisWeek = 0;

                        while (await reader.ReadAsync())
                        {
                            var durationInTable = TimeSpan.Parse(reader.GetString(0));
                            var timeDayInTable = TimeSpan.Parse(reader.GetString(1));
                            var timeWeekInTable = TimeSpan.Parse(reader.GetString(2));
                            var wagesInTable = float.Parse(reader.GetString(3));

                            totalDuration += durationInTable;
                            totalTimeClockedThisWeek += timeDayInTable;
                            totalWagesEarnedThisWeek += wagesInTable;
                        }

                        viewModel.TotalTimeClockedInToday = totalDuration;
                        viewModel.TotalTimeClockedInThisWeek = totalTimeClockedThisWeek + viewModel.TotalTimeClockedInToday;
                        viewModel.TotalWagesEarnedForDaysChosen = totalWagesEarnedThisWeek;
                    }
                }
                viewModel.HoursAndPayCollection.Clear();

                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM HoursAndPay WHERE Username = @Username AND DateClockedOut BETWEEN @startOfWeek AND @endOfWeek";
                    command.Parameters.AddWithValue("@Username", viewModel.Username);
                    command.Parameters.AddWithValue("@startOfWeek", startOfWeek);
                    command.Parameters.AddWithValue("@endOfWeek", endOfWeek);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
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
                    }
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

        public async Task PutAsync(IEmployeeTimeModel viewModel)
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

                                viewModel.TotalTimeClockedInToday = viewModel.DurationOfClockIn + totalTimeCLockedInTodayIntable - timeClockedInTodayInTable;

                                viewModel.TotalTimeClockedInThisWeek = viewModel.DurationOfClockIn + totalTimeCLockedInTodayIntable - timeClockedInTodayInTable + totalTimeClockedInThisWeekInTable;
                                ;

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
