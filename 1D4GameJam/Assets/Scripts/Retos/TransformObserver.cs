using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformObserver : MonoBehaviour
{
    public delegate void ChildChangedAction();
    public event ChildChangedAction OnChildChanged;

    void OnTransformChildrenChanged()
    {
        if (OnChildChanged != null)
        {
            OnChildChanged();
        }
    }
}
