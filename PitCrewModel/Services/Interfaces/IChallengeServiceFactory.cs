namespace PitCrewModel.Services.Interfaces
{
    public interface IChallengeServiceFactory
    {
        IChallengeService? GetChallenge(int day, int part);
    }
}
