﻿#pragma checksum "..\..\..\Views\SpeechWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F76FA55D3D6CB3329E1E021BD2ECC353"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro.Controls;
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
using TeachingPlatformApp.Views;


namespace TeachingPlatformApp.Views {
    
    
    /// <summary>
    /// SpeechWindow
    /// </summary>
    public partial class SpeechWindow : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\Views\SpeechWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbVoices;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Views\SpeechWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbAudioOut;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\Views\SpeechWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbspeech;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\Views\SpeechWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider tbarRate;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\..\Views\SpeechWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider trbVolume;
        
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
            System.Uri resourceLocater = new System.Uri("/TeachingPlatformApp;component/views/speechwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\SpeechWindow.xaml"
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
            this.cmbVoices = ((System.Windows.Controls.ComboBox)(target));
            
            #line 22 "..\..\..\Views\SpeechWindow.xaml"
            this.cmbVoices.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CmbVoices_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.cmbAudioOut = ((System.Windows.Controls.ComboBox)(target));
            
            #line 29 "..\..\..\Views\SpeechWindow.xaml"
            this.cmbAudioOut.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CmbAudioOut_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 47 "..\..\..\Views\SpeechWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Bt_speek_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 54 "..\..\..\Views\SpeechWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Bt_stop_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.tbspeech = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.tbarRate = ((System.Windows.Controls.Slider)(target));
            
            #line 93 "..\..\..\Views\SpeechWindow.xaml"
            this.tbarRate.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.TbarRate_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.trbVolume = ((System.Windows.Controls.Slider)(target));
            
            #line 112 "..\..\..\Views\SpeechWindow.xaml"
            this.trbVolume.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.TrbVolume_ValueChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

