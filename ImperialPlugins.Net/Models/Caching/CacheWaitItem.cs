using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperialPlugins.Models.Caching
{
    public class CacheWaitItem
    {
        public int ID { get; }
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
