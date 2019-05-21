/* VRGUIMenuSample

 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using MiddleVR_Unity3D;

[AddComponentMenu("MiddleVR/Samples/GUI/Menu")]
public class VRGUIMenuSample : MonoBehaviour {

    private vrWidgetToggleButton m_Checkbox;

    [VRCommand]
    private void ButtonHandler()
    {
        m_Checkbox.SetChecked( ! m_Checkbox.IsChecked() );
        print("ButtonHandler() called");
    }

    [VRCommand]
    private void CheckboxHandler(bool iValue)
    {
        print("Checkbox value : " + iValue);
    }
    
    [VRCommand]
    private void RadioHandler(string iValue)
    {
        print("Radio value : " + iValue);
    }
    
    [VRCommand]
    private void ColorPickerHandler(vrVec4 iValue)
    {
        print("Selected color : " + iValue.x() + " " + iValue.y() + " " + iValue.z());
    }
    
    [VRCommand]
    private void SliderHandler(float iValue)
    {
        print("Slider value : " + iValue);
    }
    
    private void ListHandler(int iValue)
    {
        print( "List Selected Index : " + iValue );
    }

    private void Start()
    {
        // Automatically register all methods with the [VRCommand] attribute
        MVRTools.RegisterCommands(this);

        // Create GUI

        VRWebView webViewScript = GetComponent<VRWebView>();

        if (webViewScript == null)
        {
            MVRTools.Log(0, "[X] VRGUIMenuSample does not have a WebView.");
            enabled = false;
            return;
        }

        var GUIRendererWeb = new vrGUIRendererWeb("", webViewScript.webView);
        // Register the object so the garbage collector does not collect it after this method.
        // The object will be disposed when the GameObject is destroyed.
        MVRTools.RegisterObject(this, GUIRendererWeb);

        var menu = new vrWidgetMenu("GUIMenuSample.MainMenu", GUIRendererWeb);
        MVRTools.RegisterObject(this, menu);

        var button1 = new vrWidgetButton("GUIMenuSample.Button1", menu, "Button", MVRTools.GetCommand("ButtonHandler"));
        MVRTools.RegisterObject(this, button1);

        var separator = new vrWidgetSeparator("GUIMenuSample.Separator1", menu);
        MVRTools.RegisterObject(this, separator);

        m_Checkbox = new vrWidgetToggleButton("GUIMenuSample.Checkbox", menu, "Toggle Button", MVRTools.GetCommand("CheckboxHandler"), true);
        MVRTools.RegisterObject(this, m_Checkbox);

        var submenu = new vrWidgetMenu("GUIMenuSample.SubMenu", menu, "Sub Menu");
        submenu.SetVisible(true);
        MVRTools.RegisterObject(this, submenu);

        var radio1 = new vrWidgetRadioButton("GUIMenuSample.Radio1", submenu, "Huey", MVRTools.GetCommand("RadioHandler"), "Huey");
        MVRTools.RegisterObject(this, radio1);
        var radio2 = new vrWidgetRadioButton("GUIMenuSample.Radio2", submenu, "Dewey", MVRTools.GetCommand("RadioHandler"), "Dewey");
        MVRTools.RegisterObject(this, radio2);
        var radio3 = new vrWidgetRadioButton("GUIMenuSample.Radio3", submenu, "Louie", MVRTools.GetCommand("RadioHandler"), "Louie");
        MVRTools.RegisterObject(this, radio3);

        var picker = new vrWidgetColorPicker("GUIMenuSample.ColorPicker", menu, "Color Picker", MVRTools.GetCommand("ColorPickerHandler"), new vrVec4(0, 0, 0, 0));
        MVRTools.RegisterObject(this, picker);

        var slider = new vrWidgetSlider("GUIMenuSample.Slider", menu, "Slider", MVRTools.GetCommand("SliderHandler"), 50.0f, 0.0f, 100.0f, 1.0f);
        MVRTools.RegisterObject(this, slider);

        vrValue listContents = vrValue.CreateList();
        listContents.AddListItem( "Item 1" );
        listContents.AddListItem( "Item 2" );

        var list = new vrWidgetList("GUIMenuSample.List", menu, "List", MVRTools.GetCommand("ListHandler"), listContents, 0);
        MVRTools.RegisterObject(this, list);
    }
}
