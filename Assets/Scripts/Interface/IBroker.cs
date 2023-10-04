using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBroker
{
    void Regist(IPublisher _pub, EPublisherType _publisherType);
    void Subscribe(ISubscriber _sub, EPublisherType _publisherType);
    void AlertMessageToSub(EMessageType _message, EPublisherType _publisherType);
}
