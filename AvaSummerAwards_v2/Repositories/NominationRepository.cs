using Awards.Helpers;
using Awards.Models;
using Awards.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Awards.Repositories
{
    public class NominationRepository
    {
        public AwardsContext _context { get; private set; }

        public NominationRepository(AwardsContext context)
        {
            _context = context;
        }

        public void Add(NewNominationViewModel nomination, string userName)
        {
            var existingNominee = _context.Nominees.FirstOrDefault(
                o => o.CategoryID == nomination.CategoryID && o.Email == nomination.Email);
            if (existingNominee != null)
            {
                if (existingNominee.Nominations.Any(o => o.Nominator == userName))
                {
                    /*var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    response.ReasonPhrase = "You have already nominated this person in this category";
                    return ResponseMessage(response);*/
                    return;
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
                var newNominee = new Nominee
                {
                    CategoryID = nomination.CategoryID,
                    Email = nomination.Email,
                    //Name = nominee.NomineeName, //Need to get from GraphAPI
                    //Image = nominee.NoineeImage, //Need to get from GraphAPI
                    Nominations = new List<Nomination>()
                };
                newNominee.Nominations.Add(new Nomination
                {
                    Nominator = userName,
                    Reason = nomination.Reason
                });

                _context.Nominees.Add(newNominee);
            }
            _context.SaveChanges();
        }

        public void Update(NewNominationViewModel nomination, string userName)
        {
            var existingNomination = Get(nomination.NominationId.GetValueOrDefault(), userName);
            existingNomination.Reason = nomination.Reason;
            _context.Nominations.Update(existingNomination);
            _context.SaveChanges();

            /*if (existingNomination != null)
            {              
                var currentNominee = _context.Nominees.Include("Nominations").Where(o => o.ID == existingNomination.NomineeID).FirstOrDefault();
                //Check if nominee has changed
                if (currentNominee.Email != nomination.Email)
                {
                    _context.Nominations.Remove(existingNomination);
                    if (!currentNominee.Nominations.Any())
                    {
                        _context.Nominees.Remove(currentNominee);
                    }
                    _context.SaveChanges();
                    Add(nomination, userName);
                }
                else
                {
                    existingNomination.Reason = nomination.Reason;
                    existingNomination.
                }
            }*/
        }

        public void Delete(int id, string userName)
        {
            var nomination = Get(id, userName);
            if (nomination != null)
            {
                _context.Nominations.Remove(nomination);
                _context.SaveChanges();
            }
        }

        private Nomination Get(int id, string userName)
        {
            return _context.Nominations.FirstOrDefault(s => s.ID == id && s.Nominator == userName);
        }

    }
}
