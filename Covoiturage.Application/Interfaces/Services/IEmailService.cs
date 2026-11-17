using System;
using System.Collections.Generic;
using System.Text;

namespace Covoiturage.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendVerificationCodeAsync(string toEmail, string userName, string code);
}
