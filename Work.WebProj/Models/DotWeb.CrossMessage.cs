using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using ProcCore;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(DotWeb.AppStart.Startup))]
namespace DotWeb.CrossMessage
{
    [HubName("crossHandle")]
    public class crossHub : Hub
    {
        //private static Dictionary<String, String> _connectedClients = new Dictionary<String, String>();

        public override Task OnConnected()
        {
            //_connectedClients.Add(Context.ConnectionId);
            return base.OnConnected();
        }

        //public override Task OnDisconnected()
        //{
        //    //_connectedClients.Remove(Context.ConnectionId);
        //    return base.OnDisconnected();
        //}


        public override Task OnDisconnected(bool stopCalled)
        {
            //_connectedClients.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public void registType(String type)
        {
            Groups.Add(Context.ConnectionId, type);
        }


        public void notifyApplyNew()
        {
            Clients.Group("A").ReloadData();
            Log.Write("notifyApplyNew", Context.ConnectionId);
        }

    }
}