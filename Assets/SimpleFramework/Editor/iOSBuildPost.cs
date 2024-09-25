using System.Net.Mime;
using UnityEditor;
using UnityEditor.Callbacks;

using UnityEngine;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;
 
public class PostBuildStep {
    // 设置 IDFA 请求描述：
    const string k_TrackingDescription = "Your data will be used to provide you a better and personalized ad experience.";
    const string k_appName = "Witt Solitaire";

 #if UNITY_IOS
    [PostProcessBuild(0)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToXcode) {
        if (buildTarget == BuildTarget.iOS) {
            AddPListValues(pathToXcode);

            DisableSwift(buildTarget, pathToXcode);
            AddAssociatedDomains(pathToXcode);
        }
    }
 
    // 实现一个函数以便向 plist 文件读取和写入值：
    static void AddPListValues(string pathToXcode) {
        // 从 Xcode 项目目录中获取 plist 文件：
        string plistPath = pathToXcode + "/Info.plist";
        PlistDocument plist = new PlistDocument();
 
    
        // 从 plist 文件中读取值：
        plist.ReadFromString(File.ReadAllText(plistPath));
 
        // 设置来自根对象的值：
        PlistElementDict plistRoot = plist.root;
 
        // 在 plist 中设置描述键值：
        // if (!plistRoot.values.ContainsKey("NSUserTrackingUsageDescription")){
        //     plistRoot.SetString("NSUserTrackingUsageDescription", k_TrackingDescription);    
        // }

        plistRoot.SetString("NSUserTrackingUsageDescription", k_TrackingDescription); 
        plistRoot.SetString("FacebookDisplayName", k_appName);


        // 保存对 plist 的更改：
        File.WriteAllText(plistPath, plist.WriteToString());
    }


    // [PostProcessBuildAttribute(Int32.MaxValue)] //We want this code to run last!
    public static void DisableSwift(BuildTarget buildTarget, string pathToBuildProject)
    {
        #if UNITY_IOS
            if (buildTarget != BuildTarget.iOS) return; // Make sure its iOS build
            
            // Getting access to the xcode project file
            string projectPath = pathToBuildProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromFile(projectPath);
            
            // Getting the UnityFramework Target and changing build settings
            string target = pbxProject.GetUnityFrameworkTargetGuid();
            pbxProject.SetBuildProperty(target, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");

            // After we're done editing the build settings we save it 
            pbxProject.WriteToFile(projectPath);
        #endif
    }

    private static void AddAssociatedDomains(string pathToBuiltProject)
    {
        //This is the default path to the default pbxproj file. Yours might be different
        string projectPath = "/Unity-iPhone.xcodeproj/project.pbxproj";
        //Default target name. Yours might be different
        string targetName = "Unity-iPhone";
        //Set the entitlements file name to what you want but make sure it has this extension
        string entitlementsFileName = "my_app.entitlements";
        
        var entitlements = new ProjectCapabilityManager(pathToBuiltProject + projectPath, entitlementsFileName, targetName);
        entitlements.AddAssociatedDomains(new string[] { "applinks:www.wittgames.com" });
        entitlements.WriteToFile();
    }

#endif

}