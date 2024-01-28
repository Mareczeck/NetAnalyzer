namespace NetAnalyzer.Infrastructure;

public class MacOSFileManipulationService : DefaultFileManipulationService
{
    /// <summary>
    /// MacOS usage - I dont want to share my home name
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string? ResolvePath(string? path)
    {
        if(path?.StartsWith('~') ?? false)
        {
            return path.Replace("~", Environment.GetEnvironmentVariable("HOME"));
        }

        return path;
    }
}