﻿using System.Collections.Generic;

namespace Complaints_WPF.Services.Interfaces
{
    public interface IReadProsecutorService
    {
        IEnumerable<string> LoadProsecutors();
    }
}
