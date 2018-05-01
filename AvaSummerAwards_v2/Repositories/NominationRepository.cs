using Awards.Helpers;
using Awards.Models;
using Awards.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Awards.Repositories
{
    public class NominationRepository
    {
        public AwardsContext _context { get; private set; }
        public GraphRepository _graphRepository;

        public NominationRepository(AwardsContext context, GraphRepository graphRepository)
        {
            _context = context;
            _graphRepository = graphRepository;
        }

        public async Task Add(NewNominationViewModel nomination, ClaimsPrincipal user, ISession session)
        {
            var userName = user.Identity.Name;
            if(nomination.NominationId != null)
            {
                Update(nomination, userName);
                return;
            }
            var existingNominee = _context.Nominees.Include("Nominations").FirstOrDefault(
                o => o.CategoryID == nomination.CategoryID && o.Email == nomination.Email);
            if (existingNominee != null)
            {
                if (existingNominee.Nominations.Any(o => o.Nominator == userName))
                {
                    throw new Exception($"You have already nominated {nomination.Email} in this category");
                }
                var newNomination = new Nomination
                {
                    NomineeID = existingNominee.ID,
                    Nominator = userName,
                    Reason = nomination.Reason
                };
                _context.Nominations.Add(newNomination);
            }
            else
            {
                var newNominee = await _graphRepository.PopulateNominee(nomination, user, session);
                newNominee.Nominations.Add(new Nomination
                {
                    Nominator = userName,
                    Reason = nomination.Reason
                });

                _context.Nominees.Add(newNominee);
            }
            _context.SaveChanges();
        }

        private void Update(NewNominationViewModel nomination, string userName)
        {
            var existingNomination = Get(nomination.NominationId.GetValueOrDefault(), userName);
            existingNomination.Reason = nomination.Reason;
            _context.Nominations.Update(existingNomination);
            _context.SaveChanges();
        }

        public void Delete(int id, string userName)
        {
            var nomination = Get(id, userName);
            if (nomination != null)
            {
                _context.Nominations.Remove(nomination);
                _context.SaveChanges();
                var nominee = _context.Nominees.Include("Nominations").FirstOrDefault(s => s.ID == nomination.NomineeID);
                if (!nominee.Nominations.Any())
                {
                    _context.Nominees.Remove(nominee);
                    _context.SaveChanges();
                }
            }
        }

        private Nomination Get(int id, string userName)
        {
            return _context.Nominations.FirstOrDefault(s => s.ID == id && s.Nominator == userName);
        }

    }
}
