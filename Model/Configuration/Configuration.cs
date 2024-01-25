namespace DevBuddy.Model;

public record Configuration(WorkLogConfiguration WorkLogConfiguration) 
{
    public static readonly Configuration DEFAULT_CONFIG = new(WorkLogConfiguration.DEFAULT_CONFIG);
};
