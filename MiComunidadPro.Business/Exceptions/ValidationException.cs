using MiComunidadPro.Business.Entities;
using MiComunidadPro.Common.Dtos;
using System;
using System.Runtime.Serialization;

namespace MiComunidadPro.Business.Exceptions
{
    public class ValidationException : Exception
    {
        [DataMember]
        public string Code { get; set; }

        public ValidationException(KeyValueDto<string> message)
            : base(message.Name)
        {
            Code = message.Id;
        }

        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string code, string message)
            : base(message)
        {
            Code = code;
        }

        public ValidationException(KeyValueDto<string> message, Exception exception) : base(message.Name, exception)
        {
            Code = message.Id;
        }

        public ValidationException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public ValidationException(string code, string message, Exception exception)
            : base(message, exception)
        {
            Code = code;
        }
    }
}
