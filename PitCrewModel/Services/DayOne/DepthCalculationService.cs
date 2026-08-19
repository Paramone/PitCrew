using PitCrewModel.Services.Interfaces;

namespace PitCrewModel.Services.DayOne
{
    public class DepthCalculationService : IDepthCalculationService
    {
        public DepthCalculationService() { }

        /// <summary>
        /// Counts the number of times an integer is higher than the previous integer in the list.
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="maxListAmount">Define the max amount per list you'd like.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public int CountDepthIncreases(List<int> numbers, int maxListAmount = 200)
        {
            if (!ValidateInput(numbers))
            {
                // TODO: hardcoded exception to global resource file, consistent exceptions messages.
                throw new ArgumentException("Invalid input provided");
            }

            // Split the lists into chunks.
            List<List<int>> splitLists = SplitListIntoChunks(numbers, maxListAmount);

            // The amounth of depth increases
            int depthIncreases = 0;

            // Max degree of parallelism is set to the amount of lists we have, so that we can process all lists in parallel.
            // Was thinking of setting a max for this in appsettings, but who knows. Maybe I'll do that later.
            ParallelOptions options = new() { MaxDegreeOfParallelism = splitLists.Count }; 

            Parallel.ForEach(splitLists, options, chunk =>
            {
                // Keep the count locally untill you're done, otherwise all threads would hit depthIncrease at the same time, losing the point of paralellism.
                int localCount = 0;
                int? previousNumber = null;

                foreach (int currentDepth in chunk)
                {
                    if (previousNumber.HasValue && currentDepth > previousNumber.Value)
                        localCount++;

                    previousNumber = currentDepth;
                }

                Interlocked.Add(ref depthIncreases, localCount);
            });

            return depthIncreases;
        }

        private List<List<int>> SplitListIntoChunks(List<int> numbers, int maxListAmount)
        {
            // List of number lists
            List<List<int>> splitLists = [];

            // The last number from the previous list, used to compare with the first number of the next list
            int? lastNumberFromPreviousList = null;

            // Take -1 from list, as we'll be adding one from the previous list. 
            // This will however make it so that the first list will only have maxListAmount -1.
            foreach (var numberChunk in numbers.Chunk(maxListAmount - 1))
            {
                List<int> numberChunkList = numberChunk.ToList();
                if (lastNumberFromPreviousList.HasValue)
                {
                    // Add the last number from the previous list to the front of the current chunk
                    numberChunkList.Insert(0, lastNumberFromPreviousList.Value);
                }

                splitLists.Add(numberChunkList);

                // Sets the last number of the list, so we can add that to the front of the next list.
                lastNumberFromPreviousList = numberChunk.Last();
            }

            return splitLists;
        }

        /// <summary>
        /// Validates whether given input is correct. It checks if there are at least two numbers in the list and that the list is not null.
        /// </summary>
        /// <return> Bool if valid</return>
        private bool ValidateInput(List<int> numbers)
            => numbers != null && numbers.Count > 1;
    }
}

