namespace D3NsCore.Models
{
    internal class HttpHeader
    {
        private static readonly HttpHeader[] EmptyArray = new HttpHeader[0];

        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }

        public HttpHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public HttpHeader()
        {
        }
    }
}