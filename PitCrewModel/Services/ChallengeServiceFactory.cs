using PitCrewModel.Services.DayOne;
using PitCrewModel.Services.Interfaces;

namespace PitCrewModel.Services
{
    public class ChallengeServiceFactory : IChallengeServiceFactory
    {
        public IChallengeService? GetChallenge(int day, int part)
        {
            return (day, part) switch
            {
                (1, 1) => new DepthCalculationService(windowSize: 1),
                (1, 2) => new DepthCalculationService(windowSize: 3),
                (25, 1) => null, //placeholder
                _ => null
            };
        }
    }
}
