#import <AFServiceSDK/AFServiceSDK.h>
#import <UnityAppController.h>
#import <Foundation/Foundation.h>

#if defined(__cplusplus)
extern "C" {
#endif

typedef void (*auth_callback)(const char*);

#pragma mark ==============点击支付行为==============
void Auth(const char *appId, const char *scheme, auth_callback callback) {
    // 登陆授权或别的需要跳转到支付宝完成操作的Url
    NSString *urlParams = [NSString stringWithFormat:@"https://authweb.alipay.com/auth?auth_type=PURE_OAUTH_SDK&app_id=%s&scope=auth_user,id_verify&state=init", appId];
    NSDictionary *params = @{
        kAFServiceOptionBizParams : @{@"url" : urlParams},
        kAFServiceOptionCallbackScheme : [NSString stringWithFormat:@"%s", scheme],
    };
    [AFServiceCenter callService:AFServiceAuth withParams:params andCompletion:^(AFAuthServiceResponse *response) {
          NSString *authCode = response.result[@"auth_code"];
          callback([authCode UTF8String]);
    }];
}
#if defined(__cplusplus)
}
#endif

@implementation UnityAppController (Alipay)
// NOTE: 9.0以后使用新API接口
- (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<NSString*, id> *)options
{
    if ([url.host isEqualToString:@"apmqpdispatch"]) {
        [AFServiceCenter handleResponseURL:url withCompletion:^(AFAuthServiceResponse *response) { 
            if (AFAuthResSuccess == response.responseCode) {
                NSLog(@"%@", response.result);
            }
        }];
    }
    return YES;
}
@end
