using System;
using AOT;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Umawerse.Utils
{
    public static class OpenAuthExtension
    {
        public static void AlipayAuth(this string appId, string scheme, Action<string> callback)
        {
            _onAuth = callback;
            var url = $"https://authweb.alipay.com/auth?auth_type=PURE_OAUTH_SDK&app_id={appId}&scope=auth_user,id_verify&state=init";
            Auth(url, scheme, OnAuthCallback);
        }

        private static Action<string> _onAuth;
        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnAuthCallback(string authCode)
        {
            _onAuth?.Invoke(authCode);
        }
#if UNITY_ANDROID && UNITY_EDITOR
        private static void Auth(string url, string scheme, Action<string> callback)
        {
            var activity = GetJavaObject("currentActivity","com.unity3d.player.UnityPlayer");
            var bizType = GetJavaObject("AccountAuth","com.alipay.sdk.auth.OpenAuthTask$BizType");
            var param = new AndroidJavaObject("java.util.HashMap");
            param.Call<string>("put", "url", url);
            var openAuthTask = new AndroidJavaObject("com.alipay.sdk.auth.OpenAuthTask", activity);
            openAuthTask.Call("execute", scheme, bizType, param, new AuthListener(), true);
        }

        private static AndroidJavaObject GetJavaObject(string property, string className)
        {
            using var javaClass = new AndroidJavaClass(className);
            return javaClass.GetStatic<AndroidJavaObject>(property);
        }

        private class AuthListener : AndroidJavaProxy
        {
            public AuthListener() : base("com.alipay.sdk.auth.OpenAuthTask$AuthCallback"){}

            // ReSharper disable once InconsistentNaming
            public void onResult(int code, string msg, AndroidJavaObject bundle)
            {
                try
                {
                    OnAuthCallback(bundle.Call<string>("getString", "auth_code"));
                }
                catch (Exception)
                {
                    OnAuthCallback("");
                }
            }
        }
#elif UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void Auth(string url, string scheme, Action<string> callback);
        
#else
        private static void Auth(string url, string scheme, Action<string> callback)
        {
            Debug.Log("[OpenAuthUnity]只支持iOS和安卓设备");
            OnAuthCallback("");
        }
#endif
    }
}
