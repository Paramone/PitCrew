// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

// I hate that Visual Studio keeps reminding me that ".Count == 0" is faster than ".Any()". I find it more clean. Sue me. (don't)
[assembly: SuppressMessage("Performance", "CA1860:Avoid using 'Enumerable.Any()' extension method", Justification = "<Pending>", Scope = "member", Target = "~M:PitCrewModel.Services.DayOne.DepthCalculationService.ValidateInput(System.Collections.Generic.List{System.Int32})")]
