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
    public class AdminAssignTaskRepository : IRepository<IAdminAssignTaskViewModel>, IGetAndUpdateAssignedTasksEmployee<IAdminAssignTaskViewModel>
    {
        public async Task DeleteAsync(IAdminAssignTaskViewModel viewModel)
        {
            try
            {
                if (viewModel.Id == null)
                {
                    await Application.Current.MainPage.DisplayAlert("YOU MUST ENTER A VALID ID", "TRY AGAIN", "OK");
                    return;
                }
                if (viewModel.TaskWasAssignedTo == null)
                {
                    await Application.Current.MainPage.DisplayAlert("YOU MUST ENTER A USERNAME", "TRY AGAIN", "OK");
                    return;
                }
                if (viewModel.DateOfAssignedTask == null)
                {
                    await Application.Current.MainPage.DisplayAlert("YOU MUST ENTER THE TASK DATE", "TRY AGAIN", "OK");
                    return;
                }
                await SqlModel.SqlConnection.OpenAsync();

                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM dbo.Task WHERE Id =   @Id";

                    command.Parameters.AddWithValue("@Id", viewModel.Id);

                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        command.CommandText = "DELETE FROM dbo.Task WHERE Id = @Id";

                        command.ExecuteNonQuery();

                        await Application.Current.MainPage.DisplayAlert("SUCCESSFULLY DELETED", "CLICK OK TO CONTINUE", "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("INVALID ID", "ID NOT FOUND IN DATABASE", "OK");
                    }
                    //Read and Update values after deleting.
                    command.CommandText = "SELECT NumberOfAssignedTasks FROM Task WHERE Username = @Username AND DateOfAssignedTask = @DateOfAssignedTask";

                    command.Parameters.AddWithValue("@Username", viewModel.TaskWasAssignedTo);
                    command.Parameters.AddWithValue("@DateOfAssignedTask", viewModel.DateOfAssignedTask.ToShortDateString());

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int numberOfAssignedTasksInDataBase = int.Parse(reader.GetString(0));

                            viewModel.NumberOfAssignedTasks = numberOfAssignedTasksInDataBase;

                            if (numberOfAssignedTasksInDataBase != int.MinValue)
                            {
                                viewModel.NumberOfAssignedTasks = numberOfAssignedTasksInDataBase - 1;
                            }
                        }
                    }
                    command.CommandText = "UPDATE dbo.Task SET NumberOfAssignedTasks = ISNULL(@NumberOfAssignedTasks, NumberOfAssignedTasks) " + 
                        "WHERE Username = @Username AND DateOfAssignedTask = @DateOfAssignedTask";

                    command.Parameters.AddWithValue("@NumberOfAssignedTasks", viewModel.NumberOfAssignedTasks ?? (object)DBNull.Value);

                    await command.ExecuteNonQueryAsync();
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
        public async Task<IEnumerable<IAdminAssignTaskViewModel>> GetAllAsync(IAdminAssignTaskViewModel viewModel)
        {
            try
            {
                await SqlModel.SqlConnection.OpenAsync();
                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Task";
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        viewModel.TaskCollection.Clear();
                        while (await reader.ReadAsync())
                        {
                             viewModel.TaskCollection.Insert(0, new AdminAssignTaskModel
                             {
                                 NumberOfAssignedTasks = Convert.ToInt32(reader["NumberOfAssignedTasks"]),
                                 DateOfAssignedTask = Convert.ToDateTime(reader["DateOfAssignedTask"]),
                                 TaskIsComplete = Convert.ToBoolean(reader["TaskIsComplete"]),
                                 TaskDescription = reader["TaskDescription"].ToString(),
                                 TaskTitle = reader["TaskTitle"].ToString(),
                                 TaskCompletedNotes = reader["TaskCompletedNotes"].ToString(),
                                 TaskWasAssignedTo = reader["TaskWasAssignedTo"].ToString(),
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
            return (IEnumerable<IAdminAssignTaskViewModel>)viewModel.TaskCollection;

        }
        public async Task GetAssignedTaskEmployee(IAdminAssignTaskViewModel viewModel, string username)
        {
            try
            {
                await SqlModel.SqlConnection.OpenAsync();

                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Task WHERE TaskWasAssignedTo = @Username";
                    command.Parameters.AddWithValue("@Username", username);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        viewModel.TaskCollection.Clear();
                        while (await reader.ReadAsync())
                        {
                            viewModel.TaskCollection.Insert(0, new AdminAssignTaskModel
                            {
                                Username = reader["Username"].ToString(),
                                NumberOfAssignedTasks = Convert.ToInt32(reader["NumberOfAssignedTasks"]),
                                DateOfAssignedTask = Convert.ToDateTime(reader["DateOfAssignedTask"]),
                                TaskIsComplete = Convert.ToBoolean(reader["TaskIsComplete"]),
                                TaskDescription = reader["TaskDescription"].ToString(),
                                TaskTitle = reader["TaskTitle"].ToString(),
                                TaskCompletedNotes = reader["TaskCompletedNotes"].ToString(),
                                TaskWasAssignedTo = reader["TaskWasAssignedTo"].ToString(),
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
            }
            finally
            {
                SqlModel.SqlConnection.Close();
            }
        }
        public async Task GetByIdOrUsername(IAdminAssignTaskViewModel viewModel, int id)
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
                    command.CommandText = "SELECT * FROM dbo.Task WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        viewModel.TaskCollection.Clear();
                        while (await reader.ReadAsync())
                        {
                            viewModel.TaskCollection.Add(new AdminAssignTaskModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Username = reader["Username"].ToString(),
                                NumberOfAssignedTasks = Convert.ToInt32(reader["NumberOfAssignedTasks"]),
                                DateOfAssignedTask = Convert.ToDateTime(reader["DateOfAssignedTask"]),
                                TaskIsComplete = Convert.ToBoolean(reader["TaskIsComplete"]),
                                TaskDescription = reader["TaskDescription"].ToString(),
                                TaskTitle = reader["TaskTitle"].ToString(),
                                TaskCompletedNotes = reader["TaskCompletedNotes"].ToString(),
                                TaskWasAssignedTo = reader["TaskWasAssignedTo"].ToString()
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

        public async Task GetByIdOrUsername(IAdminAssignTaskViewModel viewModel, string username)
        {
            try
            {
                if (viewModel.TaskWasAssignedTo == null)
                {
                    await Application.Current.MainPage.DisplayAlert("YOU MUST ENTER A USERNAME", "TRY AGAIN", "OK");
                    return;
                }
                await SqlModel.SqlConnection.OpenAsync();
                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Task WHERE Username = @Username";
                    command.Parameters.Add(new SqlParameter("@Username", username));

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        viewModel.TaskCollection.Clear();
                        while (await reader.ReadAsync())
                        {
                            viewModel.TaskCollection.Add(new AdminAssignTaskModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                NumberOfAssignedTasks = Convert.ToInt32(reader["NumberOfAssignedTasks"]),
                                DateOfAssignedTask = Convert.ToDateTime(reader["DateOfAssignedTask"]),
                                TaskIsComplete = Convert.ToBoolean(reader["TaskIsComplete"]),
                                TaskDescription = reader["TaskDescription"].ToString(),
                                TaskTitle = reader["TaskTitle"].ToString(),
                                TaskCompletedNotes = reader["TaskCompletedNotes"].ToString(),
                                TaskWasAssignedTo = reader["TaskWasAssignedTo"].ToString()
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
        public async Task PostAsync(IAdminAssignTaskViewModel viewModel)
        {
            viewModel.NumberOfAssignedTasks = 1;
            if (viewModel.DateOfAssignedTask == DateTime.MinValue)
            {
                viewModel.DateOfAssignedTask = DateTime.Today;
            }

            try
            {
                if (viewModel.TaskWasAssignedTo == null)
                {
                    await Application.Current.MainPage.DisplayAlert("YOU MUST ENTER A USERNAME", "TRY AGAIN", "OK");
                    return;
                }
                await SqlModel.SqlConnection.OpenAsync();

                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT NumberOfAssignedTasks FROM Task WHERE Username = @Username AND DateOfAssignedTask = @DateOfAssignedTask";

                    command.Parameters.Add(new SqlParameter("Username", viewModel.TaskWasAssignedTo));
                    command.Parameters.AddWithValue("@DateOfAssignedTask", viewModel.DateOfAssignedTask.ToShortDateString());

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int numberOfAssignedTasksInDataBase = int.Parse(reader.GetString(0));
                            viewModel.NumberOfAssignedTasks = numberOfAssignedTasksInDataBase + 1;
                            if (numberOfAssignedTasksInDataBase == int.MinValue)
                            {
                                viewModel.NumberOfAssignedTasks = 1;
                            }
                        }
                        reader.Close();
                    }
                    viewModel.NumberOfAssignedTasks = viewModel.NumberOfAssignedTasks;

                    command.CommandText = "INSERT INTO dbo.Task VALUES (@Username, @NumberOfAssignedTasks, @DateOfAssignedTask, @TaskIsComplete, @TaskDescription, @TaskTitle, @TaskCompletedNotes, @TaskWasAssignedTo)";

                    command.Parameters.Add(new SqlParameter("NumberOfAssignedTasks", viewModel.NumberOfAssignedTasks));
                    command.Parameters.Add(new SqlParameter("TaskIsComplete", viewModel.TaskIsComplete));
                    command.Parameters.Add(new SqlParameter("TaskDescription", viewModel.TaskDescription ?? string.Empty));
                    command.Parameters.Add(new SqlParameter("TaskTitle", viewModel.TaskTitle ?? string.Empty));
                    command.Parameters.Add(new SqlParameter("TaskCompletedNotes", viewModel.TaskCompletedNotes ?? string.Empty));
                    command.Parameters.Add(new SqlParameter("TaskWasAssignedTo", viewModel.TaskWasAssignedTo ?? string.Empty));

                        await command.ExecuteNonQueryAsync();

                        await Application.Current.MainPage.DisplayAlert("SUCCESSFULLY ADDED", "YOU JUST ASSIGNED A TASK", "OK");
                        SqlModel.SqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Not YEt", ex.Message, "OK");
            }
            finally
            {
                SqlModel.SqlConnection.Close();
            }
        }

        
        public async Task PutAsync(IAdminAssignTaskViewModel viewModel)
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
                    command.CommandText = "UPDATE dbo.Task SET DateOfAssignedTask = ISNULL(@DateOfAssignedTask, DateOfAssignedTask), " +
                                          "TaskDescription = ISNULL(@TaskDescription, TaskDescription), " +
                                          "TaskTitle = ISNULL(@TaskTitle, TaskTitle), "  +
                                          "TaskWasAssignedTo = ISNULL(@TaskWasAssignedTo, TaskWasAssignedTo) " +
                                          "WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", viewModel.Id);
                    command.Parameters.AddWithValue("@DateOfAssignedTask", viewModel.DateOfAssignedTask == DateTime.MinValue ? (object)DBNull.Value : viewModel.DateOfAssignedTask.ToShortDateString());
                    command.Parameters.AddWithValue("@TaskDescription", viewModel.TaskDescription ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TaskTitle", viewModel.TaskTitle ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TaskWasAssignedTo", viewModel.TaskWasAssignedTo ?? (object)DBNull.Value);

                    await command.ExecuteNonQueryAsync();

                    await Application.Current.MainPage.DisplayAlert("SUCCESSFULLY UPDATED", "THE TASK WAS UPDATED", "OK");
                }
                viewModel.TaskDescription = null;
                viewModel.TaskWasAssignedTo = null;
                viewModel.Id = null;
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
        public async Task UpdateAssignedTaskEmployee(IAdminAssignTaskViewModel viewModel, int id)
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

                    command.CommandText = "UPDATE Task SET [TaskIsComplete] = @TaskIsComplete,[TaskCompletedNotes] = @TaskCompletedNotes WHERE Id = @Id";

                    command.Parameters.AddWithValue("@Id", viewModel.Id);
                    command.Parameters.AddWithValue("@TaskIsComplete", viewModel.TaskIsComplete);


                    if (viewModel.TaskCompletedNotes != null)
                        command.Parameters.AddWithValue("@TaskCompletedNotes", viewModel.TaskCompletedNotes);

                    await command.ExecuteNonQueryAsync();
                    await Application.Current.MainPage.DisplayAlert("SUCCESSFULLY UPDATED", "YOU JUST UPDATED A TASK", "OK");
                    SqlModel.SqlConnection.Close();

                }
                viewModel.TaskCompletedNotes = null;
                viewModel.Id = null;
                viewModel.TaskIsComplete = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Not Yet", ex.Message, "OK");
            }
            finally
            {
                SqlModel.SqlConnection.Close();
            }
        }
    }
}
