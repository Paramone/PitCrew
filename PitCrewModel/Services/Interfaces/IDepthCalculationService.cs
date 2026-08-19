namespace PitCrewModel.Services.Interfaces
{
    public interface IDepthCalculationService
    {
        int CountDepthIncreases(List<int> numbers, int maxListAmount = 200);
    }
}