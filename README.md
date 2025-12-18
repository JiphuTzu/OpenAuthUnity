# open_auth_unity
支付宝登录极简版Unity接入，支持安卓和iOS。

## 使用方法

支付宝相关账号和服务申请，请参照支付宝[接入准备](https://opendocs.alipay.com/open/218/105326?pathHash=5bece2f8)。

以下主要介绍在Unity中的使用。

下载项目，把Assets/Plugins/OpenAuthUnity拷贝到项目中。

### 安卓配置

在`PlayerSettings->->PublishSettings`中勾选`Custom Main Manifst`，会自动在`Assets/Plugins/Android`中生成`AndroidManifest.xml`。打开该文件，添加以下代码：
```xml
        <!-- 为了使用 "通用跳转 SDK" 的能力，需要在您的 App 的 AndroidManifest.xml 中添加这一项 -->
        <!-- 并合理设置 android:scheme 的值 -->
        <activity
            android:name="com.alipay.sdk.app.AlipayResultActivity"
            android:exported="true"
            tools:node="merge" >
            <intent-filter tools:node="replace" >
                <action android:name="android.intent.action.VIEW" />

                <category android:name="android.intent.category.DEFAULT" />
                <category android:name="android.intent.category.BROWSABLE" />

                <data android:scheme="custom_scheme" />
            </intent-filter>
        </activity>
```
在**custom_scheme**处，设置一个比较独特的名字，不要与其他应用冲突，后面代码中也要用到。

### iOS配置

在`PlayerSettings->OtherSettings`中找到`Supported URL schemes`，添加一项`custom_scheme`，与安卓设置相同。

### C#调用

参照`Example.cs`

```csharp
var appId = "支付宝中申请的appId";
var scheme = "custom_scheme";   //上面自定义的scheme
appId.AlipayAuth(scheme, authCode =>
{
    if(string.IsNullOrEmpty(authCode))
        Debug.Log("授权错误，请重新进行操作");
    else
        Debug.Log("授权成功，根据authCode进行后续操作");
});
```
