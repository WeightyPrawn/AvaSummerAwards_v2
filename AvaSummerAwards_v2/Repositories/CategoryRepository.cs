using Awards.Helpers;
using Awards.Models;
using Awards.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Awards.Repositories
{
    public class CategoryRepository
    {
        public AwardsContext _context { get; private set; }

        public CategoryRepository(AwardsContext context)
        {
            _context = context;
        }

        public List<CategoryViewModel> GetCategoriesForUser(string userName)
        {
            var response = _context.Categories
                .Select(o => new CategoryViewModel()
                {
                    ID = o.ID,
                    Name = o.Name,
                    Description = o.Description,
                    Nominees = o.Nominees.Select(p => new NomineeViewModel()
                    {
                        ID = p.ID,
                        CategoryID = p.CategoryID,
                        Email = p.Email,
                        Name = p.Name,
                        Image = p.Image,
                        Nominations = p.Nominations
                            .Select(r => new NominationViewModel()
                            {
                                ID = r.ID,
                                NomineeID = r.NomineeID,
                                Reason = r.Reason
                            }).ToList(),
                        Vote = p.Votes
                            .Where(q => q.Voter == userName)
                            .Select(r => new VoteViewModel()
                            {
                                ID = r.ID,
                                NomineeID = r.NomineeID,
                                Voter = r.Voter
                            }).FirstOrDefault()
                    }).ToList()
                }).ToList();
            response.ForEach(o => o.UserHasVoted = o.Nominees.Any(p => p.Vote != null));
            response.ForEach(o => o.Nominees = o.Nominees.Shuffle().ToList());
            return response;
        }

        public List<EditCategoryViewModel> GetCategories()
        {
            return _context.Categories
                .Include("Nominees")
                .Select(o => new EditCategoryViewModel()
                {
                    ID = o.ID,
                    Name = o.Name,
                    Description = o.Description,
                    NumberOfNominees = o.Nominees.Count()
                }).ToList();
        }

        public void Add(EditCategoryViewModel data)
        {
            _context.Categories.Add(new Category
            {
                Name = data.Name,
                Description = data.Description
            });
            _context.SaveChanges();
        }

        public void Update(EditCategoryViewModel data)
        {
            var entity = _context.Categories.Find(data.ID);
            if(entity != null)
            {
                entity.Name = data.Name;
                entity.Description = data.Description;
            }
            _context.Categories.Update(entity);
            _context.SaveChanges();
        }

        public void Remove(int categoryId)
        {
            var category = _context.Categories.Find(categoryId);
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
