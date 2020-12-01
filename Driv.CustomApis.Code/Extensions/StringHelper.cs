using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XrmVision.Extensions.Extensions
{
    public static class StringHelper
    {

        /// <summary>
        /// REmove accents in a string : Héllô = Hello
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string text) 
            => string.IsNullOrEmpty(text) ? 
                    null : 
                    string.Concat(
                        text.Normalize(NormalizationForm.FormD)
                            .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                         UnicodeCategory.NonSpacingMark)
                    ).Normalize(NormalizationForm.FormC);



        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }

        public static string Right(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(value.Length - maxLength, maxLength)
                   );
        }

    }
}
