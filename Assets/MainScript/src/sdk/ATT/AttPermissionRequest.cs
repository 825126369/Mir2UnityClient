using UnityEngine;
#if UNITY_IOS
// 在 iOS 上运行时包括 IosSupport 命名空间：
using Unity.Advertisement.IosSupport;
#endif
 
public class AttPermissionRequest : SingleTonMonoBehaviour<AttPermissionRequest> {
    
    // void Awake() {
    // }

    public void Init(){
        #if UNITY_IOS
                // 检查用户的同意状态。
                // 在状态未确定时显示请求：
                if(ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED) {
                    ATTrackingStatusBinding.RequestAuthorizationTracking();
                }
        #endif
    }


}