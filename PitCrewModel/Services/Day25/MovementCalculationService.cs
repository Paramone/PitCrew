using PitCrewModel.Services.Interfaces;

namespace PitCrewModel.Services.Day25
{
    public class MovementCalculationService : IMovementCalculationService, IChallengeService<string[]>
    {
        public int Solve(string[] movementInput) => CountMovementsUntillDeadlock(movementInput);

        public int CountMovementsUntillDeadlock(string[] movementInput)
        {
            if (!ValidateInput(movementInput))
            {
                throw new ArgumentException("Invalid input provided");
            }

            int totalAmountOfRows = movementInput.Length;
            int totalAmountOfColumns = movementInput[0].Length;

            // TODO: Do a lot 

        }

        private bool ValidateInput(string[] movementInput)
            => movementInput != null && movementInput.Length > 1;
    }
}
