using Awards.Helpers;
using Awards.Models;
using Awards.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Awards.Repositories
{
    public class VoteRepository
    {
        public AwardsContext _context { get; private set; }

        public VoteRepository(AwardsContext context)
        {
            _context = context;
        }

        public void Add(int nomineeId, string userName)
        {
            if (!UserHasVotedInCategory(nomineeId, userName))
            {
                _context.Votes.Add(new Vote()
                {
                    NomineeID = nomineeId,
                    Voter = userName
                });
                _context.SaveChanges();
            }
        }

        public void Delete(int id, string userName)
        {
            var vote = Get(id, userName);
            if (vote != null)
            {
                _context.Votes.Remove(vote);
                _context.SaveChanges();
            }
        }

        private Vote Get(int id, string userName)
        {
            return _context.Votes.FirstOrDefault(s => s.ID == id && s.Voter == userName);
        }

        private bool UserHasVotedInCategory(int nomineeId, string userName)
        {
            var categoryId = _context.Nominees.Find(nomineeId).CategoryID;
            var category = _context.Categories.Where(o => o.ID == categoryId).Include("Nominees.Votes").FirstOrDefault();
            return category.Nominees.Any(s => s.Votes != null && s.Votes.Any(t => t.Voter == userName));
        }
    }
}
