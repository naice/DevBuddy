namespace DevBuddy.Model;

public record WorkLog(DateTime Day, WorkTime[] WorkTimes);
public record WorkTime(DateTime Day, TimeSpan? TimeSpan, string? TicketId, string? Description);