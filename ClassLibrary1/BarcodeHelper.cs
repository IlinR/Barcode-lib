using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ClassLibrary1
{
    internal static class BarcodeHelper
    {
        public static string getCode(string str)
        {
            StringBuilder code = new StringBuilder();
            IList<int> price = new List<int>();
            int i = 0; bool encodeModeDigit = true;
            StartPatern(str,code,ref encodeModeDigit,price);    
            while (i<str.Length)
                encode(str, code, price, ref encodeModeDigit,ref i);
            EndPatern(code,price);
            return code.ToString();
        }
        private static void StartPatern(string str, StringBuilder code,ref bool encodeModeDigit,IList<int> price)
        {
            if (!isAllDigits(str, 0, 2))
            {
                encodeModeDigit = false;
                addPatern(code, price, StartText);
            }
            else addPatern(code, price, StartNumbers);
        }
        private static void EndPatern(StringBuilder code, IList<int> price)
        {
            addPatern(code, price, getCheckSum(price));
            addPatern(code, price, Stop);
        }
        private static void encode(string text, StringBuilder code, IList<int> pricelist, ref bool inDigitMode, ref int current)
        {
            if ((inDigitMode && !isAllDigits(text, current, 2)) || (!inDigitMode && isAllDigits(text, current, 4)))
            {
                inDigitMode = !inDigitMode;
                addPatern(code, pricelist, inDigitMode ? SwitchToNumbers : SwitchToText);
            }
            getPatern(ref current, pricelist, inDigitMode, text, code);
        }

        private static void getPatern(ref int current, IList<int> pricelist, bool isNumbers, string text, StringBuilder codeStr)
        {
            if (isNumbers)
            {
                addPatern(codeStr, pricelist, Array.IndexOf(NumberSymbols, text.Substring(current, 2)));
                current += 2;
            }
            else
            {
                addPatern(codeStr, pricelist, Array.IndexOf(TextSymbols, text.Substring(current, 1)));
                current++;
            }
        }

        private static void addPatern(StringBuilder codeStr, IList<int> pricelist, int symbol)
        {
            codeStr.Append(Patterns[symbol]);
            pricelist.Add(symbol);
        }

        private static bool isAllDigits(string str, int firstel, int length)
        {
            var chars = str.Skip(firstel).Take(length);
            return chars.Count() == length && chars.All(x => char.IsDigit(x));
        }

        private static int getCheckSum(IList<int> pricelist)
        {
            var sum = pricelist[0];
            for (var i = 1; i < pricelist.Count; i++)
            {
                sum += i * pricelist[i];
            }
            sum %= 103;
            return sum;
        }

        private static char getBar(string code) => Bars[Convert.ToInt32(code, 2)];
        private static IEnumerable<string> SplitStr(this string str, int length)
        {
            return Enumerable.Range(0, str.Length / length).Select(i => str.Substring(i * length, length));
        }


        public static string getBarcode(string code)
        {
            int l = code.Length;
            //var sb = string.Join("", code.PadLeft(l + 2, '0').PadRight(l + 2, '0').SplitStr(2).Select(getBar));
            var s = string.Join("", code.PadLeft(l + 2, '0').PadRight(l + 4, '0').SplitStr(2).Select(getBar));
            var rs = string.Join("\n",Enumerable.Repeat(s, Height));
            var ps = string.Join("",new string('0', l + 4).SplitStr(2).Select(getBar));

            return ps + "\n" + rs + "\n" + ps ;
        }
        private const int Height = 5;
        public static readonly char[] Bars = { '█', '▌', '▐', ' ' };

        private const int StartText = 104;
        private const int StartNumbers = 105;
        private const int SwitchToText = 100;


        private static readonly string[] Patterns = {

        "11011001100", "11001101100", "11001100110", "10010011000", "10010001100",

        "10001001100", "10011001000", "10011000100", "10001100100", "11001001000",

        "11001000100", "11000100100", "10110011100", "10011011100", "10011001110",

        "10111001100", "10011101100", "10011100110", "11001110010", "11001011100",

        "11001001110", "11011100100", "11001110100", "11101101110", "11101001100",

        "11100101100", "11100100110", "11101100100", "11100110100", "11100110010",

        "11011011000", "11011000110", "11000110110", "10100011000", "10001011000",

        "10001000110", "10110001000", "10001101000", "10001100010", "11010001000",

        "11000101000", "11000100010", "10110111000", "10110001110", "10001101110",

        "10111011000", "10111000110", "10001110110", "11101110110", "11010001110",

        "11000101110", "11011101000", "11011100010", "11011101110", "11101011000",

        "11101000110", "11100010110", "11101101000", "11101100010", "11100011010",

        "11101111010", "11001000010", "11110001010", "10100110000", "10100001100",

        "10010110000", "10010000110", "10000101100", "10000100110", "10110010000",

        "10110000100", "10011010000", "10011000010", "10000110100", "10000110010",

        "11000010010", "11001010000", "11110111010", "11000010100", "10001111010",

        "10100111100", "10010111100", "10010011110", "10111100100", "10011110100",

        "10011110010", "11110100100", "11110010100", "11110010010", "11011011110",

        "11011110110", "11110110110", "10101111000", "10100011110", "10001011110",

        "10111101000", "10111100010", "11110101000", "11110100010", "10111011110",

        "10111101110", "11101011110", "11110101110", "11010000100", "11010010000",

        "11010011100", "11000111010", "11010111000", "1100011101011"};

        private static readonly string[] TextSymbols = {

        " ","!","\"","#","$","%","&","'","(",")",

        "*","+",",","-",".","/","0","1","2","3",

        "4","5","6","7","8","9",":",";","<","=",

        ">","?","@","A","B","C","D","E","F","G",

        "H","I","J","K","L","M","N","O","P","Q",

        "R","S","T","U","V","W","X","Y","Z","[",

        "\\","]","^","_","`","a","b","c","d","e",

        "f","g","h","i","j","k","l","m","n","o",

        "p","q","r","s","t","u","v","w","x","y",

        "z","{","|","|","~"

    };
        private static readonly string[] NumberSymbols = {

        "00","01","02","03","04","05","06","07","08","09",

        "10","11","12","13","14","15","16","17","18","19",

        "20","21","22","23","24","25","26","27","28","29",

        "30","31","32","33","34","35","36","37","38","39",

        "40","41","42","43","44","45","46","47","48","49",

        "50","51","52","53","54","55","56","57","58","59",

        "60","61","62","63","64","65","66","67","68","69",

        "70","71","72","73","74","75","76","77","78","79",

        "80","81","82","83","84","85","86","87","88","89",

        "90","91","92","93","94","95","96","97","98","99"

    };
        private const int Stop = 108;
        private const int SwitchToNumbers = 99;
    }
}

