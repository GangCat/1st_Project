using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broker
{
    public static void Regist(IPublisher _pub, EPublisherType _publisherType)
    {
        arrListSub[(int)_publisherType] = new List<ISubscriber>();
    }

    public static void Subscribe(ISubscriber _sub, EPublisherType _publisherType)
    {
        arrListSub[(int)_publisherType].Add(_sub);
    }

    public static void UnSubscribe(ISubscriber _sub, EPublisherType _publisherType)
    {
        arrListSub[(int)_publisherType].Remove(_sub);
    }

    public static void AlertMessageToSub(EMessageType _message, EPublisherType _publisherType)
    {
        for (int i = 0; i < arrListSub[(int)_publisherType].Count; ++i)
            arrListSub[(int)_publisherType][i].ReceiveMessage(_message);
    }

    private static List<ISubscriber>[] arrListSub = new List<ISubscriber>[(int)EPublisherType.LENGTH];
}