﻿using System;
using System.Threading.Tasks;
using ChallengeNet.Core.Models.Response;

namespace ChallengeNet.Core.Interfaces
{
    public interface INyTimesWorker
    {
        Task<CoreResult<string>> ExecuteAsync();
    }
}

