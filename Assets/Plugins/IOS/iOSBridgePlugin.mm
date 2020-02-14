 #import "iOSBridgePlugin.h"
 #import "RealReachability.h"

 void InitNetReachAbility(){
     NSLog(@"[iOS Native] I AM IOS Function!");
     [[IOSBridge GetInst] InitReachAbility] ;
 }

void UnInitNetReachAbility(){
    [[IOSBridge GetInst] UnInitReachAbility];
}

@implementation IOSBridge

static IOSBridge* _sharedInstance = nil;

+ (IOSBridge*)  GetInst{
    @synchronized (self.class) {
        if(_sharedInstance == nil){
            _sharedInstance = [[self.class alloc] init];
        }
        
        return _sharedInstance;
    }
}

- (void) InitReachAbility
{
    GLobalRealReachability.hostForPing = @"www.baidu.com";
    GLobalRealReachability.hostForCheck = @"www.apple.com";
    [GLobalRealReachability startNotifier];
    [[NSNotificationCenter defaultCenter] addObserver:(self) selector: @selector(networkChanged:) name:kRealReachabilityChangedNotification object:(nil)];
}
- (void) UnInitReachAbility
{
    [[NSNotificationCenter defaultCenter] removeObserver:self];
}

-(void) networkChanged:(NSNotification*) notification
{
    RealReachability *reachablility = (RealReachability*) notification.object;
    ReachabilityStatus status = [reachablility currentReachabilityStatus];
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
        case RealStatusViaWWAN:
            NSLog(@"RealStatusViaWWAN");
        case RealStatusUnknown:
            NSLog(@"RealStatusUnKnown");
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
    //回掉unity
    UnitySendMessage("NetReachAbility", "OnNetStatus",[retStr UTF8String]);

    
}


@end





