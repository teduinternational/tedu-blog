using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduBlog.Core.Events.LoginSuccessed
{
    public class LoginSuccessedEventHandler : INotificationHandler<LoginSuccessedEvent>
    {
        public Task Handle(LoginSuccessedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
