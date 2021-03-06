﻿using Lapiwe.Common.Infastructure;
using Lapiwe.GMS.FrontEnd.Agents;
using Lapiwe.GMS.FrontEnd.DAL;
using Lapiwe.GMS.FrontEnd.Entities;
using Lapiwe.GMS.FrontEnd.ViewModels;
using Lapiwe.IS.RDW.Export.Commands;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lapiwe.GMS.FrontEnd.Controllers
{
    public class KeuringsVerzoekController : Controller
    {
        private IRDWAgent _agent;
        private FrontendContext _context;

        public KeuringsVerzoekController(IRDWAgent agent, FrontendContext context)
        {
            _agent = agent;
            _context = context;
        }

        [HttpGet]
        public IActionResult Versturen()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Versturen(string voornaam, string tussenvoegsel, string achternaam, string kenteken, int kilometerstand)
        {
            string volledigeNaam = $"{voornaam.First()}. {tussenvoegsel} {achternaam}";

            _agent.KeuringsVerzoek(new KeuringsVerzoekCommand() {
                Kenteken = kenteken,
                Klantnaam = volledigeNaam,
                Kilometerstand = kilometerstand,
                OnderhoudsGuid = Guid.NewGuid()
            });

            return RedirectToAction("Overzicht");
        }

        [HttpGet]
        public IActionResult Overzicht()
        {
            IEnumerable<KeuringsVerzoek> verzoeken = _context.KeuringsVerzoeken.ToList();

            KeuringsVerzoekenViewModel model = new KeuringsVerzoekenViewModel(verzoeken);

            return View(model);
        }
    }
}
