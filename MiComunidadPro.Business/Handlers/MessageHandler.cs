using MiComunidadPro.Common;
using MiComunidadPro.Business.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using MiComunidadPro.Common.Dtos;

namespace MiComunidadPro.Business.Handlers
{
    public class MessageHandler : IMessageHandler
    {
        private readonly IMessagesResourceHandler _ResourceHandler;

        public MessageHandler(IMessagesResourceHandler resourceHandler)
        {
            this._ResourceHandler = resourceHandler;
        }

        public IEnumerable<KeyValueDto<string>> GetMessages(params string[] messageCodes)
        {
            //Validate input parameter
            if (messageCodes == null || messageCodes.Length == 0)
                throw new ArgumentException("No se pudo obtener los mensajes de validación");

            var data = messageCodes.Select(
                key => new KeyValueDto<string>
                {
                    Id = key,
                    Name = _ResourceHandler.GetString(key),
                });

            return data;
        }

        public KeyValueDto<string> GetMessage(string messageCode)
        {
            //Validate input parameter
            if (messageCode == null || messageCode.Length == 0)
                throw new ArgumentException("No se pudo obtener los mensajes de validación");

            var data = new KeyValueDto<string>
            {
                Id = messageCode,
                Name = _ResourceHandler.GetString(messageCode),
            };

            return data;
        }

        public KeyValueDto<string, string> GetMessageWithKey(string messageCode)
        {
            //Validate input parameter
            if(messageCode == null || messageCode.Length == 0)
                throw new ArgumentException("No se pudo obtener los mensajes de validación");

            var data = new KeyValueDto<string, string>
            {
                Id = messageCode,
                Name = _ResourceHandler.GetString(messageCode),
            };

            return data;
        }
    }
}
