﻿#pragma checksum "C:\Users\egart\Source\Repos\Site Manager\XAML Pages\Settings.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1AEBA535600A3BB6D150063FB551D32E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Site_Manager
{
    partial class Settings : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // XAML Pages\Settings.xaml line 1
                {
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)(target);
                    ((global::Windows.UI.Xaml.Controls.Page)element1).Loaded += this.Page_Loaded;
                }
                break;
            case 2: // XAML Pages\Settings.xaml line 47
                {
                    this.BlankDeployCheckBox = (global::Windows.UI.Xaml.Controls.CheckBox)(target);
                    ((global::Windows.UI.Xaml.Controls.CheckBox)this.BlankDeployCheckBox).Checked += this.BlankDeployCheckBox_Checked;
                    ((global::Windows.UI.Xaml.Controls.CheckBox)this.BlankDeployCheckBox).Unchecked += this.BlankDeployCheckBox_Unchecked;
                }
                break;
            case 3: // XAML Pages\Settings.xaml line 48
                {
                    this.UsePreviewDirectoryCheckBox = (global::Windows.UI.Xaml.Controls.CheckBox)(target);
                }
                break;
            case 4: // XAML Pages\Settings.xaml line 39
                {
                    this.SaveButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.SaveButton).Click += this.SaveFTPConfigButton_Click;
                }
                break;
            case 5: // XAML Pages\Settings.xaml line 33
                {
                    this.UsernameTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 6: // XAML Pages\Settings.xaml line 35
                {
                    this.PasswordTextBox = (global::Windows.UI.Xaml.Controls.PasswordBox)(target);
                }
                break;
            case 7: // XAML Pages\Settings.xaml line 37
                {
                    this.ServerTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

