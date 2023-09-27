using System;
using UnityEngine;

namespace Common.GameLogic.Abstract
{
    public interface ITriggerObserver
    {
        event Action<Collider> TriggerEnter;
        event Action<Collider> TriggerExit;
    }
}