using System;
using System.Collections.Generic;
using System.Linq;

namespace TheatricalPlayersRefactoringKata;

public record StatementData(string Customer, List<PerformanceData> Performances)
{
    public int TotalAmount { get; } = Performances.Sum(perf => perf.Amoumt);
    public int TotalVolumenCredits { get; } = Performances.Sum(perf => perf.VolumeCredits);

    public static StatementData For(Invoice invoice, Dictionary<string, Play> plays)
    {
        var performances = invoice.Performances.Select(performance =>
        {
            var calculator = PerformanceCalculator.Create(performance, plays[performance.PlayID]);
            return new PerformanceData(performance.PlayID, performance.Audience) with
            {
                Play = calculator.Play,
                Amoumt = calculator.AmountFor(),
                VolumeCredits = calculator.VolumeCredits()
            };
        }).ToList();
        return new StatementData(invoice.Customer, performances);
    }
}

public record PerformanceData: Performance
{
    public PerformanceData(string playID, int audience) : base(playID, audience)
    {
    }

    public Play Play { get; set; }
    public int Amoumt { get; set; }
    public int VolumeCredits { get; set; }
}

internal class PerformanceCalculator(Performance performance, Play play)
{
    public static PerformanceCalculator Create(Performance performance, Play play)
    {
        switch (play.Type)
        {
            case "tragedy": return new TragedyCalculator(performance, play);
            case "comedy": return new ComedyCalculator(performance, play);
            default:
                return new PerformanceCalculator(performance, play);
        }
    }

    public Play Play { get; } = play;

    public virtual int AmountFor()
    {
        throw new("unknown type: " + Play.Type);
    }

    public virtual int VolumeCredits() => Math.Max(performance.Audience - 30, 0);
}

internal class ComedyCalculator(Performance performance, Play play) : PerformanceCalculator(performance, play)
{
    private readonly Performance _performance = performance;

    public override int AmountFor()
    {
       var result = 30000;
        if (_performance.Audience > 20)
        {
            result += 10000 + 500 * (_performance.Audience - 20);
        }

        result += 300 * _performance.Audience;
        return result;
    }

    public override int VolumeCredits()
    {
        return base.VolumeCredits() + (int)Math.Floor((decimal)_performance.Audience / 5);
    }
}

internal class TragedyCalculator(Performance performance, Play play) : PerformanceCalculator(performance, play)
{
    private readonly Performance _performance = performance;

    public override int AmountFor()
    {
        var result = 40000;
        if (_performance.Audience > 30)
        {
            result += 1000 * (_performance.Audience - 30);
        }

        return result;
    }
}