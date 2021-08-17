namespace ImperialPluginsConsole.Models
{
    public class CacheWaitItem
    {
        public int ID { get; init; }
        private WaitItemComplete Callback;
        public bool Complete { get; private set; } = false;

        public CacheWaitItem(int id, WaitItemComplete callback)
        {
            ID = id;
            Callback = callback;
        }

        public void SendComplete()
        {
            Complete = true;
            Callback(this);
        }
    }
}