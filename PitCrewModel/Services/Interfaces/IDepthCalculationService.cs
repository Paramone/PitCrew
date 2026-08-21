namespace PitCrewModel.Services.Interfaces
{
    public interface IDepthCalculationService
    {
        int CountDepthIncreases(List<int> depths, int chunkSize = 200);
    }
}