namespace ClassLibrary1
{
    public class Barcode
    {
        private string _str { get; set; }
        private string code;
        private string barcode;
        private const int Height = 5;
        public static BarcodeType type;
        public Barcode(string str)
        {
            this._str = str;
            this.code = BarcodeHelper.getCode(_str);
            this.barcode = BarcodeHelper.getBarcode(code);
            Barcode.type = BarcodeType.Barcode;
        }
        public override string ToString()
        {
            string str = "";
            if (type != BarcodeType.Text) { str += this.barcode + "\n"; }
            if (type != BarcodeType.Barcode) { str += this._str.PadLeft((code.Length+4) / 4 + _str.Length / 2); }
            return str;

        }



        public string Str
        {
            get
            {
                return _str;
            }
            set
            {
                _str = value;
                this.code = BarcodeHelper.getCode(_str);
            }
        }



        /// <summary> 

        ///     Формать сборки штрих-кода 

        /// </summary> 

        public enum BarcodeType

        {

            /// <summary> 

            ///     Текстовая информация 

            /// </summary> 

            Text,

            /// <summary> 

            ///     Штрих-код 

            /// </summary> 

            Barcode,

            /// <summary> 

            ///     Полная информация 

            /// </summary> 

            Full

        }

    }
}