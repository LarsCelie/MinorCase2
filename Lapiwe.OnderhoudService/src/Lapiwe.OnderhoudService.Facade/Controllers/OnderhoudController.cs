
using Lapiwe.Common.Domain;
using Lapiwe.Common.Infastructure;
using Lapiwe.OnderhoudService.Domain;
using Lapiwe.OnderhoudService.Export;
using Lapiwe.OnderhoudService.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace Lapiwe.OnderhoudService.Facade.Controllers
{
    [Route("api/[controller]")]
    public class OnderhoudController : Controller
    {
        private IRepository repository;
        private IEventPublisher publisher;

        public OnderhoudController(IRepository repo, IEventPublisher pub)
        {
            repository = repo;
            publisher = pub;
        }

        // POST api/onderhoud/aanmelden
        [Route("/aanmelden")]
        [HttpPost]
        [ProducesResponseType(typeof(OkResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
        public IActionResult MaakNieuwOnderhoudsOpdracht([FromBody]RegisteerOnderhoudOpdrachtCommand command)
        {
            if (ModelState.IsValid && command != null)
            {
                OnderhoudsOpdracht opdracht = new OnderhoudsOpdracht
                {
                    AanmeldDatum = command.AanmeldDatum,
                    Apk = command.Apk,
                    AutoGuid = command.AutoGuid,
                    Kilometerstand = command.Kilometerstand,
                    KlantGuid = command.KlantGuid,
                    OpdrachtOmschrijving = command.OpdrachtOmschrijving
                };

                repository.Insert(opdracht);

                var OnderhoudEvent = new OnderhoudsOpdrachtGeregistreerdEvent
                {
                    AanmeldDatum = opdracht.AanmeldDatum,
                    Apk = opdracht.Apk,
                    AutoGuid = opdracht.AutoGuid,
                    Kilometerstand = opdracht.Kilometerstand,
                    KlantGuid = opdracht.KlantGuid,
                    OnderhoudsOpdrachtGuid = opdracht.Guid,
                    OpdrachtOmschrijving = opdracht.OpdrachtOmschrijving
                };

                publisher.Publish(OnderhoudEvent);
                return Ok();
            }

            return BadRequest();
        }

        
    }

}