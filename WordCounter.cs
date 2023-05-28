using System;
using System.Collections.Generic;
using System.ServiceModel;


namespace WordCounter
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WordCounter : IWordCounter
    {
        ServerUser user = new ServerUser();
        public void Connect()
        {
            user.operationContext = OperationContext.Current;
        }

        public void CountWords(string text)
        {
            Console.WriteLine("CountWords on server");
            FileReaderLib.WordCounter wordCounter = new FileReaderLib.WordCounter();
            Dictionary<string, int> answer = wordCounter.CountWordsThread(text);
            Console.WriteLine(answer);
            try
            {
                user.operationContext.GetCallbackChannel<IWordCounterCallBack>().CountWordsCallBack(answer);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
