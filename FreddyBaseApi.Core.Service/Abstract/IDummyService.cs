using FreddyBaseApi.Core.Model.Models;
using FreddyBaseApi.Core.Repositories.Base.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreddyBaseApi.Core.Service.Abstract
{
    public interface IDummyService
    {
        IEnumerable<DummyEntity> GetCustomDummyData();
        IRepositoryWrapper RepositoryWrapper { get; }
    }
}
