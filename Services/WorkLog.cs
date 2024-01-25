
using System.Globalization;
using System.Text.RegularExpressions;

namespace DevBuddy.Services;

public record WorkWeek(IEnumerable<WorkDay> Days, bool IsCommited = false);
public record WorkDay(DateTime Day, TimeSpan TargetWorkTime);
public record WorkLog(DateTime Day, WorkTime[] WorkTimes);
public record WorkTime(DateTime Day, TimeSpan? TimeSpan, string? TicketId, string? Description);


public class WorkLogParser
{
	public WorkWeek WorkWeek { get; }


	public WorkLogParser(WorkWeek workWeek)
	{
		WorkWeek = workWeek;
	}

	public WorkLogParser(DateTime anyDayOfIDOWeek, DayOfWeek weekBegin, DayOfWeek weekEnd, TimeSpan targetWorkTime)
	{
		var now = anyDayOfIDOWeek;
		var cal = CultureInfo.CurrentCulture.Calendar;
		int calendarWeekInt = ISOWeek.GetWeekOfYear(now);
		var beginOfWeek = ISOWeek.ToDateTime(now.Year, calendarWeekInt, DayOfWeek.Monday);
		WorkWeek = FromWeek(beginOfWeek, targetWorkTime, weekEnd);
	}

	public List<WorkLog> ParseWorkLogInput(string? value, List<WorkLog>? defaultValue = null)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return defaultValue ?? new List<WorkLog>();
		}
		var lines = value.Split(Environment.NewLine);
		var workLogs = lines.Select(x => ParseWorkLog(WorkWeek, x)).Where(x => x != null).ToList();
		return workLogs!;
	}
	
	private static WorkWeek FromWeek(DateTime beginDay, TimeSpan targetWorkTime, DayOfWeek weekEnd = DayOfWeek.Friday)
	{
		var list = new List<WorkDay>();
		var day = beginDay.Date;
		while (day.DayOfWeek < weekEnd + 1)
		{
			list.Add(new(day, targetWorkTime));
			day = day.AddDays(1);
		}

		return new(list);
	}
	private static WorkLog? ParseWorkLog(WorkWeek week, string worklog)
	{
		var tokens = worklog.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
		var first = tokens.FirstOrDefault();
		if (first == null) return null;
		var dayOfWeek = ParseDayOfWeek(first);
		if (dayOfWeek == null) return null;
		var weekDay = week.Days.FirstOrDefault(x => x.Day.DayOfWeek == dayOfWeek);
		if (weekDay == null) return null;

		tokens.RemoveAt(0);
		tokens = string.Join(' ', tokens).Split(',', StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

		return new WorkLog(weekDay.Day, tokens.Select(x => ParseWorkTime(x, weekDay.Day)).ToArray());
	}
	private static WorkTime ParseWorkTime(string input, DateTime day)
	{
		var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
		var workTime = TimeSpan.Zero;
		var removeTokens = new List<string>();
		tokens.ForEach(token =>
		{
			var parsedWorkTime = ParseTime(token);
			if (parsedWorkTime != TimeSpan.Zero) removeTokens.Add(token);
			workTime = workTime.Add(parsedWorkTime);
		});
		tokens.RemoveAll(x => removeTokens.Contains(x));

		return new WorkTime(day, workTime, null, string.Join(' ', tokens));
	}
	private static DayOfWeek? ParseDayOfWeek(string input)
	{
		var weekDays = new Dictionary<string, DayOfWeek>()
		{
			{@"^\s*?(Mo|Montag|Mon)", DayOfWeek.Monday },
			{@"^\s*?(Di|Dienstag|Die)", DayOfWeek.Tuesday },
			{@"^\s*?(Mi|Mittwoch|Mit)", DayOfWeek.Wednesday },
			{@"^\s*?(Do|Donnerstag|Don)", DayOfWeek.Thursday },
			{@"^\s*?(Fr|Freitag|Fre)", DayOfWeek.Friday },
			{@"^\s*?(Sa|Samstag|Sam)", DayOfWeek.Saturday },
			{@"^\s*?(So|Sonntag|Son)", DayOfWeek.Sunday },
		};

		DayOfWeek? dayOfWeek = null;
		foreach (var weekDay in weekDays)
		{
			if (Regex.IsMatch(input, weekDay.Key, RegexOptions.IgnoreCase))
			{
				dayOfWeek = weekDay.Value; break;
			}
		}
		return dayOfWeek;
	}
	private static TimeSpan ParseTime(string input)
	{
		var units = new Dictionary<string, int>()
		{
			{@"(\d+)(m|min|minuten|minute)", 1 },
			{@"(\d+)(h|hour|std|stunden|stunde)", 60 },
		};

		var timespan = new TimeSpan();

		foreach (var x in units)
		{
			var matches = Regex.Matches(input, x.Key);
			foreach (Match match in matches)
			{
				try
				{
					var amount = System.Convert.ToDouble(match.Groups[1].Value);
					timespan = timespan.Add(TimeSpan.FromMinutes(x.Value * amount));
				}
				catch { }
			}
		}

		return timespan;
	}
}