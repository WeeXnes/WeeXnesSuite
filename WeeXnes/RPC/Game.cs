using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeXnes.Core;
using WeeXnes.MVVM.View;
using DiscordRPC;
using DiscordRPC.Message;
using EventType = WeeXnes.Core.EventType;

namespace WeeXnes.RPC
{
    class EventCache
    {
        public static string cache1 = "";
    }
    public class Game
    {
        public DiscordRpcClient client { get; set; }
        public string id { get; set; }
        public string state { get; set; }
        public string details { get; set; }
        public string ProcessName { get; set; }
        public string Name { get; set; }
        public string bigimgkey { get; set; }
        public string smallimgkey { get; set; }
        public string bigimgtext { get; set; }
        public string smallimgtext { get; set; }
        public bool isRunning { get; set; }
        public string fileName { get; set; }
        public Game(string _fileName, string _name, string _pname = null, string _id = null, string _state = null, string _details = null, string _bigimgkey = null, string _smallimgkey = null, string _bigimgtext = null, string _smallimgtext = null)
        {
            this.fileName = _fileName;
            this.id = _id;
            this.state = _state;
            this.details = _details;
            this.Name = _name;
            this.ProcessName = _pname;
            this.bigimgkey = _bigimgkey;
            this.smallimgkey = _smallimgkey;
            this.bigimgtext = _bigimgtext;
            this.smallimgtext = _smallimgtext;
            this.client = new DiscordRpcClient(id);
        }
        public void start()
        {
            if (!client.IsInitialized)
            {
                client.Initialize();
            }
            client.OnReady += ClientOnOnReady;
            client.OnPresenceUpdate += ClientOnOnPresenceUpdate;
            client.SetPresence(new RichPresence()
            {
                Details = this.details,
                State = this.state,
                Assets = new Assets()
                {
                    LargeImageKey = this.bigimgkey,
                    LargeImageText = this.bigimgtext,
                    SmallImageKey = this.smallimgkey,
                    SmallImageText = this.smallimgtext
                }
            });
            if (Globals.settings_RpcShowElapsedTime.Value)
            {
                client.UpdateStartTime();
            }
        }

        private void ClientOnOnPresenceUpdate(object sender, PresenceMessage args)
        {
            DiscordRpcView.logContent = new customEvent("[" + this.ProcessName + ".exe] ➜ Received Update on " + args.Name, EventType.RPCUpdateEvent);
            DiscordRpcView.triggerLogupdate.Value = "nlejgmolegjog";
        }

        private void ClientOnOnReady(object sender, ReadyMessage args)
        {
            DiscordRpcView.logContent = new customEvent("[" + this.ProcessName + ".exe] ➜ Received Ready from user " + args.User.Username, EventType.RPCReadyEvent);
            DiscordRpcView.triggerLogupdate.Value = "nlejgmolegjog";
        }

        public void stop()
        {
            if (this.client.IsInitialized)
            {
                client.ClearPresence();
                client.OnReady -= ClientOnOnReady;
                client.OnPresenceUpdate -= ClientOnOnPresenceUpdate;
            }
        }
        public void checkState(Process[] processes)
        {
            if(!String.IsNullOrEmpty(this.ProcessName))
            {
                if (!String.IsNullOrEmpty(this.id))
                {
                    bool foundProcess = false;
                    foreach (Process process in processes)
                    {
                        if (process.ProcessName == this.ProcessName)
                        {
                            foundProcess = true;
                        }
                    }

                    if (!this.isRunning)
                    {
                        if (foundProcess)
                        {
                            start();
                            DiscordRpcView.logContent = new customEvent("↪ " + this.Name + " [" + this.ProcessName + ".exe] started", EventType.ProcessStartedEvent);
                            DiscordRpcView.triggerLogupdate.Value = "nlejgmolegjog";
                            this.isRunning = true;
                        }
                    }
                    if (this.isRunning)
                    {
                        if (!foundProcess)
                        {
                            stop();
                            DiscordRpcView.logContent = new customEvent( "↩ " + this.Name + " [" + this.ProcessName + ".exe] closed", EventType.ProcessStoppedEvent);
                            DiscordRpcView.triggerLogupdate.Value = "nlejgmolegjog";

                            this.isRunning = false;
                        }
                    }
                }
            }



        }
        public override string ToString()
        {
            return this.Name;
        }
    }
}
