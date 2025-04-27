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
            var statementData = new StatementData(invoice.Customer, invoice.Performances.Select(performance => Enrich(plays, performance)).ToList());

            statementData.TotalAmount = TotalAmount(statementData.Performances);
            statementData.TotalVolumenCredits = TotalVolumeCredits((statementData.Performances));
            return renderPlainText(statementData);
        }

        private Performance Enrich(Dictionary<string, Play> plays, Performance performance)
        {
            var result = performance with { Play = plays[performance.PlayID] };
            return result with
            {
                Amoumt = AmoumtFor(result),
                VolumeCredits = VolumeCredits(result)
            };
        }

        private int AmoumtFor(Performance performance)
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

        private static string renderPlainText(StatementData statementData)
        {
            var result = $"Statement for {statementData.Customer}\n";

            foreach (var perf in statementData.Performances)
            {
                // print line for this order
                var amounFor = perf.Amoumt;
                result += $"  {perf.Play.Name}: {Usd(amounFor)} ({perf.Audience} seats)\n";
            }

            result += $"Amount owed is {Usd(statementData.TotalAmount)}\n";
            result += $"You earned {statementData.TotalVolumenCredits} credits\n";
            return result;
        }

        private static int TotalAmount(List<Performance> invoicePerformances)
        {
            var result = 0;
            foreach (var perf in invoicePerformances) {
                result += perf.Amoumt;
            }

            return result;
        }

        private static int TotalVolumeCredits(List<Performance> invoicePerformances)
        {
            var result = 0;
            foreach (var perf in invoicePerformances)
            {
                // add volume credits
                result += perf.VolumeCredits;
            }

            return result;
        }

        private static string Usd(int amounFor)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");
            return Convert.ToDecimal(amounFor / 100).ToString("C", cultureInfo);
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

    public record StatementData(string Customer, List<Performance> Performances)
    {
        public int TotalAmount { get; set; }
        public int TotalVolumenCredits { get; set; }
    }
}