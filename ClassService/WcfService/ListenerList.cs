using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassService
{
    public class ListenerList
    {
        List<ClientUpdater> listeners;

        public ListenerList()
        {
            listeners = new List<ClientUpdater>();
        }

        public bool Add(ClientUpdater newClientListener)
        {
            if (!listeners.Any(l => l.ClientIP == newClientListener.ClientIP))
            {
                listeners.Add(newClientListener);
                return true;
            }
            return false;
        }

        public void WarnListenersExcept(String responsableIP)
        {
            foreach (ClientUpdater client in listeners)
            {
                if (client.ClientIP != responsableIP)
                    client.UpdateClient();
            }
        }

        public bool Remove(String ip)
        {
            if (listeners.Any(l => l.ClientIP == ip))
            {
                listeners.Remove(listeners.Where(l => l.ClientIP == ip).First());
                return true;
            }
            return false;
        }
    }
}
