using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeXnes.Core;
using WeeXnes.MVVM.View;
using DiscordRPC;

namespace WeeXnes.RPC
{
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
            client.OnReady += (sender, e) =>
            {
                /*
                Globals.logContent.Value = "[" + this.ProcessName + ".exe] -> Received Ready from user " + e.User.Username;
                Globals.logUpdateTrigger.Value = "mgjnoeimgje";
                */
                DiscordRpcView.logContent = "[" + this.ProcessName + ".exe] -> Received Ready from user " + e.User.Username;
                DiscordRpcView.triggerLogupdate.Value = "nlejgmolegjog";
            };
            client.OnPresenceUpdate += (sender, e) =>
            {
                /*
                Globals.logContent.Value = "[" + this.ProcessName + ".exe] ->Received Update!";
                Globals.logUpdateTrigger.Value = "mgjnoeimgje";
                */
                DiscordRpcView.logContent = "[" + this.ProcessName + ".exe] -> Received Update on RPC";
                DiscordRpcView.triggerLogupdate.Value = "nlejgmolegjog";

            };
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
            client.UpdateStartTime();
        }
        public void stop()
        {
            if (this.client.IsInitialized)
            {
                client.ClearPresence();
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
                            //Do when Process is launched
                            //message.running(this.ProcessName);
                            start();

                            string output = this.Name + " [" + this.ProcessName + ".exe] started";
                            /*
                            Globals.logContent.Value = output;
                            Globals.logUpdateTrigger.Value = "mjfgoklemkgoeg";
                            */
                            DiscordRpcView.logContent = output;
                            DiscordRpcView.triggerLogupdate.Value = "nlejgmolegjog";
                            this.isRunning = true;
                        }
                    }
                    if (this.isRunning)
                    {
                        if (!foundProcess)
                        {
                            //Do when Process is closed
                            //message.closed(this.ProcessName);
                            stop();
                            string output = this.Name + " [" + this.ProcessName + ".exe] closed";
                            /*
                            Globals.logContent.Value = output;
                            Globals.logUpdateTrigger.Value = "mjfgoklemkgoeg";
                            */
                            DiscordRpcView.logContent = output;
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
