using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Tools
{
//    /**
//     * 摁键事件列队
//     * @param tNode 脚本的node
//     * @param tComponent 脚本名称
//     * @param handler 脚本中的函数名
//     * @param param 自定义传参
//     * @returns 发回buttom注册任务信息
//     */
//    public static ClickEventTool(tNode: Node, tComponent: string, thandler: string, tparam?: any): EventHandler {
//        let returnValue = new EventHandler();
//        returnValue.target = tNode;
//        returnValue.component = tComponent;
//        returnValue.handler = thandler;
//        returnValue.customEventData = tparam;
//        return returnValue;
//    }

    //    /**
    //     * 摁键事件列队
    //     * @param tNode 脚本的node
    //     * @param tComponent 脚本名称
    //     * @param handler 脚本中的函数名
    //     * @param param 自定义传参
    //     * @returns 发回buttom注册任务信息
    //     */
    //    public static ClickEventTool(tNode: Node, tComponent: string, thandler: string, tparam?: any): EventHandler {
    //        let returnValue = new EventHandler();
    //        returnValue.target = tNode;
    //        returnValue.component = tComponent;
    //        returnValue.handler = thandler;
    //        returnValue.customEventData = tparam;
    //        return returnValue;
    //    }

    //    /**
    //     * 只copy值。如果对象有嵌套的。需要单独copy。
    //     * @param source       赋值的数据结构来源对象。
    //     * @param destination  修改的数据结构对象
    //     */

    //    public static copyValue(source: any, destination: any) {
    //        for (const key in destination) {
    //            if (destination.hasOwnProperty(key) && source.hasOwnProperty(key)) {
    //                destination[key] = source[key];
    //            }
    //        }
    //    }

    public static bool IsIos()
    {
        return Application.platform == RuntimePlatform.IPhonePlayer;
    }

    public static bool IsAndroid()
    {
        return Application.platform == RuntimePlatform.Android;
    }

    public static bool IsMobilePlatform(){
        return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }


    // ios的所有内置字体  https://www.jianshu.com/p/1024ee910dc8
    // exclude  排除的节点的名称.
    public static void updateFont(GameObject parentNode, string fontFamily = "Arial Rounded MT Bold", TSArray<string> exclude = null)
    {
        
    }
    public static void UpdateFont(GameObject parentNode, string fontFamily = "Arial Rounded MT Bold", TSArray<string> exclude = null)
    {
        
    }

    public static bool orTodayForTimeStamp(ulong nTimeStamp)
    {
        DateTime recordDateTime = TimeTool.GetLocalTimeFromTimeStamp(nTimeStamp);
        DateTime nowDateTime = System.DateTime.Now;
        return nowDateTime.Day == recordDateTime.Day && nowDateTime.Month == recordDateTime.Month && nowDateTime.Year == recordDateTime.Year;
    }
    
    public static bool orDifferentDay(ulong nTimeStamp)
    {
        return orTodayForTimeStamp(nTimeStamp) == false;
    }

//    //填充数字，前面补充0。
//    //eg .num 2 2-> 02  /   5,3-> 005
//    public static padding(num: number, length: number): string {
//        //这里用slice和substr均可
//        return (Array(length).join("0") + num).slice(-length);
//    }

//    /**
//     * 洗牌方式进行打乱
//     * @param arr 
//     * @returns 
//     */
//    public static shuffle<T>(arr: Array<T>) {
//        let m = arr.length;
//        let t: T;
//        let i = 0;
//        while (m >= 0) {
//            i = Math.floor(Math.random() * m--);
//            t = arr[m];
//            arr[m] = arr[i];
//            arr[i] = t;
//        }
//        return arr;
//    }

//    public static loadPhoto(remoteUrl: string, callback: Function | any): void {
//        assetManager.loadRemote(remoteUrl, { type: 'png' }, function (err, texture) {
//            // Use texture to create sprite frame
//            Log.d("load photo");
//            if (!err) {
//                if (callback) {
//                    callback(texture);
//                }
//            }
//        });
//    }

//    public static padTo2Digits(num: number, digitNum: number): string {
//        return num.toString().padStart(digitNum, '0');
//    }

//    public static formatDateYMD(date: Date): string {
//        return [
//            date.getFullYear(),
//            Tools.padTo2Digits(date.getMonth() + 1, 2),
//            Tools.padTo2Digits(date.getDate(), 2),
//        ].join('-').toString();
//    }

//    //格式化数字，如果有小数，保留两位。自动添加对应的单位。
//    public static formartNumber(inputnum: number = 1,ignore:number =1000): string {

//        if (inputnum < ignore) {
//            return inputnum.toString();
//        }
//        let dw = ["K", "M", "B", "T", "P", "E", "Z", "Y", "B", "N", "D"];
//        let digits = [3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33];

//        let middle = Math.floor(digits.length / 2);
//        let testNum = Math.floor(Number(inputnum || 0));
//        let start = 0;
//        let end = digits.length - 1;
//        let next = true;
//        let times = 0;
//        while (next) {
//            let dnum = Math.pow(10, digits[middle]);
//            times = testNum / dnum;
//            if (times >= 1000) {
//                start = middle;
//                middle = Math.floor((end + middle) / 2);
//            } else if (times < 1) {
//                end = middle;
//                middle = Math.floor((start + middle) / 2);
//            } else {
//                next = false;
//            }
//        }

//        let tmp = times.toString();
//        if (tmp.indexOf(".") == -1) {
//            return tmp + dw[middle];
//        } else {
//            let ret = times.toFixed(1);
//            return ret + dw[middle];
//        }
//    }

//    public static formartNumberKMB(inputnum: number = 1): string {

//        if(inputnum<1000){
//            return String(inputnum)
//        }
//        if(inputnum>Math.pow(10,9)){
//            return String(Math.floor(inputnum/Math.pow(10,9)*100)/100)+"B"
//        }
//        else if(inputnum>Math.pow(10,6)){
//            return String(Math.floor(inputnum/Math.pow(10,6)*100)/100)+"M"
//        }
//        else if(inputnum>Math.pow(10,3)){
//            return String(Math.floor(inputnum/Math.pow(10,3)*100)/100)+"K"
//        }
//        return inputnum.toString();
//    }

//    //格式化数字，如果有小数，保留两位。没有单位，只有逗号。
//    public static formartNumberWithDot(inputnum: number): string {

//        if (inputnum < 1000) {
//            return inputnum.toString();
//        }

//        let inputStr = inputnum.toString();
//        let numbersz = inputStr.length;
//        let retStr = "";
//        let start = numbersz - 3;
//        let end = numbersz;
//        let next = true;
//        while (next) {
//            let substr = inputStr.substring(start, end);
//            retStr = substr + retStr;
//            end = end - 3;
//            start = end - 3;
//            if (start < 0) {
//                next = false;
//                if (end > 0) {
//                    retStr = inputStr.substring(start, end) + "," + retStr;
//                }
//            } else {
//                next = true;
//                retStr = "," + retStr;
//            }
//        }
//        return retStr;
//    }

public static string GetRandomUUID()
{
    long d = DateTime.Now.Ticks;

    string uuid = "xxxx-xxxx-xxxx-yxxx";
    uuid = Regex.Replace(uuid, "[xy]", c =>
    {
        int r = (int)(d + new System.Random().Next(16)) % 16;
        d = (long)Math.Floor((double)d / 16);
        return (c.ToString() == "x" ? r : (r & 0x3 | 0x8)).ToString("x");
    });

    return uuid;
}

//    public static getRandomEquipUUid(): string {
//        let d = new Date().getTime();
//        if (window.performance && typeof window.performance.now === "function") {
//            d += performance.now(); //use high-precision timer if available
//        }
//        let uuid = 'xxxxyxyxy'.replace(/[xy]/g, function (c) {
//            let r = (d + Math.random() * 16) % 16 | 0;
//            d = Math.floor(d / 16);
//            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
//        });
//        let now =  new Date();
//        let prefix= now.getFullYear().toString()+"-"+ now.getUTCFullYear()+"-"+now.getUTCDay()+"-"+now.getUTCHours();
//        return prefix+uuid;
//    }


//    /** 对象是否是数组 */
//    public static IsArray(obj: any) {
//        return obj && typeof obj == "object" && obj instanceof Array;
//    }

//    /** 深拷贝 */
//    public static T DeepClone<T>( T tSource , T tTarget): T {
//        if (Tools.IsArray(tSource)) {
//            tTarget = tTarget || [];
//        } else {
//            tTarget = tTarget || {};
//        }
//        for (const key in tSource) {
//            if (Object.prototype.hasOwnProperty.call(tSource, key)) {
//                if (typeof tSource[key] === "object" && typeof tSource[key] !== null) {
//                    tTarget[key] = Tools.IsArray(tSource[key]) ? [] : {};
//                    this.DeepClone(tSource[key], tTarget[key]);
//                } else {
//                    tTarget[key] = tSource[key];
//                }
//            }
//        }
//        return tTarget as T;
//    }
//    /** 浅拷贝 */
//    public static SimpleClone<T>(tSource: T, tTarget?: Record<string, any> | T): T {
//        if (Tools.IsArray(tSource)) {
//            tTarget = tTarget || [];
//        } else {
//            tTarget = tTarget || {};
//        }
//        for (const key in tSource) {
//            if (Object.prototype.hasOwnProperty.call(tSource, key)) {
//                tTarget[key] = tSource[key];
//            }
//        }
//        return tTarget as T;
//    }

//    public static formatTimeToHMs(timestamp) {
//        let useingTime = timestamp / 1000;
//        // let d:any = parseInt((useingTime / 60 / 60 / 24).toString());
//        // d = d < 10 ? '0' + d : d;
//        //得到小时 格式化成前缀加零的样式
//        let h:any = parseInt((useingTime / 60 / 60).toString());
//        h = h < 10 ? '0' + h : h;
//        //得到分钟 格式化成前缀加零的样式
//        let m:any = parseInt((useingTime / 60 % 60).toString());
//        m = m < 10 ? '0' + m : m;
//        //得到秒 格式化成前缀加零的样式
//        let s:any = parseInt((useingTime % 60).toString());
//        s = s < 10 ? '0' + s : s;
//        return `${h}:${m}:${s}`; //00天 00:39:58秒

//    } 
   public static string FormatTimeToHMs_1(int timestamp) 
   {
       int useingTime = timestamp;
       int h = useingTime / 3600;
       string hStr = h < 10 ? "0" + h : "" + h;
       int m = useingTime / 60 % 60;
       string mStr = m < 10 ? "0" + m : "" + m;
       int s = useingTime % 60;
       string sStr = s < 10 ? "0" + s : "" + s;
       if(h <= 0)
       {
           return $"{mStr}:{sStr}";
       }
       return $"{hStr}:{mStr}:{sStr}"; //00天 00:39:58秒
   }


//    // MATH
//    //  p1->p2的角度。
//    public static getAngle(pos1:Vec3,pos2:Vec3){
//        let theangle  = Math.atan2(pos2.y - pos1.y, pos2.x - pos1.x);  //弧度  
//        let theta  = theangle * 180 / Math.PI;  //角度
//        return theta;


//        // let t1 = Math.atan(1);
//        // let t2 = Math.atan(2);
//        // let t3 = Math.atan(-1);
//        // let t4 = Math.atan(-0.5);
//        // Log.d(t1,t2,t3,t4)

//    }
//    //取两点之间均匀的点坐标
//    public static getPointsFromFromTo(from:Vec2,to:Vec2,num:number){
//        let points = new Array<Vec2>()
//        for (let i = 0; i < num; i += 1) {
//            let result = from.lerp(to, (i + 1) / num);
//            points.push(result);
//        }
//        return points
//    }
//    public sendError(){
//        let consoleError = window.console.error;
//        window.console.error = function () {
//            Log.d("=======>", JSON.stringify(arguments));
//            consoleError && consoleError.apply(window, arguments);
//        }
//    }
//    public static getDistance(start, end){
//        var pos =  new Vec2(start.x - end.x, start.y - end.y);
//        var dis = Math.sqrt(pos.x*pos.x + pos.y*pos.y);
//        return dis;
//    }
//    public static getDistanceFromWorld(node1, node2){
//        let start = node1.getParent().getComponent(UITransform).convertToWorldSpaceAR(node1.getPosition())
//        let end = node2.getParent().getComponent(UITransform).convertToWorldSpaceAR(node2.getPosition())
//        var pos =  new Vec2(start.x - end.x, start.y - end.y);
//        var dis = Math.sqrt(pos.x*pos.x + pos.y*pos.y);
//        return dis;
//    }

   public static float GetDistanceAtX(GameObject node1,GameObject node2){
        //let start = node1.getParent().getComponent(UITransform).convertToWorldSpaceAR(node1.getPosition())
        //let end = node2.getParent().getComponent(UITransform).convertToWorldSpaceAR(node2.getPosition())
        //let dis = Math.abs(end.x - start.x);
        float dis = 100;
        PrintTool.Log("GetDistanceAtX TODO:");
       return dis;
   }


   public static Vector3 GetWorldPt(GameObject obj) {        
    //    let uit = node.getComponent(UITransform);                
    //    return uit.convertToWorldSpaceAR(sonNodePt);   
        return obj.transform.position;

   }


//    // timestamp
//    public static timestampToDate(timestamp: number): number
//    { 
//        const date = new Date(timestamp);
//        const year = date.getFullYear();
//        date.getUTCDay()
//        const month = date.getMonth();// + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1;
//        const day =  date.getDate();// < 10 ? '0' + (date.getDate()) : date.getDate();
//        const ret = year*10000+month*100+day;

//        return ret;
//    }

//    // 16进制的字符串->颜色
//    public static creatColor(colorStr: string): Color{
//        let c = new Color(255,255,255,255);
//        c = Color.fromHEX(c,colorStr)
//        return c;
//    } 

//    // 调用
//    //Tools.bezierTo(this.spNode,2,v2(-200,-200),v2(200,0),v3(350,400,0),null).start()
//    /**
//     *  二阶贝塞尔曲线 运动
//     * @param target
//     * @param {number} duration
//     * @param {} c1 起点坐标
//     * @param {} c2 控制点
//     * @param {Vec3} to 终点坐标
//     * @param opts
//     * @returns {any}
//     */
//    public static bezierTo(target: any, duration: number, c1: Vec2, c2: Vec2, to: Vec3, opts: any) {
//        opts = opts || Object.create(null);
//        /**
//         * @desc 二阶贝塞尔
//         * @param {number} t 当前百分比
//         * @param {} p1 起点坐标
//         * @param {} cp 控制点
//         * @param {} p2 终点坐标
//         * @returns {any}
//         */
//        let twoBezier = (t:number, p1: Vec2, cp: Vec2, p2: Vec3) => {
//            let x = (1 - t) * (1 - t) * p1.x + 2 * t * (1 - t) * cp.x + t * t * p2.x;
//            let y = (1 - t) * (1 - t) * p1.y + 2 * t * (1 - t) * cp.y + t * t * p2.y;
//            return v3(x, y, 0);
//        };
//        opts.onUpdate = (arg: Vec3, ratio: number) => {
//            target.position = twoBezier(ratio, c1, c2, to);
//        };
//        return tween(target).to(duration, {}, opts);
//    }

//    public static ptDist(frompt:Vec3,topt:Vec3):number {

//        return frompt.clone().subtract(topt.clone()).length();

//    }



//    // 按照距离分割，获取上面的点。
//    public static distPts(frompt:Vec3,topt:Vec3,dist:number,maxCount:number=20):Array<Vec3> {
//        let points = new Array<Vec3>();
//        let dir = topt.clone().subtract(frompt.clone()).normalize();
//        let len = topt.clone().subtract(frompt.clone()).length();
//        let num = Math.floor(len/dist);

//        if(num > maxCount){
//            num = maxCount;
//        }

//        for (let i = 0; i < num; i += 1) {
//            let result = frompt.clone().add(dir.clone().multiplyScalar(dist*i));
//            points.push(result);
//        }
//        points.push(topt.clone());        
//        return points;
//    }
//    public static getZheng(num_all:number,num_qu:number){
//        let zheng = Math.floor(num_all/num_qu)
//        return zheng
//    }
//    public static getYu(num_all:number,num_qu:number){
//        let zheng = Tools.getZheng(num_all,num_qu)
//        let yu = num_all - num_qu*zheng
//        return yu
//    }


    public static string UUIDTime(){
        
        DateTime now = DateTime.Now;        
        // now格式化字符串，有毫秒
        return now.ToString("yyyyMMddHHmmssfff");
    }

   public static string GetUUID(){
        var uuid = LocalStorageTool.GetString("GameUID","0");    
        if(uuid == "0"){
            uuid = Tools.UUIDTime(); //Tools.GetRandomUUID();
            LocalStorageTool.SetString("GameUID",uuid);
        }
        return uuid;
   }
   

//    public static SetUILocalPosByOtherUILocalPos(targetNode:Node, targetPosNode:Node):void
//    {
//        let tow = targetPosNode.parent.getComponent(UITransform).convertToWorldSpaceAR(targetPosNode.position)
//        let toe = targetNode.parent.getComponent(UITransform).convertToNodeSpaceAR(tow)
//        targetNode.position = toe;
//    }

//    public static GetUILocalPosByOtherNode(targetNodeParent:Node, targetPosNode:Node):Vec3
//    {
//        let tow = targetPosNode.parent.getComponent(UITransform).convertToWorldSpaceAR(targetPosNode.position)
//        let toe = targetNodeParent.getComponent(UITransform).convertToNodeSpaceAR(tow)
//        return toe;
//    }
        
}


