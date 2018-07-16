using BookStore.WebUI.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace BookStore.WebUI.Infrastructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider //162
    {
        public bool Authenticate(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username, password);
            if (result)
                FormsAuthentication.GetAuthCookie(username, false);

            return result;
        }
    }
}