using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances) 
            {
                // add volume credits
                volumeCredits += VolumeCredits(plays, perf);

                // print line for this order
                var amounFor = AmounFor(perf, plays);
                result += String.Format(cultureInfo, "  {0}: {1} ({2} seats)\n", PlayFor(plays, perf).Name, format(amounFor), perf.Audience);
                totalAmount += AmounFor(perf, plays);
            }
            result += String.Format(cultureInfo, "Amount owed is {0}\n", format(totalAmount));
            result += String.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }

        private static string format(int amounFor)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");
            return Convert.ToDecimal(amounFor / 100).ToString("C", cultureInfo);
        }

        private static int VolumeCredits(Dictionary<string, Play> plays, Performance perf)
        {
            int result = 0;
            result += Math.Max(perf.Audience - 30, 0);
            // add extra credit for every ten comedy attendees
            if ("comedy" == PlayFor(plays, perf).Type) result += (int)Math.Floor((decimal)perf.Audience / 5);
            return result;
        }

        private static Play PlayFor(Dictionary<string, Play> plays, Performance perf)
        {
            var play = plays[perf.PlayID];
            return play;
        }

        private static int AmounFor(Performance perf, Dictionary<string, Play> plays)
        {
            var thisAmount = 0;
            switch (PlayFor(plays, perf).Type)
            {
                case "tragedy":
                    thisAmount = 40000;
                    if (perf.Audience > 30) {
                        thisAmount += 1000 * (perf.Audience - 30);
                    }
                    break;
                case "comedy":
                    thisAmount = 30000;
                    if (perf.Audience > 20) {
                        thisAmount += 10000 + 500 * (perf.Audience - 20);
                    }
                    thisAmount += 300 * perf.Audience;
                    break;
                default:
                    throw new Exception("unknown type: " + PlayFor(plays, perf).Type);
            }

            return thisAmount;
        }
    }
}
