using System;
using System.IO;
using System.Threading;
using WaterSort.Analytics;
using WaterSort.Generating;
using Color = byte;

const int depth = 4;
var generator = new Generator();
var analyzer = new Analyzer();
var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
using var sw = new StreamWriter($@"{desktop}\output.csv");

var i = 1;
for (var colorsCount = (Color)2; colorsCount <= 13; colorsCount++)
{
	var tubesCount = colorsCount + 2;
	for (var times = 0; times < 1000; times++)
	{
		var cts = new CancellationTokenSource();
		cts.CancelAfter(5000);
		var puzzle = generator.Generate(tubesCount, colorsCount, depth);
		var colorDistribution = puzzle.ColorCounting;
		var twoCount = colorDistribution.TryGetValue(2, out var r1) ? r1 : 0;
		var threeCount = colorDistribution.TryGetValue(3, out var r2) ? r2 : 0;
		var fourCount = colorDistribution.TryGetValue(4, out var r3) ? r3 : 0;
		var (twoSequentialCount, threeSequentialCount, fourSequentialCount) = (0, 0, 0);
		foreach (var tube in puzzle)
		{
			foreach (var element in tube.SpanningDistribution.Values)
			{
				unsafe
				{
					var p = element switch
					{
						2 => &twoSequentialCount,
						3 => &threeSequentialCount,
						4 => &fourSequentialCount,
						_ => null
					};
					if (p != null)
					{
						(*p)++;
					}
				}
			}
		}
		sw.WriteLine($"{i},{puzzle:f},{colorsCount},{tubesCount},{twoCount},{threeCount},{fourCount},{twoSequentialCount},{threeSequentialCount},{fourSequentialCount}");
		i++;
	}
}
