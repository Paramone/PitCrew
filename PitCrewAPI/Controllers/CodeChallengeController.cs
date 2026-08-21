using Microsoft.AspNetCore.Mvc;
using PitCrewModel.Services.Interfaces;

namespace PitCrewAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CodeChallengeController : ControllerBase
    {
        private readonly IChallengeServiceFactory _challengeFactory;

        public CodeChallengeController(IChallengeServiceFactory challengeFactory)
        {
            _challengeFactory = challengeFactory;
        }

        /// <summary>
        /// 
        ///                                      !!!! ---- NOTE TO PITCREW ---- !!!!
        ///                         Mijn input vanuit Advent is een lijst van cijfers, niet comma-separated. 
        ///         Ik weet niet of ik er dus van uit mag gaan dat het netjes in een array wordt meegegeven. Hierdoor dus deze methode.
        ///             Als ik daar wel van uit mag gaan, zie DayOne endpoint, waar netjes een lijst meegegeven wordt.
        ///                                      !!!! ---- NOTE TO PITCREW ---- !!!!
        /// 
        /// Day one challenge of adventofcode.com. 
        /// This method counts the number of times an integer is higher than the previous integer in the array.
        /// </summary>
        /// <param name="file">A file uploaded by the user, as .txt format.</param>
        /// <returns></returns>
        [HttpPost("day/{day}/part/{part}/newline")]
        public async Task<ActionResult<int>> SolveNewline(int day, int part, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided");

            if (file.Length > 1_000_000)
                return BadRequest("File too large");

            if (Path.GetExtension(file.FileName).ToLower() != ".txt")
                return BadRequest("Only .txt files are allowed");

            using StreamReader reader = new(file.OpenReadStream());
            string rawInput = await reader.ReadToEndAsync();

            if (!rawInput.All(c => char.IsDigit(c) || c == '\n' || c == '\r' || c == ' '))
                return BadRequest("File contains invalid characters");

            var challenge = _challengeFactory.GetChallenge(day, part);
            if (challenge == null)
                return NotFound($"Day {day} part {part} is not implemented");

            try
            {
                List<int> numbers = rawInput
                    .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x.Trim()))
                    .ToList();

                return Ok(challenge.Solve(numbers));
            }
            catch (FormatException)
            {
                return BadRequest("File contains invalid data");
            }
        }

        //TODO: misschien DTO
        [HttpPost("day/{day}/part/{part}")]
        public ActionResult<int> Solve(int day, int part, [FromBody] List<int> numbers)
        {
            if (numbers == null || !numbers.Any())
                return BadRequest("No numbers provided");

            var challenge = _challengeFactory.GetChallenge(day, part);
            if (challenge == null)
                return NotFound($"Day {day} part {part} is not implemented");

            // TODO: Misschien een mooiere response maken, hoeveel items het waren, hoeveel lists, hoelang het duurde?
            return Ok(challenge.Solve(numbers));
        }
    }
}
