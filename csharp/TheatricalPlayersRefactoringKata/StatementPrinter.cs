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
            return RenderPlainText(StatementData.For(invoice, plays));
        }

        public static string PrintHtml(Invoice invoice, Dictionary<string, Play> plays)
        {
            return RenderHtml(StatementData.For(invoice, plays));
        }

        private static string RenderHtml(StatementData data)
        {
            var result = $"<h1>Statement for {data.Customer}</h1>\n";
            result += "<table>\n";
            result += "<tr><th>play</th><th>seats</th><th>cost</th></tr>";
            foreach (var perf in data.Performances) {
                result += $" <tr><td>{perf.Play.Name}</td><td>{perf.Audience}</td>";
                result += $" <td>{Usd(perf.Amoumt)}</td></tr>\n";
            }
            result += "</table>\n";
            result += $"<p>Amount owed is <em>{Usd(data.TotalAmount)}</em></p>\n";
            result += $"<p>You earned <em>{data.TotalVolumenCredits}</em> credits</p>\n";
            return result;
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