using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreddyBaseApi.Core.Model;
using FreddyBaseApi.Core.Model.Models;
using FreddyBaseApi.Core.Repositories.Base.Abstract;
using FreddyBaseApi.Core.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace FreddyBaseApi.Controllers
{
    /// <summary>
    /// Dummy controller for dummy entities
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DummyController : BaseApiController<DummyEntity>
    {
        public DummyController(IRepositoryWrapper repositoryWrapper)
        {
            Repository = repositoryWrapper.DummyEntity;

            //Seed data
            if (Repository.GetAll().Count() == 0)
            {
                SeedData();
            }

            void SeedData()
            {
                var dummyNestedEntity1 = new DummyNestedEntity
                {
                    Name = "Dummy Name 1",
                    Description = "Dummy desc 1",
                    DateCreated = DateTime.Now
                };
                repositoryWrapper.DummyNestedEntity.Add(dummyNestedEntity1);

                var dummyNestedEntity2 = new DummyNestedEntity
                {
                    Name = "Dummy Name 2",
                    Description = "Dummy desc 2",
                    DateCreated = DateTime.Now
                };
                repositoryWrapper.DummyNestedEntity.Add(dummyNestedEntity2);

                Repository.Add(new DummyEntity()
                {
                    Name = "Dummy Name 1",
                    Description = "Dummy desc 1",
                    DateCreated = DateTime.Now,
                    DommyNestedEntityId = dummyNestedEntity1.DummyNestedEntityId
                });

                Repository.Add(new DummyEntity()
                {
                    Name = "Dummy Name 2",
                    Description = "Dummy desc 2",
                    DateCreated = DateTime.Now,
                    DommyNestedEntityId = dummyNestedEntity2.DummyNestedEntityId
                });
            }
        }

        public override ActionResult<IEnumerable<DummyEntity>> GetAll()
        {
            var result = Repository.GetAllBy(x => true, x => x.DummyNestedEntity).ToList();
            return result;
        }
    }
}
