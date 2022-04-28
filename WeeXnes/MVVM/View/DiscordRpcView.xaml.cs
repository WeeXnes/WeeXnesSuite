using Nocksoft.IO.ConfigFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeeXnes.Core;
using WeeXnes.RPC;

namespace WeeXnes.MVVM.View
{
    /// <summary>
    /// Interaktionslogik für DiscordRpcView.xaml
    /// </summary>
    public partial class DiscordRpcView : UserControl
    {
        //static bool shouldBeRunning = false;
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        List<Game> Games = new List<Game>();
        Game lastSelectedGame = null;
        public static string logContent = null;
        public static UpdateVar<string> triggerLogupdate = new UpdateVar<string>();
        public DiscordRpcView()
        {
            CheckFolders();
            InitializeComponent();
            triggerLogupdate.ValueChanged += () =>
            {
                if(logContent != null)
                {
                    writeLog(logContent);
                }
            };
            InitializeSettings();
            CheckForAutostart();
        }

        public void CheckFolders()
        {
            if (!Directory.Exists(Globals.settings_RpcItemsPath.Value))
            {
                Directory.CreateDirectory(Globals.settings_RpcItemsPath.Value);
                Console.WriteLine("Created settings_RpcItemsPath");
            }
        }

        private void CheckForAutostart()
        {
            if (Globals.info_RpcAutoStart)
            {
                Globals.info_RpcAutoStart = false;
                runBackgroundWorker();

            }
        }

        private void InitializeSettings()
        {
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            Refresh();
        }
        public void writeLog(string _content, bool _timestamp = true)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            if (_timestamp)
            {
                _content = "[" + timestamp + "] " + _content;
            }
            Console.WriteLine(_content);
            this.Dispatcher.Invoke(() =>
            {
                RpcLog.AppendText(_content + "\n");
                RpcLog.ScrollToEnd();
            });
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Globals.info_isRpcRunning = true;
            writeLog("Thread Started");
            bool runWorker = true;
            int delay = 2000;
            while (runWorker)
            {
                if (backgroundWorker.CancellationPending)
                {
                    runWorker = false;
                    delay = 0;

                    foreach (Game game in Games)
                    {
                        game.stop();
                    }
                }
                Process[] processes = Process.GetProcesses();
                foreach (Game game in Games)
                {
                    game.checkState(processes);
                }
                Thread.Sleep(delay);
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Globals.info_isRpcRunning = false;
            foreach(Game game in Games)
            {
                game.isRunning = false;
            }
            writeLog("Thread Closed");
        }
        public void runBackgroundWorker()
        {
            try
            {
                backgroundWorker.RunWorkerAsync();
            }
            catch (Exception e)
            {
                Misc.Message message = new Misc.Message(e.ToString());
                message.Show();
            }
        }
        public void stopBackgroundWorker()
        {
            try
            {
                backgroundWorker.CancelAsync();
            }
            catch (Exception e)
            {
                Misc.Message message = new Misc.Message(e.ToString());
                message.Show();
            }
        }

        public void Refresh()
        {
            readRpcFileDirectory();
            LoadGamesIntoListBox();
        }
        public void generateNewGame()
        {
            string filename = Guid.NewGuid().ToString() + ".rpc";
            Game newGame = new Game(filename, generateIncrementalName(), null, Globals.settings_RpcDefaultClientID.Value);
            saveGameToFile(newGame);
        }
        public string generateIncrementalName()
        {
            Random random = new Random();
            int randomInt = random.Next(1000);
            return "New Game " + randomInt;
        }
        public void saveGameToFile(Game game)
        {
            INIFile rpcFile = new INIFile(Globals.settings_RpcItemsPath.Value + "\\" + game.fileName, true);
            rpcFile.SetValue("config", "name", game.Name);
            rpcFile.SetValue("config", "pname", game.ProcessName);
            rpcFile.SetValue("config", "id", game.id);
            rpcFile.SetValue("config", "state", game.state);
            rpcFile.SetValue("config", "details", game.details);
            rpcFile.SetValue("config", "BigImgKey", game.bigimgkey);
            rpcFile.SetValue("config", "SmallImgKey", game.smallimgkey);
            rpcFile.SetValue("config", "BigImgText", game.bigimgtext);
            rpcFile.SetValue("config", "SmallImgText", game.smallimgtext);
            rpcFile.SetValue("config", "FileName", game.fileName);
        }
        public void deleteGameFile(Game game)
        {
            try
            {
                File.Delete(Globals.settings_RpcItemsPath.Value + "\\" + game.fileName);
            }
            catch (Exception e)
            {
                Misc.Message message = new Misc.Message(e.ToString());
                message.Show();
            }
        }
        public void readRpcFileDirectory()
        {
            bool Empty = funcs.IsDirectoryEmpty(Globals.settings_RpcItemsPath.Value);
            List<Game> readGames = new List<Game>();
            if (!Empty)
            {
                Console.WriteLine("RpcDir is not Empty, Reading content");
                string[] files = Directory.GetFiles(Globals.settings_RpcItemsPath.Value, "*.rpc", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    INIFile rpcFile = new INIFile(file);
                    string Name = rpcFile.GetValue("config", "name");
                    string ProcessName = rpcFile.GetValue("config", "pname");
                    string id = rpcFile.GetValue("config", "id");
                    string state = rpcFile.GetValue("config", "state");
                    string details = rpcFile.GetValue("config", "details");
                    string bigimgkey = rpcFile.GetValue("config", "BigImgKey");
                    string smallimgkey = rpcFile.GetValue("config", "SmallImgKey");
                    string bigimgtxt = rpcFile.GetValue("config", "BigImgText");
                    string smallimgtxt = rpcFile.GetValue("config", "SmallImgText");
                    string FileName = rpcFile.GetValue("config", "FileName");
                    Game newGame = new Game(FileName, Name, ProcessName, id, state, details, bigimgkey, smallimgkey, bigimgtxt, smallimgtxt);
                    readGames.Add(newGame);
                }
                Games.Clear();
                foreach (Game game in readGames)
                {
                    //Console.WriteLine("Added " + game + " from file");
                    Games.Add(game);
                }
            }
        }
        public void LoadGamesIntoListBox()
        {
            RpcItemList.Items.Clear();
            foreach (Game game in Games)
            {
                RpcItemList.Items.Add(game);
            }
        }

        private void RpcItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RpcItemList.SelectedItem != null)
            {
                Game selectedGame = RpcItemList.SelectedItem as Game;
                lastSelectedGame = selectedGame;


                tb_FormPName.Text = selectedGame.ProcessName;
                tb_FormClient.Text = selectedGame.id;
                tb_FormName.Text = selectedGame.Name;
                tb_FormState.Text = selectedGame.state;
                tb_FormDetails.Text = selectedGame.details;
                tb_FormLargeImgKey.Text = selectedGame.bigimgkey;
                tb_FormLargeImgTxt.Text = selectedGame.bigimgtext;
                tb_FormSmallImgKey.Text = selectedGame.smallimgkey;
                tb_FormSmallImgTxt.Text = selectedGame.smallimgtext;
            }
        }

        private void RpcItemList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RpcItemList.SelectedItem != null)
            {
                deleteGameFile((Game)RpcItemList.SelectedItem);
            }
            Refresh();
        }

        private void DiscordRpcSave_Click(object sender, RoutedEventArgs e)
        {
            Game selectedItem = lastSelectedGame;
            if (selectedItem != null)
            {
                Game editedGame = new Game(
                    selectedItem.fileName, 
                    tb_FormName.Text, 
                    tb_FormPName.Text, 
                    tb_FormClient.Text, 
                    tb_FormState.Text, 
                    tb_FormDetails.Text, 
                    tb_FormLargeImgKey.Text, 
                    tb_FormSmallImgKey.Text, 
                    tb_FormLargeImgTxt.Text, 
                    tb_FormSmallImgTxt.Text
                );
                saveGameToFile(editedGame);

            }
            Refresh();
        }

        private void DiscordRpcStop_Click(object sender, RoutedEventArgs e)
        {
            stopBackgroundWorker();
        }

        private void DiscordRpcStart_Click(object sender, RoutedEventArgs e)
        {
            runBackgroundWorker();
        }

        private void DiscordRpcNew_Click(object sender, RoutedEventArgs e)
        {
            generateNewGame();
            Refresh();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            stopBackgroundWorker();

        }
    }
}
