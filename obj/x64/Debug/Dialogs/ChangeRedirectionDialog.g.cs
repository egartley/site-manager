﻿#pragma checksum "C:\Users\egart\Source\Repos\Site Manager\Dialogs\ChangeRedirectionDialog.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B41842983ECB54A07ED672E54906DB66"
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
    partial class ChangeRedirectionDialog : 
        global::Windows.UI.Xaml.Controls.ContentDialog, 
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
            case 1: // Dialogs\ChangeRedirectionDialog.xaml line 12
                {
                    this.DestinationTextBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 2: // Dialogs\ChangeRedirectionDialog.xaml line 15
                {
                    this.RenameTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.RenameTextBox).TextChanged += this.RenameTextBox_TextChanged;
                }
                break;
            case 3: // Dialogs\ChangeRedirectionDialog.xaml line 18
                {
                    this.DeleteRedirectionCheckBox = (global::Windows.UI.Xaml.Controls.CheckBox)(target);
                    ((global::Windows.UI.Xaml.Controls.CheckBox)this.DeleteRedirectionCheckBox).Click += this.DeleteRedirectionCheckBox_Click;
                }
                break;
            case 4: // Dialogs\ChangeRedirectionDialog.xaml line 20
                {
                    this.ConfirmationCheckBox = (global::Windows.UI.Xaml.Controls.CheckBox)(target);
                    ((global::Windows.UI.Xaml.Controls.CheckBox)this.ConfirmationCheckBox).Click += this.ConfirmationCheckBox_Click;
                }
                break;
            case 5: // Dialogs\ChangeRedirectionDialog.xaml line 22
                {
                    this.ApplyButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.ApplyButton).Click += this.ApplyButton_Click;
                }
                break;
            case 6: // Dialogs\ChangeRedirectionDialog.xaml line 23
                {
                    this.WorkingStackPanel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 7: // Dialogs\ChangeRedirectionDialog.xaml line 27
                {
                    this.OkButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.OkButton).Click += this.OkButton_Click;
                }
                break;
            case 8: // Dialogs\ChangeRedirectionDialog.xaml line 24
                {
                    this.WorkingProgressRing = (global::Windows.UI.Xaml.Controls.ProgressRing)(target);
                }
                break;
            case 9: // Dialogs\ChangeRedirectionDialog.xaml line 25
                {
                    this.WorkingTextBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
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

