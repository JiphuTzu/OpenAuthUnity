using Umawerse.Utils;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    public string appId;
    public string scheme;
    public Text result;

    public void OnLogin()
    {
        appId.AlipayAuth(scheme,authCode=> result.text = authCode);
    }
}
