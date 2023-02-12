using Archon.DataAccessLayer;
using Archon.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archon.Tests
{
    public class LoginTest
    {
        [Fact]
        public void Test1()
        {
            var loginVm = new LoginViewModel();
            loginVm.Username = "testname";
            loginVm.Password = "testpass";

            //Arrange setup test
            //Act perform test
            //Assert verify test
            //Assert.Equal();
        }
    }
}
