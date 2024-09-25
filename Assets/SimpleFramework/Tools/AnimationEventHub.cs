using UnityEngine;
using System;



public class AnimationEventHub : MonoBehaviour
{
	public void AnimationEventFunc(string strParam)
	{
		Debug.Log("AnimationEventFunc: " + strParam);
		// if (m_LuaAnimationEventFunc == null)
		// {
		// 	m_LuaTable = LuaClient.Instance.GetLuaState().Global.Get<LuaTable>("AnimationEventHub");
		// 	m_LuaAnimationEventFunc = m_LuaTable.Get<Action<LuaTable, string>>("AnimationEventFunc");
		// }
		// m_LuaAnimationEventFunc(m_LuaTable, strParam);
	}
}
