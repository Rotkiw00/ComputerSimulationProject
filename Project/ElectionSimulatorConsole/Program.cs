int sum = 0;
for (int i = 1; i <= 41; i++)
{
    sum += ElectionSimulatorLibrary.BaseValues.GetSejmMandates(i);
}

Console.WriteLine(sum);