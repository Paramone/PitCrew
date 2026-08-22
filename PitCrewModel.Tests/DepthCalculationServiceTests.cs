using PitCrewModel.Services.Day1;

namespace PitCrewModel.Tests
{
    public class DepthCalculationServiceTests
    {
        // Day one input
        private readonly List<int> _inputDayOne = [199, 200, 208, 210, 200, 207, 240, 269, 260, 263];

        [Fact]
        public void CountDepthIncreases_Day1Part1()
        {
            var service = new DepthCalculationService(windowSize: 1);

            int result = service.CountDepthIncreases(_inputDayOne);

            Assert.Equal(7, result);
        }

        [Fact]
        public void CountDepthIncreases_Day1Part2()
        {
            var service = new DepthCalculationService(windowSize: 3);

            int result = service.CountDepthIncreases(_inputDayOne);

            Assert.Equal(5, result);
        }

        [Fact]
        public void CountDepthIncreases_ArgException()
        {
            var service = new DepthCalculationService(windowSize: 1);

            Assert.Throws<ArgumentException>(() => service.CountDepthIncreases([]));
        }

        [Fact]
        public void CountDepthIncreases_ChunkSize1()
        {
            var service = new DepthCalculationService(windowSize: 1);

            Assert.Throws<ArgumentException>(() => service.CountDepthIncreases(_inputDayOne, 1));
        }

        [Fact]
        public void CountDepthIncreases_ChunkSize10000()
        {
            var service = new DepthCalculationService(windowSize: 1);

            int result = service.CountDepthIncreases(_inputDayOne, 10000);
            
            Assert.Equal(7, result);
        }
    }
}
