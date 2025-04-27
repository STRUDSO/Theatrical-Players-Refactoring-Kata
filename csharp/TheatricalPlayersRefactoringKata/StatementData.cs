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
        var calculator = new PerformanceCalculator(performance, plays[performance.PlayID]);
        var result = performance with { Play = calculator.Play };
        return result with
        {
            Amoumt = AmoumtFor(result),
            VolumeCredits = VolumeCredits(result)
        };
    }

    private static int AmoumtFor(Performance performance)
    {
        var result = 0;
        switch (performance.Play.Type)
        {
            case "tragedy":
                result = 40000;
                if (performance.Audience > 30)
                {
                    result += 1000 * (performance.Audience - 30);
                }

                break;
            case "comedy":
                result = 30000;
                if (performance.Audience > 20)
                {
                    result += 10000 + 500 * (performance.Audience - 20);
                }

                result += 300 * performance.Audience;
                break;
            default:
                throw new Exception("unknown type: " + performance.Play.Type);
        }

        return result;
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

    private static int VolumeCredits(Performance perf)
    {
        int result = 0;
        result += Math.Max(perf.Audience - 30, 0);
        // add extra credit for every ten comedy attendees
        if ("comedy" == perf.Play.Type) result += (int)Math.Floor((decimal)perf.Audience / 5);
        return result;
    }
}

internal class PerformanceCalculator
{
    public PerformanceCalculator(Performance performance, Play play)
    {
        Play = play;
    }

    public Play Play { get; set; }
}