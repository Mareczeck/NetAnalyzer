namespace NetAnalyzer.Infrastructure;
public interface IFileManipulationService 
{
    string? ResolvePath(string? path)
    {
        return path;

    }

    void SaveStreamToFile(string path, string fileName, Stream stream);
}
