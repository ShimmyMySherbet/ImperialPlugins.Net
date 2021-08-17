using System.Collections.Generic;
using System.Threading;

namespace ImperialPluginsConsole.Models
{
    public class CacheWaitHandle
    {
        private bool m_Completed = false;
        private bool m_Active = false;

        public bool Completed => m_Active && m_Completed;

        private List<CacheWaitItem> m_WaitItems = new List<CacheWaitItem>();
        private int m_Step { get; set; } = 0;

        public CacheWaitItem CreateTask()
        {
            var item = new CacheWaitItem(m_Step, ItemCallback);
            m_Step++;

            lock (m_WaitItems)
                m_WaitItems.Add(item);
            return item;
        }

        public void Activate() => m_Active = true;

        private void ItemCallback(CacheWaitItem item)
        {
            lock (m_WaitItems)
                if (m_WaitItems.Contains(item))
                    m_WaitItems.Remove(item);

            if (m_WaitItems.Count == 0)
            {
                m_Completed = true;
            }
        }

        public void Wait()
        {
            SpinWait.SpinUntil(() => Completed);
        }
    }
}