using FreddyBaseApi.Core.Model.Models;
using FreddyBaseApi.Core.Repositories.Base.Abstract;
using FreddyBaseApi.Core.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreddyBaseApi.Core.Service.Concrete
{
    public class DummyService : IDummyService
    {
        public IRepositoryWrapper RepositoryWrapper { get; }
        public DummyService(IRepositoryWrapper repositoryWrapper)
        {
            RepositoryWrapper = repositoryWrapper;
        }

        public IEnumerable<DummyEntity> GetCustomDummyData()
        {
            return RepositoryWrapper.DummyEntity.GetAll().ToList();
        }
    }
}
