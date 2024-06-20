namespace Report
{
    public class PdfRequest
    {
        public string Name { get; set; }
        public string Std { get; set; }
        public FeesStructure fees { get; set; }
    }
//
    public class FeesStructure
    {
        public int id { get; set; }
        public string FeesDescription { get; set; }
        public string Amount { get; set; }
    }
}
