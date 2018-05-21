
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;

namespace DifferentMethods.Extensions.Procedural
{
    /// <summary>
    /// The Markov generator is used to generate random strings (usually words or names) based on an input set of words.
    /// </summary>
    public class MarkovGenerator
    {
        private Dictionary<string, List<char>> _chains = new Dictionary<string, List<char>>();
        private List<string> _samples = new List<string>();
        private List<string> _used = new List<string>();
        private int _order;
        private int _minLength;
        private int suffix = 0;

        public MarkovGenerator(IEnumerable<string> samples, int minLength, int order = 4)
        {
            _order = order < 1 ? 1 : order;
            _minLength = minLength < 1 ? 1 : minLength;

            foreach (string word in samples)
            {
                string upper = word.Trim().ToUpper();
                if (upper.Length < order + 1)
                    continue;
                upper = upper.Replace("\t", " ");
                upper = upper.Replace("  ", " ");
                _samples.Add(upper);
            }

            foreach (string word in _samples)
            {
                for (int letter = 0; letter < word.Length - order; letter++)
                {
                    string token = word.Substring(letter, order);
                    List<char> entry = null;
                    if (_chains.ContainsKey(token))
                        entry = _chains[token];
                    else
                    {
                        entry = new List<char>();
                        _chains[token] = entry;
                    }
                    entry.Add(word[letter + order]);
                }
            }
        }

        /// <summary>
        /// Get the next generated string from the generator. MaxAttempts is the maximum number of tried to generate a unique string.
        /// </summary>
        /// <returns>The string.</returns>
        public string NextString(int maxAttempts = 30)
        {
            var N = _NextName();
            var attempt = 0;
            while (_samples.Contains(N.ToUpper()))
            {
                N = _NextName();
                if (attempt >= maxAttempts)
                {
                    return N;
                }
                attempt++;
            }
            return N;
        }

        string _NextName()
        {

            //get a random token somewhere in middle of sample word                
            string s = "";
            var count = 0;
            do
            {
                count++;
                if (count > 15)
                    suffix++;
                int n = (Rnd.Range(0, _samples.Count));
                int nameLength = UnityEngine.Mathf.Min(_samples[n].Length, 12);
                s = _samples[n].Substring((Rnd.Range(0, _samples[n].Length - _order)), _order);
                while (s.Length < nameLength)
                {
                    string token = s.Substring(s.Length - _order, _order);
                    char c = GetLetter(token);
                    if (c != '?')
                        s += GetLetter(token);
                    else
                        break;
                }

                if (s.Contains(" "))
                {
                    string[] tokens = s.Split(' ');
                    s = "";
                    for (int t = 0; t < tokens.Length; t++)
                    {
                        if (tokens[t] == "")
                            continue;
                        if (tokens[t].Length == 1)
                            tokens[t] = tokens[t].ToUpper();
                        else
                            tokens[t] = tokens[t].Substring(0, 1) + tokens[t].Substring(1).ToLower();
                        if (s != "")
                            s += " ";
                        s += tokens[t];
                    }
                }
                else
                    s = s.Substring(0, 1) + s.Substring(1).ToLower();
                if (suffix > 0)
                {
                    s += "-" + ToRoman(suffix + 1);
                }
            } while (_used.Contains(s) || s.Length < _minLength);
            _used.Add(s);
            return s;

        }

        static string ToRoman(int number)
        {
            if (-9999 >= number || number >= 9999)
            {
                throw new ArgumentOutOfRangeException("number");
            }

            if (number == 0)
            {
                return "NUL";
            }

            StringBuilder sb = new StringBuilder(10);

            if (number < 0)
            {
                sb.Append('-');
                number *= -1;
            }

            string[,] table = new string[,] {
                { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" },
                {
                    "",
                    "X",
                    "XX",
                    "XXX",
                    "XL",
                    "L",
                    "LX",
                    "LXX",
                    "LXXX",
                    "XC"
                },
                {
                    "",
                    "C",
                    "CC",
                    "CCC",
                    "CD",
                    "D",
                    "DC",
                    "DCC",
                    "DCCC",
                    "CM"
                },
                {
                    "",
                    "M",
                    "MM",
                    "MMM",
                    "M(V)",
                    "(V)",
                    "(V)M",
                    "(V)MM",
                    "(V)MMM",
                    "M(X)"
                }
            };

            for (int i = 1000, j = 3; i > 0; i /= 10, j--)
            {
                int digit = number / i;
                sb.Append(table[j, digit]);
                number -= digit * i;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Reset the generator, allowing previously generated strings to be generated again.
        /// </summary>
        public void Reset()
        {
            _used.Clear();
        }

        private char GetLetter(string token)
        {
            if (!_chains.ContainsKey(token))
                return '?';
            List<char> letters = _chains[token];
            int n = (Rnd.Range(0, letters.Count));
            return letters[n];
        }
    }
}
