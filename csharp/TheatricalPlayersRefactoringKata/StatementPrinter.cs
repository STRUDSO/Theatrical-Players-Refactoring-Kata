using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var statementData = new StatementData(invoice.Customer, invoice.Performances.Select(performance =>
            {
                var play = plays[performance.PlayID];
                return EnrichPerformances(performance, play);
            }).ToList());
            return renderPlainText(statementData, plays);
        }

        private Performance EnrichPerformances(Performance arg, Play playFor)
        {
            return arg with { Play = playFor };
        }

        private static string renderPlainText(StatementData statementData, Dictionary<string, Play> plays)
        {
            var result = $"Statement for {statementData.Customer}\n";

            foreach (var perf in statementData.Performances)
            {
                // print line for this order
                var amounFor = AmounFor(perf, plays);
                result += $"  {PlayFor(perf).Name}: {Usd(amounFor)} ({perf.Audience} seats)\n";
            }

            result += $"Amount owed is {Usd(TotalAmount(plays, statementData.Performances))}\n";
            result += $"You earned {TotalVolumeCredits(plays, statementData.Performances)} credits\n";
            return result;
        }

        private static int TotalAmount(Dictionary<string, Play> plays, List<Performance> invoicePerformances)
        {
            var result = 0;
            foreach (var perf in invoicePerformances) {
                result += AmounFor(perf, plays);
            }

            return result;
        }

        private static int TotalVolumeCredits(Dictionary<string, Play> plays, List<Performance> invoicePerformances)
        {
            var result = 0;
            foreach (var perf in invoicePerformances)
            {
                // add volume credits
                result += VolumeCredits(plays, perf);
            }

            return result;
        }

        private static string Usd(int amounFor)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");
            return Convert.ToDecimal(amounFor / 100).ToString("C", cultureInfo);
        }

        private static int VolumeCredits(Dictionary<string, Play> plays, Performance perf)
        {
            int result = 0;
            result += Math.Max(perf.Audience - 30, 0);
            // add extra credit for every ten comedy attendees
            if ("comedy" == PlayFor(perf).Type) result += (int)Math.Floor((decimal)perf.Audience / 5);
            return result;
        }

        private static Play PlayFor(Performance perf)
        {
            return perf.Play;
        }

        private static int AmounFor(Performance perf, Dictionary<string, Play> plays)
        {
            var thisAmount = 0;
            switch (PlayFor(perf).Type)
            {
                case "tragedy":
                    thisAmount = 40000;
                    if (perf.Audience > 30)
                    {
                        thisAmount += 1000 * (perf.Audience - 30);
                    }

                    break;
                case "comedy":
                    thisAmount = 30000;
                    if (perf.Audience > 20)
                    {
                        thisAmount += 10000 + 500 * (perf.Audience - 20);
                    }

                    thisAmount += 300 * perf.Audience;
                    break;
                default:
                    throw new Exception("unknown type: " + PlayFor(perf).Type);
            }

            return thisAmount;
        }
    }

    public record StatementData(string Customer, List<Performance> Performances);
}