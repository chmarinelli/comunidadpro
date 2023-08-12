using MiComunidadPro.Business.Contracts;
using MiComunidadPro.Web.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiComunidadPro.Web.Infrastructure.Attributes
{
    public sealed class CustomRequiredAttribute : ValidationAttribute
    {
        private string IdMessage { get; set; }
        private ClientModelValidationContext CurrentContext { get; set; }

        public CustomRequiredAttribute(string idMessage)
        {
            IdMessage = idMessage;
        }

        readonly RequiredAttribute _InnerAttribute = new RequiredAttribute();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(!_InnerAttribute.IsValid(value))
            {
                var messageHandler = (IMessageHandler)validationContext.GetService(typeof(IMessageHandler));

                throw new ArgumentException(messageHandler.GetMessage(IdMessage).Name);
            }

            return ValidationResult.Success;
        }
    }
}
