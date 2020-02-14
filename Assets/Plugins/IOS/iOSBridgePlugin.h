//!---------------------------外部接口声明--------------------------------------------
#if defined(__cplusplus)
extern "C"{
#endif
    extern void UnitySendMessage(const char*, const char*, const char*);
#if defined(__cplusplus)
}
#endif

extern "C" {
     void InitNetReachAbility();
     void UnInitNetReachAbility();
 }

@interface IOSBridge : NSObject
+(IOSBridge*) GetInst;
-(void) InitReachAbility;
-(void) UnInitReachAbility;
-(void) networkChanged:(NSNotification*) notification;
@end

