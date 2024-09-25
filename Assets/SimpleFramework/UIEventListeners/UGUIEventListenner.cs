//----------------------------------------------
//            ColaFramework
// Copyright © 2018-2049 ColaFramework 马三小伙儿
//----------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

public class UGUIEventListenner : MonoBehaviour, IPointerClickHandler
{
    public Action<PointerEventData> OnPointerClickEvent;
    static public UGUIEventListenner Get(GameObject go)
    {
        UGUIEventListenner listener = go.AddMissComponent<UGUIEventListenner>();
        return listener;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClickEvent?.Invoke(eventData);
    }
}
