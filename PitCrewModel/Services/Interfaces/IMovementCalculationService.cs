using System;
using System.Collections.Generic;
using System.Text;

namespace PitCrewModel.Services.Interfaces
{
    public interface IMovementCalculationService
    {
        int CountMovementsUntillDeadlock(string[] movementInput);
    }
}
