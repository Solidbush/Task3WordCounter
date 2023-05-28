using System.Collections.Generic;
using System.ServiceModel;

namespace WordCounter
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract(CallbackContract = typeof(IWordCounterCallBack))]
    public interface IWordCounter
    {
        [OperationContract]
        void Connect();

        [OperationContract(IsOneWay = true)]
        void CountWords(string text);

    }

    public interface IWordCounterCallBack
    {
        [OperationContract(IsOneWay = true)]
       void CountWordsCallBack(Dictionary<string, int> answer);
    }
}
