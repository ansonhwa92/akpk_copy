using FEP.Helper;
using FEP.Model;
using FEP.WebApiModel.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FEP.WebApi.Api.Administration
{
    [Route("api/Administration/State")]
    public class StateController : ApiController
    {

        private DbEntities db = new DbEntities();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: api/State
        public List<StateModel> Get()
        {
            var states = db.State.Select(s => new StateModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return states;

        }

    }
}
