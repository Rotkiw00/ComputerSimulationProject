using System.IO.Compression;

namespace ElectionSimulatorLibrary;

public class BaseValues
{
    public static bool LoadDataFromFile(string mapFilePath)
    {
        try
        {
            if (Directory.Exists("MapData")) Directory.Delete("MapData", true);
            File.Copy(mapFilePath, "tmp.zip", true);
            string extractPath = "MapData";
            ZipFile.ExtractToDirectory("tmp.zip", extractPath);
            File.Delete("tmp.zip");

            bool dataExists = false;

            if (Directory.Exists("MapData"))
            {
                dataExists = true;
                for (int i = 1; i <= 100; i++)
                {
                    if (!File.Exists($"MapData/{i}.json"))
                        dataExists = false;
                }
            }

            return dataExists;
        }
        catch
        {
            return false;
        }
    }
}
