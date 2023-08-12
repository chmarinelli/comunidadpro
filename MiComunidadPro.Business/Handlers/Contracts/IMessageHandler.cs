using System.Collections.Generic;
using MiComunidadPro.Common.Dtos;

namespace MiComunidadPro.Business.Contracts
{
    public interface IMessageHandler
    {
        IEnumerable<KeyValueDto<string>> GetMessages(params string[] messageCodes);

        KeyValueDto<string> GetMessage(string messageCode);

        KeyValueDto<string, string> GetMessageWithKey(string messageCode);
    }
}
