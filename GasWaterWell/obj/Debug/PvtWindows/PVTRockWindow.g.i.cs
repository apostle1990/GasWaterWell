﻿#pragma checksum "..\..\..\PvtWindows\PVTRockWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "71C49DF370240E669A7A55FD4661B6D80D7B25D30554AD5C35B1698ED036C407"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using GasWaterWell;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace GasWaterWell {
    
    
    /// <summary>
    /// PVTStone
    /// </summary>
    public partial class PVTStone : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\PvtWindows\PVTRockWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox rockName;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\PvtWindows\PVTRockWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox rock;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\PvtWindows\PVTRockWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox rock_gama;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\PvtWindows\PVTRockWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox rock_pi;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\PvtWindows\PVTRockWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button calc_btn;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\PvtWindows\PVTRockWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Output_Button;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\PvtWindows\PVTRockWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox mycombox;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\PvtWindows\PVTRockWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Save_Button;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\PvtWindows\PVTRockWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dataGrid;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GasWaterWell;component/pvtwindows/pvtrockwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\PvtWindows\PVTRockWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.rockName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.rock = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.rock_gama = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.rock_pi = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.calc_btn = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\..\PvtWindows\PVTRockWindow.xaml"
            this.calc_btn.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Output_Button = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\PvtWindows\PVTRockWindow.xaml"
            this.Output_Button.Click += new System.Windows.RoutedEventHandler(this.Button_Click_export);
            
            #line default
            #line hidden
            return;
            case 7:
            this.mycombox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 8:
            this.Save_Button = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\..\PvtWindows\PVTRockWindow.xaml"
            this.Save_Button.Click += new System.Windows.RoutedEventHandler(this.Button_Click_save);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 41 "..\..\..\PvtWindows\PVTRockWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_cancel);
            
            #line default
            #line hidden
            return;
            case 10:
            this.dataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

