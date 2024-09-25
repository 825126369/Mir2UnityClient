//import { math } from "cc"; 
//import { Log } from "./Log";

//const { pseudoRandom ,randomRangeInt } = math;

//export class Wheel{


//    private wheel:number[] = [];
//    private wheel_weight:number[] = [];
//    private weightArray:number[] = [];
//    private total_weight:number = 0;

//    constructor(wheel:number[],wheel_weight:number[]){

//        this.wheel.push(...wheel);
//        this.wheel_weight.push(...wheel_weight);

//        if (this.wheel.length != this.wheel_weight.length){
//            Log.warn("the wheel lenght does not equal");
//        }

//        this.total_weight = this.get_total_weight();
//    }

//    public get_total_weight():number{
//        let total=0        
//        this.weightArray = []

//        for (const weight of this.wheel_weight) {
//            total+= weight
//            this.weightArray.push(total);
//        }
//        return total 
//    }


//    /*
//    # 接受 wheel= [1,2,3,4]  wheel_weight = [1,2,3,10] 这样分开配置的
//    :param wheel:  eg [1,2,3,4]
//    :param wheel_weight: eg [1,2,3,10]
//    :return:
//    '''
//    */
//    public static  createByWeight(wheel,wheel_weight):Wheel{
//        return new Wheel(wheel, wheel_weight)
//    }
   
//    /*
//     '''
//        接受 wheelconfig = {1:1,2:1,3:1,4:4} 这样配置的
//        :param dicConfig:
//        :return:
//        '''
//    */
//    public   static createByDic(dicConfig:object):Wheel{
//        let wheel:number[] = []
//        let wheel_weight:number[] = []

//        for (const key in dicConfig) {
//            if (Object.prototype.hasOwnProperty.call(dicConfig, key)) {
//                const element = dicConfig[key];
//                wheel.push(parseInt(key))
//                wheel_weight.push(parseInt(element))
//            }
//        }
//        return new Wheel(wheel, wheel_weight)        
//    }

//     /*
//        # 接受 wheelconfig = [1,2,2] 这样配置的.表示 1-权重1, 2-权重2
//    */
//    public  static createByList(listConfig:number[]):Wheel{
//        let wheel:number[] = []
//        let wheel_weight:number[] = []  
//        for (const key in listConfig) {
//            wheel.push(parseInt(key));
//            wheel_weight.push(1);
//        }
//        return new Wheel(wheel, wheel_weight)        
//    }

//    /*
//     '''
//        随机权重点
//        :return: int
//        '''
//    */

//    rand_index():number{
        
//        let rand_weight = Math.ceil(randomRangeInt(0,this.total_weight)); //GameUtils.randomInt(0,this.total_weight-1);
//        return this.get_cur_index(rand_weight)
//    }   
    
//    get_cur_index(rand_weight:number):number{
//        for (const index in this.weightArray) {
//            if (Object.prototype.hasOwnProperty.call(this.weightArray, index)) {
//                const value = this.weightArray[index];
//                if (rand_weight < value){
//                    return parseInt(index);
//                }
//            }
//        }  
//        return -1;      
//    }

//    // 返回转盘的真实值
//    spin(){
//        let rand_index = this.rand_index()
    
//        return this.wheel[rand_index]
//    }   
 

//    static Test(){

//        // step 1
//        // let testList = [9,8,7,6,5,4,3,2,1,0]
//        // let w = Wheel.createByList(testList);

//        //test2
//        let testList = [9,8,7,6,5,4,3,2,1,0]
//        let testList_w = [10,1,1,1,1,1,1,1,1,1]
//        let w = Wheel.createByWeight(testList,testList_w);

//        let rm:Map<string,number> = new Map<string,number>();

//        let szlen = 10000
//        for (let index = 0; index < szlen; index++) {            
//            let r = w.spin();            
//            // Log.d(`${index} :  random : ${r}`);
//            let rmv = rm.get(r.toString());
//            if(rmv){
//                rm.set(r.toString(),rmv+1);
//            }else{
//                rm.set(r.toString(),1);
//            }            
//        }

//        Log.d(`result:`,rm);
//        for (let index = 0; index < testList.length; index++) {
//            const v =  rm.get(index.toString());
//            Log.d(`${index} :`,v*1.0/szlen*100);
//        }
//    }

//}