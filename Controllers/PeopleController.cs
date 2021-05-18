using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleSportsSandbox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableQuery]
    public class PeopleController : ODataController
    {

        [HttpGet]
        public IQueryable<Person> Get(ODataQueryOptions opts)
        {

            var myDB = new MyDbContext();
            var myReturn = myDB.People.Include(x => x.Sports).AsQueryable();
            return myReturn;
        }
    }
}
