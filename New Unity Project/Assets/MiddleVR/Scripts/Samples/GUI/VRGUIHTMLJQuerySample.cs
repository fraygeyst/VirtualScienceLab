/* VRGUIHTMLJQuerySample

 * MiddleVR
 * (c) MiddleVR
 */

/*
 * 
 * Use with data/GUI/HTMLJQuerySample/index.html in MiddleVR install directory
 * 
 */

using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Samples/GUI/HTML JQuery")]
public class VRGUIHTMLJQuerySample : MonoBehaviour
{
	private int m_Progress = 0;

    [VRCommand("ButtonCommand")]
	private void ButtonHandler(vrValue iValue)
	{
		m_Progress += 1;

        var vrWebViewScript = gameObject.GetComponent<VRWebView>();
        if (vrWebViewScript != null)
        {
            Debug.Log("Progressbar incremented!");
            var webView = vrWebViewScript.webView;
            webView.ExecuteJavascript(
                "$('#progressbar').progressbar('value'," + m_Progress.ToString() + ");");
        }
	}

    [VRCommand("RadioCommand")]
	private void RadioHandler(vrValue iValue)
	{
		if(iValue.IsString())
		{
			Debug.Log("Radio value = " + iValue.GetString() );
		}
	}

    [VRCommand("SliderCommand")]
	private void SliderHandler(vrValue iValue)
	{
		if (iValue.IsNumber())
		{
			Debug.Log("Slider value as Number = " + iValue.GetNumber() );
		}
	}
	
	private void Start()
	{
        MVRTools.RegisterCommands(this);
	}
}
