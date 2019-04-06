using FreddyBaseApi.Core.Model.Models;
using FreddyBaseApi.Core.Repositories.Base.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreddyBaseApi.Core.Repositories.Abstract
{
    public interface IDummyEntityRepository : IRepository<DummyEntity>
    {
        bool DummyProcedureCall();
    }
}
