namespace CraigBot.Bot.Common
{
    public class Fraction
    {
        public int Numerator { get; set; }
        
        public int Denominator { get; set; }

        private Fraction(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }
        
        public override string ToString()
        {
            return Numerator + "/" + Denominator;
        }

        public static bool TryParse(string input, out Fraction fraction)
        {
            fraction = null;
            
            var numbers = input.Split(DelimiterChars);

            if (numbers.Length != 2)
            {
                return false;
            }

            if (!int.TryParse(numbers[0], out var numerator) || !int.TryParse(numbers[1], out var denominator))
            {
                return false;
            }
            
            fraction = new Fraction(numerator, denominator);

            return true;
        }
        
        private static readonly char[] DelimiterChars = { '/', '\\', '|', ':', '.' };
    }
}