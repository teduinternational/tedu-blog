using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduBlog.Core.Events.LoginSuccessed
{
    public class LoginSuccessedEvent : INotification
    {
        public string Email { get; set; }

        public LoginSuccessedEvent(string email)
        {
            Email = email;
        }
    }
}
