using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lavalamp
{
    using ServiceStack.ServiceInterface;
    using lavalampData;

    public class authenticationService : RestServiceBase<IUser>, IAuthenticationService
    {
        public void Login(string username)
        {
            throw new NotImplementedException();
        }

        public override object OnPost(IUser login)
        {
            var appHost = this.GetAppHost();
          //  appHost.RegisterAs<>();
            return null;
        }
    }
}