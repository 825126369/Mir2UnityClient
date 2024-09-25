using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UploadFeiShuTool
{
    [System.Serializable]
    public class MsgObj
    {
        public string msg_type = null;
        public MsgObjContent content = null;
    }

    [System.Serializable]
    public class MsgObjContent
    {
        public string text = null;
    }
    
    public static void Do(string msg)
    {
        if(GameConst.isMobilePlatform())
        {
            MsgObj mMsg = new MsgObj();
            mMsg.msg_type = "text";
            mMsg.content = new MsgObjContent();
            mMsg.content.text = "Unity INFO: " + msg;
            
            var sendMsg = JsonUtility.ToJson(mMsg);
            var feishuURL = GameConst.feishuURL;

            if(minigameconfig.Instance.gameCanDebug){
                WWWTools.Instance.PostJsonData(feishuURL, sendMsg, (result) =>
                            {
                                Debug.Log("上传飞书: " + result);
                            });
               // TAController.Instance.TrackError(sendMsg);
            } 
            Debug.Log("Error :  " + sendMsg);
            
            //TAController.Instance.TrackError(sendMsg);

        }
    }

}
