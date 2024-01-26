using DevBuddy.Services;

namespace DevBuddy.Model;



public record WorkLogConfiguration(List<TargetWorkTime> TargetWorkTimeByDayOfWeek, List<TicketLink> TicketLinks)
{
    public static readonly WorkLogConfiguration DEFAULT_CONFIG = new(
        new List<TargetWorkTime>()
        {
            new(DayOfWeek.Monday, TimeSpan.FromHours(8)),
            new(DayOfWeek.Tuesday, TimeSpan.FromHours(8)),
            new(DayOfWeek.Wednesday, TimeSpan.FromHours(8)),
            new(DayOfWeek.Thursday, TimeSpan.FromHours(8)),
            new(DayOfWeek.Friday, TimeSpan.FromHours(8)),
        },
        new List<TicketLink>() 
        {
            new TicketLink(@"\s*?(Ticket-\d+)\s*?", "http://test+%TICKET%"),
        }
    );
};