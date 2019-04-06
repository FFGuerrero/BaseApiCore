using FreddyBaseApi.Core.Model;
using FreddyBaseApi.Core.Model.Models;
using FreddyBaseApi.Core.Repositories.Abstract;
using FreddyBaseApi.Core.Repositories.Base.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreddyBaseApi.Core.Repositories.Concrete
{
    public class DummyNestedEntityRepository : RepositoryBase<DummyNestedEntity>, IDummyNestedEntityRepository
    {
        protected override string FriendlyName => "DummyEntity";
        protected override Func<DummyNestedEntity, object> OrderBySelector => x => x.DummyNestedEntityId;
        protected override Func<DummyNestedEntity, object> KeySelector => x => x.DummyNestedEntityId;

        public DummyNestedEntityRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            Context = applicationDbContext;
        }

        public bool DummyProcedureCall() => true;
    }
}
