namespace DevBuddy.Model;

public record WorkWeek(IEnumerable<WorkDay> Days, bool IsCommited = false);
