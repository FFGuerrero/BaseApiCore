using FreddyBaseApi.Core.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreddyBaseApi.Core.Repositories.Base.Abstract
{
    public interface IRepositoryWrapper
    {
        IDummyEntityRepository DummyEntity { get; }
        IDummyNestedEntityRepository DummyNestedEntity { get; }
    }
}
