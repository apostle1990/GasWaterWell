﻿#pragma checksum "..\..\MainWindowCopy.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "72E2102544EE140B4C5C6D6064508F30C949B3E00B280619BFDC5EFD7EDF7790"
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
    /// MainWindowCopy
    /// </summary>
    public partial class MainWindowCopy : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 40 "..\..\MainWindowCopy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem CreateProject;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\MainWindowCopy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem OpenProject;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\MainWindowCopy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ToolBar toolBar;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\MainWindowCopy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Connect_Button;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\MainWindowCopy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView proSourceTree;
        
        #line default
        #line hidden
        
        
        #line 171 "..\..\MainWindowCopy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button gasWater;
        
        #line default
        #line hidden
        
        
        #line 183 "..\..\MainWindowCopy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl mainTab;
        
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
            System.Uri resourceLocater = new System.Uri("/GasWaterWell;component/mainwindowcopy.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindowCopy.xaml"
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
            
            #line 12 "..\..\MainWindowCopy.xaml"
            ((GasWaterWell.MainWindowCopy)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 13 "..\..\MainWindowCopy.xaml"
            ((GasWaterWell.MainWindowCopy)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.CreateProject = ((System.Windows.Controls.MenuItem)(target));
            
            #line 40 "..\..\MainWindowCopy.xaml"
            this.CreateProject.Click += new System.Windows.RoutedEventHandler(this.CreateProject_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.OpenProject = ((System.Windows.Controls.MenuItem)(target));
            
            #line 41 "..\..\MainWindowCopy.xaml"
            this.OpenProject.Click += new System.Windows.RoutedEventHandler(this.OpenProject_CLick);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 47 "..\..\MainWindowCopy.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Setting_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.toolBar = ((System.Windows.Controls.ToolBar)(target));
            
            #line 53 "..\..\MainWindowCopy.xaml"
            this.toolBar.Loaded += new System.Windows.RoutedEventHandler(this.ToolBar_Loaded);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Connect_Button = ((System.Windows.Controls.Button)(target));
            
            #line 65 "..\..\MainWindowCopy.xaml"
            this.Connect_Button.Click += new System.Windows.RoutedEventHandler(this.Connect_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.proSourceTree = ((System.Windows.Controls.TreeView)(target));
            return;
            case 9:
            
            #line 150 "..\..\MainWindowCopy.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddGasPVT_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 151 "..\..\MainWindowCopy.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddStonePVT_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 152 "..\..\MainWindowCopy.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddWaterPVT_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.gasWater = ((System.Windows.Controls.Button)(target));
            
            #line 171 "..\..\MainWindowCopy.xaml"
            this.gasWater.Click += new System.Windows.RoutedEventHandler(this.GasWater_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.mainTab = ((System.Windows.Controls.TabControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 8:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.PreviewMouseRightButtonDownEvent;
            
            #line 105 "..\..\MainWindowCopy.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.OnPreviewMouseRightButtonDown);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.Control.PreviewMouseDoubleClickEvent;
            
            #line 107 "..\..\MainWindowCopy.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.OnPreviewMouseDoubleClick);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}
