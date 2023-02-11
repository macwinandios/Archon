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
            finally
            {
                SqlModel.SqlConnection.Close();
            }
        }

        public async Task DeleteAsync(IEmployeeTimeViewModel viewModel)
        {
            try
            {
                await SqlModel.SqlConnection.OpenAsync();

                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM dbo.HoursAndPay WHERE Id =   @Id";
                    command.Parameters.AddWithValue("@Id", viewModel.Id);
                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        command.CommandText = "DELETE FROM dbo.HoursAndPay WHERE Id =   @Id";
                        command.ExecuteNonQuery();
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

        
        public async Task GetByIdOrUsername(IEmployeeTimeViewModel viewModel, string username)
        {
            try
            {
                SqlModel.SqlConnection.Open();

                SqlCommand clientCommand = new SqlCommand("SELECT * FROM dbo.HoursAndPay WHERE Username = @Username", SqlModel.SqlConnection);
                clientCommand.Parameters.AddWithValue("@Username", username);

                SqlDataReader clientReader = clientCommand.ExecuteReader();
                viewModel.HoursAndPayCollection.Clear();


                while (clientReader.Read())
                {
                    viewModel.HoursAndPayCollection.Add(new EmployeeTimeModel
                    {

                        Username = clientReader["Username"].ToString(),
                        HourlyWage = Convert.ToSingle(clientReader["HourlyWage"]),
                        DateClockedIn = Convert.ToDateTime(clientReader["DateClockedIn"]),
                        ClockedInAt = Convert.ToDateTime(clientReader["ClockedInAt"]),
                        DateClockedOut = Convert.ToDateTime(clientReader["DateClockedOut"]),
                        ClockedOutAt = Convert.ToDateTime(clientReader["ClockedOutAt"]),
                        DurationOfClockIn = TimeSpan.Parse((string)clientReader["DurationOfClockIn"]),
                        TotalTimeClockedInToday = TimeSpan.Parse((string)clientReader["TotalTimeClockedInToday"]),
                        TotalTimeClockedInThisWeek = TimeSpan.Parse((string)clientReader["TotalTimeClockedInThisWeek"]),
                        TotalWagesEarnedThisWeek = Convert.ToSingle(clientReader["TotalWagesEarnedThisWeek"]),
                        Id = Convert.ToInt32(clientReader["Id"])
                    });

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
        public async Task PutAsync(IEmployeeTimeViewModel viewModel)
        {
            try
            {
                SqlModel.SqlConnection.Open();

                using (var command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = @"UPDATE [dbo].[HoursAndPay]
                                   SET [ClockedInAt] = @ClockedInAt, [ClockedOutAt] = @ClockedOutAt WHERE [Id] = @Id";

                    command.Parameters.AddWithValue("@Id", viewModel.Id.ToString());
                    command.Parameters.AddWithValue("@ClockedInAt", viewModel.ClockedInAt.ToShortTimeString());
                    command.Parameters.AddWithValue("@ClockedOutAt", viewModel.ClockedOutAt.ToShortTimeString());
                    await command.ExecuteNonQueryAsync();
                    await Application.Current.MainPage.DisplayAlert("SUCCESSFULLY UPDATED", "YOU JUST UPDATED A TIME", "OK");

                }

                SqlModel.SqlConnection.Close();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Not YEt", ex.Message, "OK");
            }
        }
        public Task<IEnumerable<IEmployeeTimeViewModel>> GetAllAsync(IEmployeeTimeViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public async Task GetByIdOrUsername(IEmployeeTimeViewModel viewModel, int id)
        {
            try
            {
                SqlModel.SqlConnection.Open();

                SqlCommand clientCommand = new SqlCommand("SELECT * FROM dbo.HoursAndPay WHERE Id = @Id", SqlModel.SqlConnection);
                clientCommand.Parameters.AddWithValue("@Id", id);

                SqlDataReader clientReader = clientCommand.ExecuteReader();
                viewModel.HoursAndPayCollection.Clear();


                while (clientReader.Read())
                {
                    viewModel.HoursAndPayCollection.Add(new EmployeeTimeModel
                    {

                        Username = clientReader["Username"].ToString(),
                        HourlyWage = Convert.ToSingle(clientReader["HourlyWage"]),
                        DateClockedIn = Convert.ToDateTime(clientReader["DateClockedIn"]),
                        ClockedInAt = Convert.ToDateTime(clientReader["ClockedInAt"]),
                        DateClockedOut = Convert.ToDateTime(clientReader["DateClockedOut"]),
                        ClockedOutAt = Convert.ToDateTime(clientReader["ClockedOutAt"]),
                        DurationOfClockIn = TimeSpan.Parse((string)clientReader["DurationOfClockIn"]),
                        TotalTimeClockedInToday = TimeSpan.Parse((string)clientReader["TotalTimeClockedInToday"]),
                        TotalTimeClockedInThisWeek = TimeSpan.Parse((string)clientReader["TotalTimeClockedInThisWeek"]),
                        TotalWagesEarnedThisWeek = Convert.ToSingle(clientReader["TotalWagesEarnedThisWeek"]),
                        Id = Convert.ToInt32(clientReader["Id"])
                    });

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
    }


}
