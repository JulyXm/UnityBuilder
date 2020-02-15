 #import "iOSBridgePlugin.h"
 
//提供给Unity C#代码调用的接口实现
//初始化ReachAbility
 int InitNetReachAbility(){
     //NSLog(@"[iOS Native] I AM IOS Function!");
    int ret =  [[IOSBridge GetInst] InitReachAbility] ;
    return ret;
 }
// 销毁ReachAbility
void UnInitNetReachAbility(){
    [[IOSBridge GetInst] UnInitReachAbility];
}

//IOSBridge类的实现

@implementation IOSBridge
//单例模式中的唯一实例
static IOSBridge* _sharedInstance = nil;
//单例模式获取实例接口
+ (IOSBridge*)  GetInst{
    @synchronized (self.class) {
        if(_sharedInstance == nil){
            _sharedInstance = [[self.class alloc] init];
        }
        
        return _sharedInstance;
    }
}

//初始化并返回当前的网络状态 -1：未联网 0：未知网络 1: WIFI 2:2G网络 3:3G网络 4:4G网络	
- (int) InitReachAbility
{
	//设置用于ping的远端地址 这里使用baidu和apple的地址
    GLobalRealReachability.hostForPing = @"www.baidu.com";
    GLobalRealReachability.hostForCheck = @"www.apple.com";
	//调用接口开启网络监听
    [GLobalRealReachability startNotifier];
	//注册一个观察者，用于放回当前的网络状态ReachabilityStatus的改变
	//监听函数设置为本类的networkChanged方法
    [[NSNotificationCenter defaultCenter] addObserver:(self) selector: @selector(networkChanged:) name:kRealReachabilityChangedNotification object:(nil)];
    ReachabilityStatus status = [GLobalRealReachability currentReachabilityStatus];
        NSLog(@"Initial reachability status:%@",@(status));
	//返回当前的网络状态	
    return [self handleNetworkChanged:status];
}
//销毁 移除监听
- (void) UnInitReachAbility
{
    [[NSNotificationCenter defaultCenter] removeObserver:self];
}

//处理网络状态，并回调到Unity中
-(int) handleNetworkChanged:(ReachabilityStatus) status
{
    NSLog(@"cur Status:%@",@(status));
    int ret = 0;
    //获取当前网络状态
    switch (status) {
        case RealStatusNotReachable:
            NSLog(@"RealStatusNotReachable");
            ret = -1;
            break;
        case RealStatusViaWiFi:
            NSLog(@"RealStatusViaWifi");
            ret = 1;
            break;
        case RealStatusViaWWAN:
            NSLog(@"RealStatusViaWWAN");
            break;
        case RealStatusUnknown:
            NSLog(@"RealStatusUnKnown");
            break;
        default:
            NSLog(@"Unkown Error!!!!!!");
            break;
    }
     
    //是否开启vpn
    BOOL isVpnON = [GLobalRealReachability isVPNOn];
    NSLog(@"isVPN ON :%d",isVpnON);
    //获取当前网络连接途径
    if(status == RealStatusViaWWAN){
        WWANAccessType accessType = [GLobalRealReachability currentWWANtype];
        if(accessType == WWANType2G ){
            NSLog(@"2G");
            ret = 2;
        }
        else if (accessType == WWANType3G){
            NSLog(@"3G");
            ret = 3;
        }
        else if(accessType == WWANType4G){
            NSLog(@"4G");
            ret = 4;
        }
        else{
            NSLog(@"UnKown WWANAccessType");
        }
    }
    
    NSString* retStr = [NSString stringWithFormat:@"%d",ret ];
    //回调unity
    UnitySendMessage("NetReachAbility", "OnNetStatus",[retStr UTF8String]);
    return ret;

}

//监听函数
-(void) networkChanged:(NSNotification*) notification
{
    RealReachability *reachablility = (RealReachability*) notification.object;
    ReachabilityStatus status = [reachablility currentReachabilityStatus];
    [self handleNetworkChanged:status];
     
}


@end





