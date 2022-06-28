using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSGSI;
using CSGSI.Nodes;
using WeeXnes.Core;
using WeeXnes.MVVM.View;
using DiscordRPC;
using DiscordRPC.Message;
using EventType = WeeXnes.Core.EventType;

namespace WeeXnes.RPC.CSGO
{
    class EventCache
    {
        public static string cache1 = "";
    }
    
    public class CSGORPC
    {
        
        public DiscordRpcClient client { get; set; }
        public string ProcessName { get; set; }
        public bool isRunning { get; set; }
        public CSGORPC(string ClientID)
        {
            this.ProcessName = "csgo";
            this.client = new DiscordRpcClient(ClientID);
            Globals.gameStateListener.NewGameState += GameStateListenerOnNewGameState;
        }

        private void GameStateListenerOnNewGameState(GameState gs)
        {
            
            if (gs.Player.Activity == PlayerActivity.Menu)
            {
                this.client.UpdateLargeAsset("menu");
                this.client.UpdateDetails("In Menu");
                this.client.UpdateState("");
            }
            else if (gs.Player.Activity == PlayerActivity.Playing)
            {
                this.client.UpdateLargeAsset(gs.Map.Name);
                this.client.UpdateDetails("Playing on " + gs.Map.Name);
                if (gs.Player.Team == PlayerTeam.T)
                {
                    this.client.UpdateSmallAsset("t_logo");
                }else if(gs.Player.Team == PlayerTeam.CT)
                {
                    this.client.UpdateSmallAsset("ct_logo");
                }
                else
                {
                    this.client.UpdateSmallAsset("");
                }
            }
            
        }

        public void start()
        {
            
            Globals.gameStateListener.Start();
            if (!client.IsInitialized)
            {
                client.Initialize();
            }
            client.OnReady += ClientOnOnReady;
            client.OnPresenceUpdate += ClientOnOnPresenceUpdate;
            client.SetPresence(new RichPresence()
            {
                Details = "Playing CSGO",
                State = "Launching Game...",
                Assets = new Assets()
                {
                    LargeImageKey = "menu",
                    LargeImageText = "",
                    SmallImageKey = "",
                    SmallImageText = ""
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
            
            Globals.gameStateListener.Stop();
            
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

                        /*
                        Globals.logContent.Value = output;
                        Globals.logUpdateTrigger.Value = "mjfgoklemkgoeg";
                        */
                        DiscordRpcView.logContent = new customEvent("↪ " + "CSGO" + " [" + this.ProcessName + ".exe] started", EventType.ProcessStartedEvent);
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
                        /*
                        Globals.logContent.Value = output;
                        Globals.logUpdateTrigger.Value = "mjfgoklemkgoeg";
                        */
                        DiscordRpcView.logContent = new customEvent( "↩ " + "CSGO" + " [" + this.ProcessName + ".exe] closed", EventType.ProcessStoppedEvent);
                        DiscordRpcView.triggerLogupdate.Value = "nlejgmolegjog";

                        this.isRunning = false;
                    }
                }
            }



        }
        public override string ToString()
        {
            return "CSGO";
        }
    }
}
