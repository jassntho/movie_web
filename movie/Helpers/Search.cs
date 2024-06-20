using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using FuzzySharp;


namespace Movie_Web.Helpers
{
    public class Search
    {
        public static string NormalizeText(string text)
        {
            text = RemoveDiacritics(text);
            text = text.ToLower();
            text = Regex.Replace(text, @"\s+", " ").Trim();
            text = Regex.Replace(text, @"[^\w\s]", "");
            return text;
        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static List<string> FindSimilarProducts(string searchText, List<string> products, int thresholdScore)
        {
            string normalizedSearchText = NormalizeText(searchText);
            List<(string product, int score)> scoredProducts = new List<(string product, int score)>();

            foreach (var product in products)
            {
                string normalizedProduct = NormalizeText(product);
                int tokenSetRatio = Fuzz.TokenSetRatio(normalizedSearchText, normalizedProduct);
                int partialRatio = Fuzz.PartialRatio(normalizedSearchText, normalizedProduct);
                int weightedScore = (tokenSetRatio + partialRatio) / 2;

                if (weightedScore >= thresholdScore)
                {
                    scoredProducts.Add((product, weightedScore));
                }
            }

            var sortedProducts = scoredProducts.OrderByDescending(x => x.score).Select(x => x.product).ToList();
            return sortedProducts;
        }

      

    }
}
