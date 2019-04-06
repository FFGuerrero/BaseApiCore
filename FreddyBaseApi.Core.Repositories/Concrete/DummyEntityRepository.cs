using FreddyBaseApi.Core.Model;
using FreddyBaseApi.Core.Model.Models;
using FreddyBaseApi.Core.Repositories.Abstract;
using FreddyBaseApi.Core.Repositories.Base.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreddyBaseApi.Core.Repositories.Concrete
{
    public class DummyEntityRepository : RepositoryBase<DummyEntity>, IDummyEntityRepository
    {
        protected override string FriendlyName => "DummyEntity";
        protected override Func<DummyEntity, object> OrderBySelector => x => x.DommyEntityId;
        protected override Func<DummyEntity, object> KeySelector => x => x.DommyEntityId;

        public DummyEntityRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            Context = applicationDbContext;
        }

        public bool DummyProcedureCall() => true;
    }
}
