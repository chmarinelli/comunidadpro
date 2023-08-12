using MiComunidadPro.Common.Contracts;
using MiComunidadPro.Web.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace MiComunidadPro.Web.Services
{

    public class SMTPEmailSender : ISMTPEmailSender
    {
        private readonly EmailSetting _EmailSetting;
        private readonly IDataRepositoryFactory _DataRepositoryFactory;

        public SMTPEmailSender(EmailSetting emailSetting, IDataRepositoryFactory dataRepositoryFactory)
        {
            _EmailSetting = emailSetting;
            _DataRepositoryFactory = dataRepositoryFactory;
        }
    }
}