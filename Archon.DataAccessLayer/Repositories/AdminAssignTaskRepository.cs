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
                //viewModel.TaskCollection.Clear();
                while (clientReader.Read())
                {
                    viewModel.TaskCollection.Insert (0, new AdminAssignTaskModel
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

                SqlCommand clientCommand = new SqlCommand("SELECT * FROM dbo.Task WHERE Username = @Username", SqlModel.SqlConnection);
                clientCommand.Parameters.AddWithValue("@Username", username);

                SqlDataReader clientReader = clientCommand.ExecuteReader();
                //viewModel.TaskCollection.Clear();


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
                        TaskWasAssignedTo = clientReader["TaskWasAssignedTo"].ToString()
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
                clientCommand.Parameters.Add(new SqlParameter("@Username", viewModel.Username));

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
            try
            {
                SqlModel.SqlConnection.Open();
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
                using (var command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = @"UPDATE [dbo].[Task]
                                   SET [Username] = @Username, [NumberOfAssignedTasks] = @NumberOfAssignedTasks, [DateOfAssignedTask] = @DateOfAssignedTask,[TaskIsComplete] = @TaskIsComplete, [TaskDescription] = @TaskDescription,[TaskTitle] = @TaskTitle,[TaskCompletedNotes] = @TaskCompletedNotes, [TaskWasAssignedTo] = @TaskWasAssignedTo
                                   WHERE [Username] = @Username";
                    command.Parameters.AddWithValue("@Username", viewModel.Username != null ? viewModel.Username : "");
                    command.Parameters.AddWithValue("@NumberOfAssignedTasks", viewModel.NumberOfAssignedTasks != null ? viewModel.NumberOfAssignedTasks.ToString() : "");
                    command.Parameters.AddWithValue("@DateOfAssignedTask", viewModel.DateOfAssignedTask != null ? viewModel.DateOfAssignedTask.ToString() : "");
                    command.Parameters.AddWithValue("@TaskIsComplete", viewModel.TaskIsComplete != null ? viewModel.TaskIsComplete.ToString() : "");
                    command.Parameters.AddWithValue("@TaskDescription", viewModel.TaskDescription != null ? viewModel.TaskDescription : "");
                    command.Parameters.AddWithValue("@TaskTitle", viewModel.TaskTitle != null ? viewModel.TaskTitle : "");
                    command.Parameters.AddWithValue("@TaskCompletedNotes", viewModel.TaskCompletedNotes != null ? viewModel.TaskCompletedNotes : "");
                    command.Parameters.AddWithValue("@TaskWasAssignedTo", viewModel.TaskWasAssignedTo != null ? viewModel.TaskWasAssignedTo : "");


                    await command.ExecuteNonQueryAsync();
                    await Application.Current.MainPage.DisplayAlert("SUCCESSFULLY UPDATED", "YOU JUST ASSIGNED A TASK", "OK");
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

        public async Task UpdateAssignedTaskEmployee(IAdminAssignTaskViewModel viewModel, string username)
        {
            try
            {
                SqlModel.SqlConnection.Open();
                using (var command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "UPDATE Task SET [TaskIsComplete] = @TaskIsComplete,[TaskCompletedNotes] = @TaskCompletedNotes WHERE Username = @Username";
                    command.Parameters.AddWithValue("@Username", viewModel.Username);

                    if (viewModel.TaskIsComplete != true)
                        command.Parameters.AddWithValue("@TaskIsComplete", viewModel.TaskIsComplete);

                    if (viewModel.TaskCompletedNotes != null)
                        command.Parameters.AddWithValue("@TaskCompletedNotes", viewModel.TaskCompletedNotes);

                    await command.ExecuteNonQueryAsync();
                    await Application.Current.MainPage.DisplayAlert("SUCCESSFULLY UPDATED", "YOU JUST ASSIGNED A TASK", "OK");
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
