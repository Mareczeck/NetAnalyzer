
namespace NetAnalyzer.Infrastructure;

public class NetworkDataLoaderService : INetworkDataLoaderService
{
    /// <summary>
    /// Method for reading dataset 
    /// </summary>
    /// <param name="stream">Input stream</param>
    /// <returns>Filtered dataset of relationships</returns>
    /// <exception cref="InvalidFormatException"></exception>
    public HashSet<(int n1, int n2)> ReadData(Stream stream)
    {
        // optimalization - estimate number of lines by size of the stream for quicker initialization of hashset
        //      - I assume average id to be 2 or 3 digits, and format is "{number 1}_{number_2}\n"
        // assumption file is no bigger than 2 billion lines (+file size limit)
        // var estimatedRows = (int) (stream.Length / ((2.5 * 2) + 2));
        
        var relationships = new HashSet<(int n1, int n2)>();

        using (StreamReader reader = new StreamReader(stream))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(' ');
                if (parts.Length == 2 && int.TryParse(parts[0], out int num1) && int.TryParse(parts[1], out int num2))
                {
                    // Sorting for easier contains check
                    int n1 = Math.Min(num1, num2);
                    int n2 = Math.Max(num1, num2);

                    if(relationships.Contains((n1,n2)))
                    {
                        // This relationship was already added
                        continue;
                    }
                    relationships.Add((n1, n2));
                }
                else
                {
                    throw new InvalidFormatException();
                }
            }
        }

        return relationships;
    }
}
