/* VRGUIHTMLBasicSample

 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using MiddleVR_Unity3D;

/*
 * 
 * Use with data/GUI/HTMLBasicSample/index.html in MiddleVR install directory
 * 
 */
[AddComponentMenu("MiddleVR/Samples/GUI/HTML Basic")]
public class VRGUIHTMLBasicSample : MonoBehaviour
{
    [VRCommand]
    public void MyVRCommand()
    {
        print("HTML Button was clicked");

        CallJavascript();
    }

    protected void Start()
    {
        MVRTools.RegisterCommands(this);
    }

    protected void CallJavascript()
    {
        vrWebView webView = GetComponent<VRWebView>().webView;
        webView.ExecuteJavascript("AddResult('Button was clicked!')");
    }
}
