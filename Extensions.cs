namespace DevBuddy;

public static class Extensions 
{
    public static TClone Clone<TClone>(this TClone clone) where TClone : class 
    {
        return System.Text.Json.JsonSerializer.Deserialize<TClone>(System.Text.Json.JsonSerializer.Serialize(clone)) ?? throw new ArgumentException("Can't clone.");
    }
}