using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPublisher
{
    void RegisterBroker();
    void PushMessageToBroker(EMessageType _message);
}
