using ClassLibrary1;


internal class Program
{
    private static void Main(string[] args)
    {
        Barcode cod = new Barcode("123456789Example");
        Barcode.type = Barcode.BarcodeType.Full;
        Console.WriteLine(cod.ToString());
    }
}