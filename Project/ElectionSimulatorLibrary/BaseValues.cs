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

    public static int GetSejmMandates(int districtId)
    {
        return districtId switch
        {
            1 => 12,
            2 => 8,
            3 => 14,
            4 => 12,
            5 => 13,
            6 => 15,
            7 => 12,
            8 => 12,
            9 => 10,
            10 => 9,
            11 => 12,
            12 => 8,
            13 => 14,
            14 => 10,
            15 => 9,
            16 => 10,
            17 => 9,
            18 => 12,
            19 => 20,
            20 => 12,
            21 => 12,
            22 => 11,
            23 => 15,
            24 => 14,
            25 => 12,
            26 => 14,
            27 => 9,
            28 => 7,
            29 => 9,
            30 => 9,
            31 => 12,
            32 => 9,
            33 => 16,
            34 => 8,
            35 => 10,
            36 => 12,
            37 => 9,
            38 => 9,
            39 => 10,
            40 => 8,
            41 => 12,
            _ => 0,
        };
    }

    public static int TimeToHour(int time)
    {
        if (time < 4)
        {
            switch(time)
            {
                case 0: return 20;
                case 1: return 21;
                case 2: return 22;
                case 3: return 23;
            }
        }

        return (time - 1) % 24;
    }
}
