﻿#pragma checksum "..\..\..\Method2\PressureSquare.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "85823A366E715C3BDF66C35B3D734BF8F55A6AAFCE36176B73AB899C10222F18"
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
    /// PressureSquare
    /// </summary>
    public partial class PressureSquare : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\Method2\PressureSquare.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Product_Button;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\Method2\PressureSquare.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Cal_Button;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\Method2\PressureSquare.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid OutputDataGrid;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Method2\PressureSquare.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Output_Button;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Method2\PressureSquare.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Output_ComboBox;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Method2\PressureSquare.xaml"
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
            System.Uri resourceLocater = new System.Uri("/GasWaterWell;component/method2/pressuresquare.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Method2\PressureSquare.xaml"
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
            this.Product_Button = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\..\Method2\PressureSquare.xaml"
            this.Product_Button.Click += new System.Windows.RoutedEventHandler(this.Product_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Cal_Button = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\..\Method2\PressureSquare.xaml"
            this.Cal_Button.Click += new System.Windows.RoutedEventHandler(this.Cal_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.OutputDataGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 18 "..\..\..\Method2\PressureSquare.xaml"
            this.OutputDataGrid.AutoGeneratingColumn += new System.EventHandler<System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs>(this.DataGrid_AutoGeneratingColumn);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Output_Button = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\Method2\PressureSquare.xaml"
            this.Output_Button.Click += new System.Windows.RoutedEventHandler(this.Output_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Output_ComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.Save_Button = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\Method2\PressureSquare.xaml"
            this.Save_Button.Click += new System.Windows.RoutedEventHandler(this.Save_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 26 "..\..\..\Method2\PressureSquare.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Cancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

