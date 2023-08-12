using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiComunidadPro.Business.Engines.Contracts;
using MiComunidadPro.Business.Entities.Dtos;
using MiComunidadPro.Common.Contracts;

namespace MiComunidadPro.Business.Engines
{
    public class AccountEngine : IAccountEngine
    {
        private readonly IDataRepositoryFactory _DataRepositoryFactory;
        public AccountEngine(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        public async ValueTask LoginAsync(LoginDto dto) 
        {

        }
    }
}