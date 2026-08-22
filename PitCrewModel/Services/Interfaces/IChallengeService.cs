namespace PitCrewModel.Services.Interfaces
{
    public interface IChallengeService { }

    public interface IChallengeService<T> : IChallengeService
    {
        int Solve(T input);
    }
}
