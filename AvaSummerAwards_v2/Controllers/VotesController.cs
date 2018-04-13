using System.Diagnostics;
using Awards.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Awards.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class VotesController : Controller
    {
        private VoteRepository _voteRepository { get; set; }

        public VotesController(VoteRepository voteRepository)
        {
            _voteRepository = voteRepository;
        }

        [HttpPost("{nomineeId:int}")]
        public IActionResult Post(int nomineeId)
        {
            _voteRepository.Add(nomineeId, User.Identity.Name);
            return Ok();
        }

        [HttpDelete("{voteId:int}")]
        public IActionResult Delete(int voteId)
        {
            _voteRepository.Delete(voteId, User.Identity.Name);
            return Ok();
        }
    }
}
