using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceCore.Utils;
using System.Threading;

namespace ClassService
{
    public class ClientUpdater
    {
        public String ClientIP { get; set; }

        public long LastUpdate { get; set; }

        private volatile bool lastCallResponse;
        public Boolean LastCallResponse { get { return lastCallResponse; } set { lastCallResponse = value; } }

        private RESTJsonClient restClient;
        private Clock clock;

        public ClientUpdater(RESTJsonClient client)
        {
            this.restClient = client;
            this.clock = new Clock();
        }

        private void UpdateClientCall()
        {
            try
            {
                restClient.Put(new Uri(ClientIP + "/update"), "{}", "application/json");
                LastCallResponse = true;
            }
            catch(Exception e)
            {
                LastCallResponse = false;
            }
        }

        public void UpdateClient()
        {
            if (LastUpdate < clock.CurrentTimeInMillis() - 1000)
            {
                Thread aNewThread = new System.Threading.Thread(new System.Threading.ThreadStart(UpdateClientCall));
                aNewThread.Start();
            }
        }
    }
}