﻿using System;
using Lapiwe.OnderhoudService.Domain;

namespace Lapiwe.OnderhoudService.Infrastructure
{
    public interface IRepository
    {
        void Insert(OnderhoudsOpdracht opdracht);
    }
}
