namespace OgurySdk
{
    public class OguryRewardItem
    {
        public OguryRewardItem(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }
    }
}