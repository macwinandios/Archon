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
                        //viewModel.Id = null;
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("INVALID ID", "ID NOT FOUND IN DATABASE", "OK");
                        //viewModel.Id = null;
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

        public async Task<IEnumerable<IAdminAssignTaskViewModel>> GetAllAsync(IAdminAssignTaskViewModel viewModel)
        {
            try
            {
                SqlModel.SqlConnection.Open();

                SqlCommand clientCommand = new SqlCommand("SELECT * FROM dbo.Task", SqlModel.SqlConnection);

                SqlDataReader clientReader = clientCommand.ExecuteReader();
                viewModel.TaskCollection.Clear();

                while (clientReader.Read())
                {
                    try
                    {
                        viewModel.TaskCollection.Insert(0, new AdminAssignTaskModel
                        {

                            Username = clientReader["Username"].ToString(),
                            NumberOfAssignedTasks = Convert.ToInt32(clientReader["NumberOfAssignedTasks"]),
                            DateOfAssignedTask = Convert.ToDateTime(clientReader["DateOfAssignedTask"]),
                            TaskIsComplete = Convert.ToBoolean(clientReader["TaskIsComplete"]),
                            TaskDescription = clientReader["TaskDescription"].ToString(),
                            TaskTitle = clientReader["TaskTitle"].ToString(),
                            TaskCompletedNotes = clientReader["TaskCompletedNotes"].ToString(),
                            TaskWasAssignedTo = clientReader["TaskWasAssignedTo"].ToString(),
                            Id = Convert.ToInt32(clientReader["Id"])
                        });
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("NOT YET", ex.Message, "OK");

                    }

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
            return (IEnumerable<IAdminAssignTaskViewModel>)Task.CompletedTask;

        }
        public Task GetAssignedTaskEmployee(IAdminAssignTaskViewModel viewModel, string username)
        {
            try
            {
                SqlModel.SqlConnection.Open();

                SqlCommand clientCommand = new SqlCommand("SELECT * FROM dbo.Task WHERE TaskWasAssignedTo = @Username", SqlModel.SqlConnection);
                clientCommand.Parameters.AddWithValue("@Username", username);

                SqlDataReader clientReader = clientCommand.ExecuteReader();
                viewModel.TaskCollection.Clear();

                while (clientReader.Read())
                {
                    viewModel.TaskCollection.Insert(0,new AdminAssignTaskModel
                    {
                        Username = clientReader["Username"].ToString(),
                        NumberOfAssignedTasks = Convert.ToInt32(clientReader["NumberOfAssignedTasks"]),
                        DateOfAssignedTask = Convert.ToDateTime(clientReader["DateOfAssignedTask"]),
                        TaskIsComplete = Convert.ToBoolean(clientReader["TaskIsComplete"]),
                        TaskDescription = clientReader["TaskDescription"].ToString(),
                        TaskTitle = clientReader["TaskTitle"].ToString(),
                        TaskCompletedNotes = clientReader["TaskCompletedNotes"].ToString(),
                        TaskWasAssignedTo = clientReader["TaskWasAssignedTo"].ToString(),
                        Id = Convert.ToInt32(clientReader["Id"])
                    });

                }
                clientReader.Close();
                SqlModel.SqlConnection.Close();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("NOT YET", ex.Message, "OK");
            }
            finally
            {
                SqlModel.SqlConnection.Close();

            }
            return Task.CompletedTask;

        }

        public async Task GetByIdOrUsername(IAdminAssignTaskViewModel viewModel, int id)
        {
            try
            {
                SqlModel.SqlConnection.Open();

                SqlCommand clientCommand = new SqlCommand("SELECT * FROM dbo.Task WHERE Id = @Id", SqlModel.SqlConnection);
                clientCommand.Parameters.AddWithValue("@Id", id);

                SqlDataReader clientReader = clientCommand.ExecuteReader();
                viewModel.TaskCollection.Clear();


                while (clientReader.Read())
                {
                    viewModel.TaskCollection.Add(new AdminAssignTaskModel
                    {
                        Id = Convert.ToInt32(clientReader["Id"]),
                        Username = clientReader["Username"].ToString(),
                        NumberOfAssignedTasks = Convert.ToInt32(clientReader["NumberOfAssignedTasks"]),
                        DateOfAssignedTask = Convert.ToDateTime(clientReader["DateOfAssignedTask"]),
                        TaskIsComplete = Convert.ToBoolean(clientReader["TaskIsComplete"]),
                        TaskDescription = clientReader["TaskDescription"].ToString(),
                        TaskTitle = clientReader["TaskTitle"].ToString(),
                        TaskCompletedNotes = clientReader["TaskCompletedNotes"].ToString(),
                        TaskWasAssignedTo = clientReader["TaskWasAssignedTo"].ToString()
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

        public async Task GetByIdOrUsername(IAdminAssignTaskViewModel viewModel, string username)
        {

            try
            {
                SqlModel.SqlConnection.Open();

                SqlCommand clientCommand = new SqlCommand("SELECT * FROM dbo.Task WHERE Username = @Username", SqlModel.SqlConnection);
                clientCommand.Parameters.Add(new SqlParameter("@Username", username));

                SqlDataReader clientReader = clientCommand.ExecuteReader();
                viewModel.TaskCollection.Clear();


                while (clientReader.Read())
                {
                    viewModel.TaskCollection.Add(new AdminAssignTaskModel
                    {
                        Id = Convert.ToInt32(clientReader["Id"]),
                        Username = clientReader["Username"].ToString(),
                        NumberOfAssignedTasks = Convert.ToInt32(clientReader["NumberOfAssignedTasks"]),
                        DateOfAssignedTask = Convert.ToDateTime(clientReader["DateOfAssignedTask"]),
                        TaskIsComplete = Convert.ToBoolean(clientReader["TaskIsComplete"]),
                        TaskDescription = clientReader["TaskDescription"].ToString(),
                        TaskTitle = clientReader["TaskTitle"].ToString(),
                        TaskCompletedNotes = clientReader["TaskCompletedNotes"].ToString(),
                        TaskWasAssignedTo = clientReader["TaskWasAssignedTo"].ToString()
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

        public async Task PostAsync(IAdminAssignTaskViewModel viewModel)
        {
            viewModel.NumberOfAssignedTasks = 1;
            if (viewModel.DateOfAssignedTask == DateTime.MinValue)
            {
                viewModel.DateOfAssignedTask = DateTime.Today;
            }

            try
            {
                SqlModel.SqlConnection.Open();

              using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
              {
                    command.CommandText = "SELECT NumberOfAssignedTasks FROM Task WHERE Username = @Username AND DateOfAssignedTask = @DateOfAssignedTask";
                    command.Parameters.AddWithValue("@Username", viewModel.Username);
                    command.Parameters.AddWithValue("@DateOfAssignedTask", viewModel.DateOfAssignedTask.ToShortDateString());

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var numberOfAssignedTasksInDataBase = int.Parse(reader.GetString(0));
                            viewModel.NumberOfAssignedTasks = numberOfAssignedTasksInDataBase + 1;
                        }
                    }
                    viewModel.NumberOfAssignedTasks = viewModel.NumberOfAssignedTasks;

                    
                    using (SqlCommand insertCommand = new SqlCommand("INSERT INTO dbo.Task VALUES (@Username, @NumberOfAssignedTasks, @DateOfAssignedTask, @TaskIsComplete, @TaskDescription, @TaskTitle, @TaskCompletedNotes, @TaskWasAssignedTo)", SqlModel.SqlConnection))
                    {
                        insertCommand.Parameters.Add(new SqlParameter("Username", viewModel.Username));
                        insertCommand.Parameters.Add(new SqlParameter("NumberOfAssignedTasks", viewModel.NumberOfAssignedTasks));
                        insertCommand.Parameters.Add(new SqlParameter("DateOfAssignedTask", viewModel.DateOfAssignedTask.ToShortDateString()));
                        insertCommand.Parameters.Add(new SqlParameter("TaskIsComplete", viewModel.TaskIsComplete));
                        insertCommand.Parameters.Add(new SqlParameter("TaskDescription", viewModel.TaskDescription ?? string.Empty));
                        insertCommand.Parameters.Add(new SqlParameter("TaskTitle", viewModel.TaskTitle ?? string.Empty));
                        insertCommand.Parameters.Add(new SqlParameter("TaskCompletedNotes", viewModel.TaskCompletedNotes ?? string.Empty));
                        insertCommand.Parameters.Add(new SqlParameter("TaskWasAssignedTo", viewModel.TaskWasAssignedTo ?? string.Empty));

                        await insertCommand.ExecuteNonQueryAsync();

                        await Application.Current.MainPage.DisplayAlert("SUCCESSFULLY ADDED", "YOU JUST ASSIGNED A TASK", "OK");
                        SqlModel.SqlConnection.Close();
                    }
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
                SqlModel.SqlConnection.Open();

                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "UPDATE dbo.Task SET DateOfAssignedTask = ISNULL(@DateOfAssignedTask, DateOfAssignedTask), " +
                                          "TaskDescription = ISNULL(@TaskDescription, TaskDescription), " +
                                          "TaskTitle = ISNULL(@TaskTitle, TaskTitle), " +
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
                SqlModel.SqlConnection.Open();
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
    }
}
