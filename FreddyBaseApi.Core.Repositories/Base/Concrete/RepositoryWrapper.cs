using FreddyBaseApi.Core.Model;
using FreddyBaseApi.Core.Repositories.Abstract;
using FreddyBaseApi.Core.Repositories.Base.Abstract;
using FreddyBaseApi.Core.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreddyBaseApi.Core.Repositories.Base.Concrete
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ApplicationDbContext _applicationDbContext;
        private IDummyEntityRepository _dummyEntity;
        private IDummyNestedEntityRepository _dummyNestedEntity;

        public RepositoryWrapper(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IDummyEntityRepository DummyEntity
        {
            get
            {
                if (_dummyEntity == null)
                {
                    _dummyEntity = new DummyEntityRepository(_applicationDbContext);
                }

                return _dummyEntity;
            }
        }

        public IDummyNestedEntityRepository DummyNestedEntity
        {
            get
            {
                if (_dummyNestedEntity == null)
                {
                    _dummyNestedEntity = new DummyNestedEntityRepository(_applicationDbContext);
                }

                return _dummyNestedEntity;
            }
        }
    }
}
