﻿using Synaplic.Inventory.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Synaplic.Inventory.Transfer.Responses.Audit;

namespace Synaplic.Inventory.Application.Interfaces.Services
{
    public interface IAuditService
    {
        Task<IResult<IEnumerable<AuditResponse>>> GetCurrentUserTrailsAsync(string userId);

        Task<IResult<string>> ExportToExcelAsync(string userId, string searchString = "", bool searchInOldValues = false, bool searchInNewValues = false);
    }
}