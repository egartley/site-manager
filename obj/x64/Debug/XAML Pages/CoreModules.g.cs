﻿#pragma checksum "C:\Users\egart\Source\Repos\Site Manager\XAML Pages\CoreModules.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D64D374E331C79F4A66F58E455CD03F6"
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
    partial class CoreModules : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // XAML Pages\CoreModules.xaml line 1
                {
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)(target);
                    ((global::Windows.UI.Xaml.Controls.Page)element1).Loaded += this.Page_Loaded;
                }
                break;
            case 2: // XAML Pages\CoreModules.xaml line 68
                {
                    this.HeaderTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 3: // XAML Pages\CoreModules.xaml line 82
                {
                    this.NavBarTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 4: // XAML Pages\CoreModules.xaml line 96
                {
                    this.SidebarTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 5: // XAML Pages\CoreModules.xaml line 110
                {
                    this.FooterTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 6: // XAML Pages\CoreModules.xaml line 107
                {
                    global::Windows.UI.Xaml.Controls.Button element6 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element6).Click += this.SaveButton_Click;
                }
                break;
            case 7: // XAML Pages\CoreModules.xaml line 108
                {
                    this.FooterChevron = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.FooterChevron).Click += this.Chevron_Click;
                }
                break;
            case 8: // XAML Pages\CoreModules.xaml line 93
                {
                    global::Windows.UI.Xaml.Controls.Button element8 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element8).Click += this.SaveButton_Click;
                }
                break;
            case 9: // XAML Pages\CoreModules.xaml line 94
                {
                    this.SidebarChevron = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.SidebarChevron).Click += this.Chevron_Click;
                }
                break;
            case 10: // XAML Pages\CoreModules.xaml line 79
                {
                    global::Windows.UI.Xaml.Controls.Button element10 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element10).Click += this.SaveButton_Click;
                }
                break;
            case 11: // XAML Pages\CoreModules.xaml line 80
                {
                    this.NavbarChevron = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.NavbarChevron).Click += this.Chevron_Click;
                }
                break;
            case 12: // XAML Pages\CoreModules.xaml line 65
                {
                    global::Windows.UI.Xaml.Controls.Button element12 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element12).Click += this.SaveButton_Click;
                }
                break;
            case 13: // XAML Pages\CoreModules.xaml line 66
                {
                    this.HeaderChevron = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.HeaderChevron).Click += this.Chevron_Click;
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
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
