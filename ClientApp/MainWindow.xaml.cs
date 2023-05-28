using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;


namespace ClientApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, WordCounter.IWordCounterCallback
    {
        bool isConnected = false;
        WordCounter.WordCounterClient client;
        string text = null;
        FileInfo fileForRead;
        FileInfo fileForWrite;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }

        void ConnectUser()
        {
            client = new WordCounter.WordCounterClient(new System.ServiceModel.InstanceContext(this));
            if (!isConnected)
            {
                client.Connect();
                bConnDiconn.Content = "Disconnect";
                isConnected = true;
                tbMessageLog.Items.Add(client.ToString());
            }
        }

        void DisconnectUser()
        {
            if (isConnected)
            {
                client = null;
                bConnDiconn.Content = "Connect";
                isConnected = false;
            }
        }

        private void ConnectClick(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
                tbMessageLog.Items.Add("Here! Connection");
            }
        }

        public void CountWordsCallBack(Dictionary<string, int> answer)
        {
            tbMessageLog.Items.Add("Here");
            tbMessageLog.Items.Add(answer);
            StreamWriter fileWriter = new StreamWriter(fileForWrite.FullName, false, Encoding.Default);
            foreach (var wordCountPair in answer)
            {
                fileWriter.WriteLine($"{wordCountPair.Key} {wordCountPair.Value}");
            }

            fileWriter.Close();
            tbMessageLog.Items.Add($"Words have been successfully written to file: {fileForWrite.FullName}");
            tbMessageLog.Items.Add($"Total: {answer.Count()}");
        }

        private void PathForRead(object sender, RoutedEventArgs e)
        {
            fileForRead = new FileInfo(tbFilePathForRead.Text);
            if (!fileForRead.Exists)
                throw new Exception($"File with path: {fileForRead.FullName} doesn't exists!");

            text = File.ReadAllText(fileForRead.FullName, System.Text.Encoding.Default).Replace("\n", " ");

            tbMessageLog.Items.Add($"File from {fileForRead.FullName} read successfully!");

        }

        private void PathForWrite(object sender, RoutedEventArgs e)
        {
            tbMessageLog.Items.Add("Here file for write");
            fileForWrite = new FileInfo(tbFilePathForWrite.Text);
            if (!fileForWrite.Exists)
            {
                var forRead = File.Create(fileForRead.FullName);
                forRead.Close();
            }

            if (isConnected = true && text != null)
            {
                client.CountWords(text);
                tbMessageLog.Items.Add("Request sent to server!");
            }
            else
            {
                tbMessageLog.Items.Add("Something was wrong!");
            }
        }
    }
}
