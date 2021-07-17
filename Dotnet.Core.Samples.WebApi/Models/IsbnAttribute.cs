using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Dotnet.Core.Samples.WebApi.Models
{
    /// <summary>
    /// Provides ISBN validation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class IsbnAttribute : ValidationAttribute
    {
        /// <summary>
        /// The following regular expression validates the format of an ISBN:
        /// https://www.safaribooksonline.com/library/view/regular-expressions-cookbook/9781449327453/ch04s13.html
        /// </summary>
        public override bool IsValid(object value)
        {
            var pattern = @"
                ^
                (?:ISBN(?:-1[03])?:?\ )?    # Optional ISBN/ISBN-10/ISBN-13 identifier.
                (?=                         # Basic format pre-checks (lookahead):
                [0-9X]{10}$                 # Require 10 digits/Xs (no separators).
                |                           # Or:
                (?=(?:[0-9]+[-\ ]){3})      # Require 3 separators
                [-\ 0-9X]{13}$              # out of 13 characters total.
                |                           # Or:
                97[89][0-9]{10}$            # 978/979 plus 10 digits (13 total).
                |                           # Or:
                (?=(?:[0-9]+[-\ ]){4})      # Require 4 separators
                [-\ 0-9]{17}$               # out of 17 characters total.
                )                           # End format pre-checks.
                (?:97[89][-\ ]?)?           # Optional ISBN-13 prefix.
                [0-9]{1,5}[-\ ]?            # 1-5 digit group identifier.
                [0-9]+[-\ ]?[0-9]+[-\ ]?    # Publisher and title identifiers.
                [0-9X]                      # Check digit.
                $
                ";

            var regex = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);

            return regex.IsMatch(value as string);
        }
    }
}