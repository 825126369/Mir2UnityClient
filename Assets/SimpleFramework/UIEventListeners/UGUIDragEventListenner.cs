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

public class UGUIDragEventListenner :MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Action<PointerEventData> OnBeginDragEvent;
    public Action<PointerEventData> OnDragEvent;
    public Action<PointerEventData> OnEndDragEvent;

    static public UGUIDragEventListenner Get(GameObject go)
    {
        UGUIDragEventListenner listener = go.AddMissComponent<UGUIDragEventListenner>();
        return listener;
    }
        
    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragEvent?.Invoke(eventData);
    }

}
