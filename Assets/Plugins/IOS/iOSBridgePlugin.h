#import "RealReachability.h"

//!---------------------------外部接口声明--------------------------------------------
#if defined(__cplusplus)
extern "C"{
#endif
    extern void UnitySendMessage(const char*, const char*, const char*);
#if defined(__cplusplus)
}
#endif

extern "C" {
     int InitNetReachAbility();
     void UnInitNetReachAbility();
 }

@interface IOSBridge : NSObject
+(IOSBridge*) GetInst;
-(int) InitReachAbility;
-(void) UnInitReachAbility;
-(int) networkChanged:(NSNotification*) notification;
-(int) handleNetworkChanged:(ReachabilityStatus) status;
@end

