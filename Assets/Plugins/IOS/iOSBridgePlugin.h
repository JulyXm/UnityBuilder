#import "RealReachability.h"

//!---------------------------外部接口声明OC中调用Unity的接口--------------------------------------------
#if defined(__cplusplus)
extern "C"{
#endif
    extern void UnitySendMessage(const char*, const char*, const char*);
#if defined(__cplusplus)
}
#endif

//提供给Unity C#代码调用的接口声明
extern "C" {
	 //初始化reachAbility接口
     int InitNetReachAbility();
	 //销毁reachAbility接口
     void UnInitNetReachAbility();
 }

@interface IOSBridge : NSObject
+(IOSBridge*) GetInst;
//初始化并返回当前的网络状态 -1：未联网 0：未知网络 1: WIFI 2:2G网络 3:3G网络 4:4G网络	
-(int) InitReachAbility;
//销毁 移除监听
-(void) UnInitReachAbility;
//网络状态改变监听函数
-(int) networkChanged:(NSNotification*) notification;
//处理网络状态，并回调到Unity中
-(int) handleNetworkChanged:(ReachabilityStatus) status;
@end

