using Newtonsoft.Json;

namespace ImperialPlugins.Models
{
    public class EnumerableResponse<T> : IPObject
    {
        public int TotalCount;
        public T[] Items;

        [JsonIgnore]
        private ImperialPluginsClient m_ImperialPlugins;

        [JsonIgnore]
        public ImperialPluginsClient ImperialPlugins
        {
            get => m_ImperialPlugins;
            set
            {
                m_ImperialPlugins = value;

                if (typeof(IPObject).IsAssignableFrom(typeof(T)))
                {
                    if (Items == null) return;
                    foreach (var item in Items)
                    {
                        if (item is IPObject ipo) ipo.ImperialPlugins = value;
                    }
                }
            }
        }
    }
}