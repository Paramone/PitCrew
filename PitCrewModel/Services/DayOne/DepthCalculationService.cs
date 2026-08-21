using PitCrewModel.Services.Interfaces;

namespace PitCrewModel.Services.DayOne
{
    public class DepthCalculationService : IDepthCalculationService, IChallengeService
    {
        public DepthCalculationService(int windowSize = 1)
        {
            _windowSize = windowSize;
        }

        private readonly int _windowSize;

        public int Solve(List<int> numbers) => CountDepthIncreases(numbers);

        /// <summary>
        /// Counts the number of times an integer is higher than the previous integer in the list.
        /// </summary>
        /// <param name="depths"></param>
        /// <param name="chunkSize">How many numbers to process per parallel worker.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public int CountDepthIncreases(List<int> depths, int chunkSize = 200)
        {
            if (!ValidateInput(depths))
            {
                // TODO: hardcoded exception to global resource file, consistent exceptions messages.
                throw new ArgumentException("Invalid input provided");
            }

            if (chunkSize <= 1)
            {
                throw new ArgumentException("Chunksize may not be lower than 2.");
            }

            // Split the lists into chunks.
            List<int> windowedDepths = GroupIntoWindows(depths);
            List<List<int>> chunks = SplitIntoChunks(windowedDepths, chunkSize);

            // The amounth of depth increases
            int totalIncreases = 0;

            // Max degree of parallelism is set to the amount of chunks we have, so that we can process all chunks in parallel.
            // Was thinking of setting a max for this in appsettings, but who knows. Maybe I'll do that later.
            ParallelOptions options = new() { MaxDegreeOfParallelism = Math.Max(1, chunks.Count) };

            Parallel.ForEach(chunks, options, chunk =>
            {
                // Keep the count locally untill you're done, otherwise all threads would hit depthIncrease at the same time, losing the point of paralellism.

                int localIncreases = 0;
                int? previousDepth = null;

                foreach (int depth in chunk)
                {
                    if (previousDepth.HasValue && depth > previousDepth.Value)
                        localIncreases++;

                    previousDepth = depth;
                }

                Interlocked.Add(ref totalIncreases, localIncreases);
            });

            return totalIncreases;
        }

        private List<List<int>> SplitIntoChunks(List<int> depths, int chunkSize)
        {
            List<List<int>> chunks = [];

            List<int> carryOver = [];
            // Take -1 from list, as we'll be adding one from the previous list. 
            // This will however make it so that the first list will only have maxListAmount -1.
            foreach (var rawChunk in depths.Chunk(chunkSize - 1))
            {
                List<int> chunkWithCarryOver = rawChunk.ToList();

                if (carryOver.Any())
                {
                    // Add the last number from the previous list to the front of the current chunk
                    chunkWithCarryOver.InsertRange(0, carryOver);
                }

                chunks.Add(chunkWithCarryOver);

                // Carry over the last number so the next chunk can compare its first value
                carryOver = rawChunk.TakeLast(1).ToList();
            }

            return chunks;
        }

        /// <summary>
        /// Groups depths into windows of windowSize and returns the sum of each window.
        /// </summary>
        private List<int> GroupIntoWindows(List<int> depths)
        {
            if (_windowSize == 1)
                return depths;

            List<int> windowSums = [];

            // Slide a window across the list and sum each one.
            for (int i = 0; i <= depths.Count - _windowSize; i++)
            {
                windowSums.Add(depths.GetRange(i, _windowSize).Sum());
            }

            return windowSums;
        }

        /// <summary>
        /// Validates whether given input is correct. It checks if there are at least two numbers in the list and that the list is not null.
        /// </summary>
        /// <return> Bool if valid</return>
        private bool ValidateInput(List<int> depths)
            => depths != null && depths.Count > 1;
    }
}
