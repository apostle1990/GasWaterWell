﻿#pragma checksum "..\..\Fitting.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "8D7EAE2AAE02BF3B4914BE5FB9B47B66750FA4A332F7620CC05E1319B3233D07"
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
    /// Fitting
    /// </summary>
    public partial class Fitting : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\Fitting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Pr_TextBox;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\Fitting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid InputDataGrid;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\Fitting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox A_TextBox;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\Fitting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox B_TextBox;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\Fitting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid OutputDataGrid;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\Fitting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Cal_Button;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\Fitting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Save_Button;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\Fitting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Next_Button;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\Fitting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Cancel_Button;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\Fitting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Output_Button;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\Fitting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Output_ComboBox;
        
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
            System.Uri resourceLocater = new System.Uri("/GasWaterWell;component/fitting.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Fitting.xaml"
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
            this.Pr_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            
            #line 24 "..\..\Fitting.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Import_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.InputDataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 4:
            this.A_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.B_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.OutputDataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 7:
            this.Cal_Button = ((System.Windows.Controls.Button)(target));
            
            #line 43 "..\..\Fitting.xaml"
            this.Cal_Button.Click += new System.Windows.RoutedEventHandler(this.Cal_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.Save_Button = ((System.Windows.Controls.Button)(target));
            
            #line 44 "..\..\Fitting.xaml"
            this.Save_Button.Click += new System.Windows.RoutedEventHandler(this.Save_Button_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.Next_Button = ((System.Windows.Controls.Button)(target));
            
            #line 45 "..\..\Fitting.xaml"
            this.Next_Button.Click += new System.Windows.RoutedEventHandler(this.Next_Button_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.Cancel_Button = ((System.Windows.Controls.Button)(target));
            
            #line 46 "..\..\Fitting.xaml"
            this.Cancel_Button.Click += new System.Windows.RoutedEventHandler(this.Cancel_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.Output_Button = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\Fitting.xaml"
            this.Output_Button.Click += new System.Windows.RoutedEventHandler(this.Output_Button_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.Output_ComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
