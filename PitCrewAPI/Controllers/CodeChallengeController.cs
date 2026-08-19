using Microsoft.AspNetCore.Mvc;
using PitCrewModel.Services.Interfaces;

namespace PitCrewAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CodeChallengeController : ControllerBase
    {
        private readonly IDepthCalculationService _depthCalculationService;

        public CodeChallengeController(IDepthCalculationService depthCalculationService)
        {
            _depthCalculationService = depthCalculationService;
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
        [HttpPost("day-one/newline")]
        public async Task<ActionResult<int>> DayOneNewline(IFormFile file)
        {
            // Check if there is a file
            if (file == null || file.Length == 0)
                return BadRequest("No file provided");

            // Check if file is larger than 1MB. A file full of numbers shouldn't be that large. 
            if (file.Length > 1_000_000)
                return BadRequest("File too large");

            // Check if it's .txt and not some virus.exe :-)
            if (Path.GetExtension(file.FileName).ToLower() != ".txt")
                return BadRequest("Only .txt files are allowed");

            using StreamReader reader = new(file.OpenReadStream());
            string rawInput = await reader.ReadToEndAsync();
            // ONLY spaces, digits and spaces are allowed. 
            if (!rawInput.All(c => char.IsDigit(c) || c == '\n' || c == '\r' || c == ' '))
                return BadRequest("File contains invalid characters");

            // Parse numbers into list 
            List<int> numbers = rawInput
                .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x.Trim()))
                .ToList();

            // TODO: Misschien een mooiere response maken, hoeveel items het waren, hoeveel lists, hoelang het duurde?
            return Ok(_depthCalculationService.CountDepthIncreases(numbers));
        }

        //TODO: misschien DTO
        [HttpPost("day-one")]
        public ActionResult<int> DayOne([FromBody] List<int> numbers)
        {
            if (numbers == null || !numbers.Any())
                return BadRequest("No numbers provided");

            // TODO: Misschien een mooiere response maken, hoeveel items het waren, hoeveel lists, hoelang het duurde?
            return Ok(_depthCalculationService.CountDepthIncreases(numbers));
        }
    }
}
