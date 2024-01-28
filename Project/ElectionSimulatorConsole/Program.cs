using ElectionSimulatorLibrary;

int sum = 0;
for (int i = 1; i <= 41; i++)
{
    sum += ElectionSimulatorLibrary.BaseValues.GetSejmMandates(i);
}

Console.WriteLine(sum);

for (int i = 0; i <= 1588; i++)
{
    Console.WriteLine($"{i}: {BaseValues.TimeToDateTime(i)}");
}