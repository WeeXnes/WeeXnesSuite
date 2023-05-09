using System;
using System.Diagnostics;
using System.Windows.Forms;
using DiscordRPC;
using DiscordRPC.Message;
using Nocksoft.IO.ConfigFiles;
using WeeXnes.Core;
using WeeXnes.Views.Settings;
using EventType = WeeXnes.Core.EventType;

namespace WeeXnes.Views.DiscordRPC
{
    public class Game
    {
        public static class Methods
        {
            public static Game GameFromIni(INIFile inifile)
            {
                Game returnval = new Game(
                    inifile.GetValue(SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                        SaveSettingsHandler.Data.DiscordRpcFiles.ProcessName),
                    
                    inifile.GetValue(SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                        SaveSettingsHandler.Data.DiscordRpcFiles.ClientId),
                    
                    inifile.GetValue(SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                        SaveSettingsHandler.Data.DiscordRpcFiles.Details),
                    
                    inifile.GetValue(SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                        SaveSettingsHandler.Data.DiscordRpcFiles.State),
                    
                    inifile.GetValue(SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                        SaveSettingsHandler.Data.DiscordRpcFiles.BigImageKey),
                    
                    inifile.GetValue(SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                        SaveSettingsHandler.Data.DiscordRpcFiles.BigImageText),
                    
                    inifile.GetValue(SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                        SaveSettingsHandler.Data.DiscordRpcFiles.SmallImageKey),
                    
                    inifile.GetValue(SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                        SaveSettingsHandler.Data.DiscordRpcFiles.SmallImageText)
                );
                returnval.UUID = inifile.GetValue(SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                    SaveSettingsHandler.Data.DiscordRpcFiles.UUID);
                return returnval;

            }
        }
        public string ProcessName { get; set; }
        public bool IsRunning { get; set; }
        public DiscordRpcClient PresenceClient { get; set; }
        public string Details { get; set; }
        public string State { get; set; }
        
        public string BigImageKey { get; set; }
        public string BigImageText { get; set; }
        public string SmallImageKey { get; set; }
        public string SmallImageText { get; set; }
        public string UUID { get; set; }
        private string generateUUID()
        {
            return Guid.NewGuid().ToString();
        }
        public Game(
            string processName,
            string clientId,
            string details,
            string state,
            string bigImageKey,
            string bigImageText,
            string smallImageKey,
            string smallImageText
            )
        {
            this.ProcessName = processName;
            this.IsRunning = false;
            this.PresenceClient = new DiscordRpcClient(clientId);
            this.Details = details;
            this.State = state;
            this.BigImageKey = bigImageKey;
            this.BigImageText = bigImageText;
            this.SmallImageKey = smallImageKey;
            this.SmallImageText = smallImageText;
            this.UUID = generateUUID();
        }

        public void Start()
        {
            this.IsRunning = true;
            //Console.WriteLine("Process started");
            RunRPCView.Data.LogCache.Value = new customEvent("[" + this.ProcessName + ".exe] ➜ is running", EventType.ProcessStartedEvent);
            
            if (!this.PresenceClient.IsInitialized)
            {
                this.PresenceClient.Initialize();
            }
            this.PresenceClient.OnReady += PresenceClientOnOnReady;
            this.PresenceClient.OnPresenceUpdate += PresenceClientOnOnPresenceUpdate;
            this.PresenceClient.SetPresence(new RichPresence()
            {
                Details = this.Details,
                State = this.State,
                Assets = new Assets()
                {
                    LargeImageKey = this.BigImageKey,
                    LargeImageText = this.BigImageText,
                    SmallImageKey = this.SmallImageKey,
                    SmallImageText = this.SmallImageText
                }
            });
            PresenceClient.UpdateStartTime();
        } 
        public void Stop()
        {
            this.IsRunning = false;
            //Console.WriteLine("Process stopped");
            RunRPCView.Data.LogCache.Value = new customEvent("[" + this.ProcessName + ".exe] ➜ stopped running", EventType.ProcessStoppedEvent);
            if (this.PresenceClient.IsInitialized)
            {
                this.PresenceClient.ClearPresence();
                this.PresenceClient.OnReady -= PresenceClientOnOnReady;
                this.PresenceClient.OnPresenceUpdate -= PresenceClientOnOnPresenceUpdate;
            }
            
        }
        private void PresenceClientOnOnPresenceUpdate(object sender, PresenceMessage args)
        {
            //Console.WriteLine("[" + this.ProcessName + ".exe] ➜ Received Update on " + args.Name);
            RunRPCView.Data.LogCache.Value = new customEvent("[" + this.ProcessName + ".exe] ➜ Received Update on " + args.Name, EventType.RPCUpdateEvent);
        }

        private void PresenceClientOnOnReady(object sender, ReadyMessage args)
        {
            //Console.WriteLine("[" + this.ProcessName + ".exe] ➜ Received Ready from user " + args.User.Username);
            RunRPCView.Data.LogCache.Value =
                new customEvent("[" + this.ProcessName + ".exe] ➜ Received Ready from user " + args.User.Username, EventType.RPCReadyEvent);
        }
        public void CheckState(Process[] processes)
        {
            if(String.IsNullOrEmpty(this.ProcessName))
                return;

            bool processFound = false;

            foreach (Process process in processes)
            {
                if (process.ProcessName == this.ProcessName)
                    processFound = true;
            }
            
            if(!this.IsRunning)
                if (processFound)
                    Start();
            
            if(this.IsRunning)
                if(!processFound)
                    Stop();
        }

        public void Save()
        {
            INIFile rpcFile = new INIFile(Global.AppDataPathRPC.Value + "\\" + this.UUID + ".rpc", true);
            rpcFile.SetValue(
                SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                SaveSettingsHandler.Data.DiscordRpcFiles.ProcessName,
                this.ProcessName);
            rpcFile.SetValue(
                SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                SaveSettingsHandler.Data.DiscordRpcFiles.ClientId,
                this.PresenceClient.ApplicationID);
            rpcFile.SetValue(
                SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                SaveSettingsHandler.Data.DiscordRpcFiles.State,
                this.State);
            rpcFile.SetValue(
                SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                SaveSettingsHandler.Data.DiscordRpcFiles.Details,
                this.Details);
            rpcFile.SetValue(
                SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                SaveSettingsHandler.Data.DiscordRpcFiles.BigImageKey,
                this.BigImageKey);
            rpcFile.SetValue(
                SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                SaveSettingsHandler.Data.DiscordRpcFiles.BigImageText,
                this.BigImageText);
            rpcFile.SetValue(
                SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                SaveSettingsHandler.Data.DiscordRpcFiles.SmallImageKey,
                this.SmallImageKey);
            rpcFile.SetValue(
                SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                SaveSettingsHandler.Data.DiscordRpcFiles.SmallImageText,
                this.SmallImageText);
            rpcFile.SetValue(
                SaveSettingsHandler.Data.DiscordRpcFiles.Section,
                SaveSettingsHandler.Data.DiscordRpcFiles.UUID,
                this.UUID);
        }
        
        public override string ToString()
        {
            return this.ProcessName + ".exe";
        }
        
    }
}