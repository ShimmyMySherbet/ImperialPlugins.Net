namespace ImperialPluginsConsole.Models
{
    public struct WeightInfoPair
    {
        public int Weight { get; init; }
        public CommandInfo Info { get; init; }

        public WeightInfoPair(int weight, CommandInfo info)
        {
            Weight = weight;
            Info = info;
        }
    }
}