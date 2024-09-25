//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;

//public class JoystickD : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
//{
//    public RectTransform mJoystickClickZone;
//    private Vector3 originPos;//用于保存原始的位置
//    private MyJoystick mETCJoystick;
//    private RectTransform mETCJoystickRectTran;
//    private RectTransform mETCJoystickParentRectTransform;

//    //private void Start()
//    //{
//    //    mETCJoystick = transform.GetComponentInChildren<MyJoystick>();
//    //    mETCJoystickRectTran = mETCJoystick.transform as RectTransform;
//    //    mETCJoystickParentRectTransform = mETCJoystick.transform.parent.GetComponent<RectTransform>();
//    //    originPos = mETCJoystickRectTran.localPosition;
//    //}

//    //private void Update()
//    //{
//    //    if (Input.GetMouseButtonUp(0))
//    //    {
//    //        mETCJoystickRectTran.localPosition = originPos;
//    //    }
//    //}

//    //public void OnPointerDown(PointerEventData eventData)
//    //{
//    //    if (eventData.pointerPressRaycast.gameObject == mJoystickClickZone.gameObject)
//    //    {
//    //        Vector2 localPoint = Input.mousePosition;
//    //        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mETCJoystickParentRectTransform, localPoint, Camera.main, out localPoint))
//    //        {
//    //            mETCJoystickRectTran.localPosition = localPoint;
//    //        }
//    //    }

//    //    eventData.pointerId = mETCJoystick.pointId;
//    //    mETCJoystick.OnPointerDown(eventData);
//    //}

//    //public void OnPointerUp(PointerEventData eventData)
//    //{
//    //    eventData.pointerId = mETCJoystick.pointId;
//    //    mETCJoystick.OnPointerUp(eventData);
//    //}

//    //public void OnDrag(PointerEventData eventData)
//    //{
//    //    eventData.pointerId = mETCJoystick.pointId;
//    //    mETCJoystick.OnDrag(eventData);
//    //}
//}
