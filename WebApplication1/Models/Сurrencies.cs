

namespace WebApplication1
{
    public class Сurrencies
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Currency Currency { get; set; }
    }

    public class Currency
    {
        public int Id { get; set; }
        public double Value { get; set; }
    }
}
