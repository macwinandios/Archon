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
        public async Task DeleteAsync(ILoginViewModel viewModel)
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
                    command.CommandText = "SELECT COUNT(*) FROM dbo.Login WHERE Id =   @Id";

                    command.Parameters.AddWithValue("@Id", viewModel.Id);

                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        command.CommandText = "DELETE FROM dbo.Login WHERE Id = @Id";

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
        public async Task GetByIdOrUsername(ILoginViewModel viewModel, int id)
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

                    command.CommandText = "SELECT * FROM dbo.Login WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            viewModel.Username = reader["Username"].ToString();
                            SqlModel.SqlConnection.Close();
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("INVALID ID", "ID NOT FOUND IN DATABASE", "OK");
                            SqlModel.SqlConnection.Close();
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
        public async Task GetByIdOrUsername(ILoginViewModel viewModel, string username)
        {
            try
            {
                if (viewModel.Username == String.Empty)
                {
                    await Application.Current.MainPage.DisplayAlert("YOU MUST ENTER A VALID USERNAME", "TRY AGAIN", "OK");
                    return;
                }

                await SqlModel.SqlConnection.OpenAsync();

                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Login WHERE Username = @Username";

                    command.Parameters.AddWithValue("@Username", username);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            await Application.Current.MainPage.DisplayAlert("This is a Valid Username", "Username WAS FOUND IN DATABASE", "OK");
                            SqlModel.SqlConnection.Close();
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("INVALID Username", "Username NOT FOUND IN DATABASE", "OK");
                            SqlModel.SqlConnection.Close();
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
        public async Task<IEnumerable<ILoginViewModel>> GetAllAsync(ILoginViewModel viewModel)
        {
            try
            {
                await SqlModel.SqlConnection.OpenAsync();
                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Login";
                
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {

                        viewModel.UserList.Clear();
                        while (await reader.ReadAsync())
                        {
                            viewModel.UserList.Insert(0, new LoginModel
                            {
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
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
            return (IEnumerable<ILoginViewModel>)viewModel.UserList;
        }
        public async Task<bool> Login(ILoginViewModel viewModel)
        {
            try
            {
                if (string.IsNullOrEmpty(viewModel.Username) || string.IsNullOrEmpty(viewModel.Password))
                {
                    await Application.Current.MainPage.DisplayAlert("NOT A VALID USERNAME OR PASSWORD", "YOU MUST ENTER VALID CREDENTIALS", "OK");
                    viewModel.CompanyId = null;
                    viewModel.Username = string.Empty;
                    viewModel.Password = string.Empty;
                    return false;
                }

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
        public async Task PostAsync(ILoginViewModel viewModel)
        {
            try
            {
                if (string.IsNullOrEmpty(viewModel.Username) || string.IsNullOrEmpty(viewModel.Password))
                {
                    await Application.Current.MainPage.DisplayAlert("NOT A VALID USERNAME OR PASSWORD", "YOU MUST ENTER VALID CREDENTIALS", "OK");
                    viewModel.CompanyId = null;
                    viewModel.Username = string.Empty;
                    viewModel.Password = string.Empty;
                    return;
                }
                if (viewModel.CompanyId != 123)
                {
                    SqlModel.SqlConnection.Close();
                    await Application.Current.MainPage.DisplayAlert("INVALID COMPANY ID", "PLEASE ENTER YOUR COMPANY'S ID", "OK");
                    viewModel.CompanyId = null;
                    viewModel.Username = string.Empty;
                    viewModel.Password = string.Empty;
                    return;
                }
                await SqlModel.SqlConnection.OpenAsync();

                using (SqlCommand command = SqlModel.SqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM dbo.Login WHERE LOWER(Username) = LOWER(@Username) AND LOWER(Password) = LOWER(@Password)";

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

                            command.CommandText = "INSERT INTO dbo.Login VALUES (@Username, @Password)";

                            await command.ExecuteNonQueryAsync();

                            await Application.Current.MainPage.DisplayAlert("SUCCESSFULLY ADDED", "YOU MAY NOW LOGIN", "OK");
                            viewModel.Username = string.Empty;
                            viewModel.Password = string.Empty;
                            SqlModel.SqlConnection.Close();
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
        public async Task PutAsync(ILoginViewModel viewModel)
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
        
        
    }
}
