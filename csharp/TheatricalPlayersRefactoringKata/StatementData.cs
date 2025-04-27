using System;
using System.Collections.Generic;
using System.Linq;

namespace TheatricalPlayersRefactoringKata;

public record StatementData(string Customer, List<Performance> Performances)
{
    public int TotalAmount { get; set; }
    public int TotalVolumenCredits { get; set; }

    public static StatementData For(Invoice invoice, Dictionary<string, Play> plays)
    {
        var statementData = new StatementData(invoice.Customer, invoice.Performances.Select(performance => Enrich(plays, performance)).ToList());

        statementData.TotalAmount = TotalAmountFor(statementData.Performances);
        statementData.TotalVolumenCredits = TotalVolumeCreditsFor((statementData.Performances));
        return statementData;
    }

    private static Performance Enrich(Dictionary<string, Play> plays, Performance performance)
    {
        var calculator = PerformanceCalculator.Create(performance, plays[performance.PlayID]);
        return performance with
        {
            Play = calculator.Play,
            Amoumt = calculator.AmountFor(),
            VolumeCredits = calculator.VolumeCredits()
        };
    }

    private static int TotalAmountFor(List<Performance> invoicePerformances)
    {
        var result = 0;
        foreach (var perf in invoicePerformances) {
            result += perf.Amoumt;
        }

        return result;
    }

    public static int TotalVolumeCreditsFor(List<Performance> invoicePerformances)
    {
        var result = 0;
        foreach (var perf in invoicePerformances)
        {
            // add volume credits
            result += perf.VolumeCredits;
        }

        return result;
    }
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

    public Play Play { get; set; } = play;

    public virtual int AmountFor()
    {
        var result = 0;
        var performancePlay = Play;
        switch (performancePlay.Type)
        {
            case "tragedy":

                throw new NotImplementedException();
            case "comedy":
                result = 30000;
                if (performance.Audience > 20)
                {
                    result += 10000 + 500 * (performance.Audience - 20);
                }

                result += 300 * performance.Audience;
                break;
            default:
                throw new Exception("unknown type: " + performancePlay.Type);
        }

        return result;
    }

    public int VolumeCredits()
    {
        int result = 0;
        result += Math.Max(performance.Audience - 30, 0);
        // add extra credit for every ten comedy attendees
        if ("comedy" == Play.Type) result += (int)Math.Floor((decimal)performance.Audience / 5);
        return result;
    }
}

internal class ComedyCalculator(Performance performance, Play play) : PerformanceCalculator(performance, play)
{
    public override int AmountFor()
    {
       var result = 30000;
        if (performance.Audience > 20)
        {
            result += 10000 + 500 * (performance.Audience - 20);
        }

        result += 300 * performance.Audience;
        return result;
    }
}

internal class TragedyCalculator(Performance performance, Play play) : PerformanceCalculator(performance, play)
{
    public override int AmountFor()
    {
        var result = 40000;
        if (performance.Audience > 30)
        {
            result += 1000 * (performance.Audience - 30);
        }

        return result;
    }
}