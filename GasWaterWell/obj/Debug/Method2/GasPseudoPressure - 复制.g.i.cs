﻿#pragma checksum "..\..\..\Method2\GasPseudoPressure - 复制.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "32008E8E52115FA264022E2B347342F5AFB74C0FE2F9F3803073844D56ED08F3"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using GasWaterWell.Method2;
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


namespace GasWaterWell.Method2 {
    
    
    /// <summary>
    /// GasPseudoPressure
    /// </summary>
    public partial class GasPseudoPressure : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\Method2\GasPseudoPressure - 复制.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid OutputDataGrid;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Method2\GasPseudoPressure - 复制.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Output_Button;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Method2\GasPseudoPressure - 复制.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Output_ComboBox;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Method2\GasPseudoPressure - 复制.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Save_Button;
        
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
            System.Uri resourceLocater = new System.Uri("/GasWaterWell;component/method2/gaspseudopressure%20-%20%e5%a4%8d%e5%88%b6.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Method2\GasPseudoPressure - 复制.xaml"
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
            
            #line 12 "..\..\..\Method2\GasPseudoPressure - 复制.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Product_Button);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 13 "..\..\..\Method2\GasPseudoPressure - 复制.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Cal_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.OutputDataGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 18 "..\..\..\Method2\GasPseudoPressure - 复制.xaml"
            this.OutputDataGrid.AutoGeneratingColumn += new System.EventHandler<System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs>(this.DataGrid_AutoGeneratingColumn);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Output_Button = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\Method2\GasPseudoPressure - 复制.xaml"
            this.Output_Button.Click += new System.Windows.RoutedEventHandler(this.Output_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Output_ComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.Save_Button = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\Method2\GasPseudoPressure - 复制.xaml"
            this.Save_Button.Click += new System.Windows.RoutedEventHandler(this.Save_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 26 "..\..\..\Method2\GasPseudoPressure - 复制.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Cancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

