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
            return RenderPlainText(StatementData.CreateStatementData(invoice, plays));
        }

        private static string RenderPlainText(StatementData statementData)
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

        private static string Usd(int amounFor)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US");
            return Convert.ToDecimal(amounFor / 100).ToString("C", cultureInfo);
        }
    }
}