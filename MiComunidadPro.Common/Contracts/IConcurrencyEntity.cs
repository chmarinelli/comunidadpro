﻿namespace MiComunidadPro.Common.Contracts
{
    public interface IConcurrencyEntity
    {
        byte[] RowVersion { get; set; }
    }
}
