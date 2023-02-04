using Archon.DataAccessLayer.Models;
using Archon.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Archon.DataAccessLayer
{
    public class LoginRepository : IRepository<ILoginViewModel>, ILoginRepository<ILoginViewModel>
    {
        public LoginRepository(){ }

        public async Task<bool> Login(ILoginViewModel viewModel)
        {
            try
            {
                await SqlModel.SqlConnection.OpenAsync();
                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    string hashedPassword = SqlModel.HashedString(viewModel.Password);
                    command.CommandText = "SELECT COUNT(*) FROM dbo.Login WHERE Username = @Username AND Password = @Password";
                    command.Parameters.AddWithValue("@Username", viewModel.Username);
                    command.Parameters.AddWithValue("@Password", hashedPassword);

                    int count = (int)await command.ExecuteScalarAsync();

                    if (count > 0 && viewModel.CompanyId == 123 || viewModel.CompanyId == 999)
                    {
                        SqlModel.SqlConnection.Close();
                        return true;
                    }
                    if (viewModel.CompanyId != 123)
                    {
                        SqlModel.SqlConnection.Close();
                        await Application.Current.MainPage.DisplayAlert("INVALID COMPANY ID", "PLEASE ENTER YOUR COMPANY'S ID", "OK");
                        viewModel.Username = String.Empty;
                        viewModel.Password = String.Empty;
                        viewModel.CompanyId = null;
                        return false;
                    }
                    else
                    {
                        SqlModel.SqlConnection.Close();
                        await Application.Current.MainPage.DisplayAlert("INVALID CREDENTIALS", "USERNAME OR PASSWORD INCORRECT", "OK");
                        viewModel.Username = String.Empty;
                        viewModel.Password = String.Empty;
                        viewModel.CompanyId = null;
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("ERROR", ex.Message, "OK");
                return false;
            }
        }




        public async Task GetByIdOrUsername(ILoginViewModel model, int id)
        {
            try
            {
                SqlModel.SqlConnection.Open();

                SqlCommand clientCommand = new SqlCommand("SELECT * FROM dbo.Login WHERE Id = @Id", SqlModel.SqlConnection);
                clientCommand.Parameters.AddWithValue("@Id", id);

                SqlDataReader clientReader = clientCommand.ExecuteReader();

                if (clientReader.Read())
                {
                    model.Username = clientReader["Username"].ToString();
                    SqlModel.SqlConnection.Close();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("INVALID ID", "ID NOT FOUND IN DATABASE", "OK");
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


        public async Task<IEnumerable<ILoginViewModel>> GetAllAsync(ILoginViewModel model)
        {
            try
            {
                    SqlModel.SqlConnection.Open();

                    SqlCommand clientCommand = new SqlCommand("SELECT * FROM dbo.Login", SqlModel.SqlConnection);

                    SqlDataReader clientReader = clientCommand.ExecuteReader();
                    model.UserList.Clear();
                    while (clientReader.Read())
                    {
                        model.UserList.Insert(0,new LoginModel
                        {
                            Username = clientReader["Username"].ToString(),
                            Password = clientReader["Password"].ToString(),
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
            return (IEnumerable<ILoginViewModel>)Task.CompletedTask;

        }
        
        public async Task PostAsync(ILoginViewModel viewModel)
        {
            try
            {
                if (string.IsNullOrEmpty(viewModel.Username) || string.IsNullOrEmpty(viewModel.Password))
                {
                    return;
                }

                await SqlModel.SqlConnection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Login WHERE LOWER(Username) = LOWER(@Username) AND LOWER(Password) = LOWER(@Password)", SqlModel.SqlConnection))
                {
                    command.Parameters.Add(new SqlParameter("Username", viewModel.Username));
                    command.Parameters.Add(new SqlParameter("Password", SqlModel.HashedString(viewModel.Password)));

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            reader.Close();
                            SqlModel.SqlConnection.Close();
                            await Application.Current.MainPage.DisplayAlert("NOT VALID", "THIS USERNAME ALREADY EXISTS", "OK");
                            viewModel.CompanyId = null;
                            viewModel.Username = string.Empty;
                            viewModel.Password = string.Empty;
                        }
                        else
                        {
                            reader.Close();

                            using (SqlCommand insertCommand = new SqlCommand("INSERT INTO dbo.Login VALUES (@Username, @Password)", SqlModel.SqlConnection))
                            {
                                insertCommand.Parameters.Add(new SqlParameter("Username", viewModel.Username));
                                insertCommand.Parameters.Add(new SqlParameter("Password", SqlModel.HashedString(viewModel.Password)));

                                await insertCommand.ExecuteNonQueryAsync();

                                await Application.Current.MainPage.DisplayAlert("SUCCESSFULLY ADDED", "YOU MAY NOW LOGIN", "OK");
                                viewModel.Username = string.Empty;
                                viewModel.Password = string.Empty;
                                SqlModel.SqlConnection.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("NOT YET", ex.Message, "OK");
                viewModel.Username = string.Empty;
                viewModel.Password = string.Empty;
            }
            finally
            {
                SqlModel.SqlConnection.Close();
            }

        }
        public async Task DeleteAsync(ILoginViewModel viewModel)
        {
            try
            {
                await SqlModel.SqlConnection.OpenAsync();

                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM dbo.Login WHERE Id =   @Id";
                    command.Parameters.AddWithValue("@Id", viewModel.Id);

                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        command.CommandText = "DELETE FROM dbo.Login WHERE Id = @Id";
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

        public async Task PutAsync(ILoginViewModel viewModel)
        {
            
            try
            {
                await SqlModel.SqlConnection.OpenAsync();

                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM dbo.Login WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", viewModel.Id);

                    int count = (int)await command.ExecuteScalarAsync();
                    if (count > 0)
                    {
                        command.CommandText = "UPDATE Login SET Username = @Username WHERE Id = @Id";
                        command.Parameters.AddWithValue("@Username", viewModel.Username);
                        await command.ExecuteNonQueryAsync();
                        await Application.Current.MainPage.DisplayAlert("SUCCESSFULLY UPDATED", "CLICK OK TO CONTINUE", "OK");
                        SqlModel.SqlConnection.Close();
                        viewModel.Id = null;
                        viewModel.Username = String.Empty;
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("INVALID ID", "ID NOT FOUND IN DATABASE", "OK");
                        SqlModel.SqlConnection.Close();
                        viewModel.Id = null;
                        viewModel.Username = String.Empty;

                    }
                }
                
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("NOT YET", ex.Message, "OK");
            }
        }

        public async Task GetByIdOrUsername(ILoginViewModel viewModel, string username)
        {
            try
            {
                SqlModel.SqlConnection.Open();

                SqlCommand clientCommand = new SqlCommand("SELECT * FROM dbo.Login WHERE Username = @Username", SqlModel.SqlConnection);
                clientCommand.Parameters.AddWithValue("@Username", username);

                SqlDataReader clientReader = clientCommand.ExecuteReader();

                if (clientReader.Read())
                {
                    viewModel.Username = clientReader["Username"].ToString();
                    SqlModel.SqlConnection.Close();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("INVALID Username", "Username NOT FOUND IN DATABASE", "OK");
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
    }
}
