﻿#pragma checksum "D:\Visual Studio\repo\MovieLibraryApp\MovieLibraryApp\Views\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1BFFB99DFBDF5C5F5A337ACA83766B23"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MovieLibraryApp.Views
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        private static class XamlBindingSetters
        {
        };

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        private class MainPage_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IMainPage_Bindings
        {
            private global::MovieLibraryApp.Views.MainPage dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.GridView obj9;

            public MainPage_obj1_Bindings()
            {
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 9:
                        this.obj9 = (global::Windows.UI.Xaml.Controls.GridView)target;
                        ((global::Windows.UI.Xaml.Controls.GridView)target).Tapped += (global::System.Object sender, global::Windows.UI.Xaml.Input.TappedRoutedEventArgs e) =>
                        {
                        this.dataRoot.ViewModel.GoToMovieDetailsPage();
                        };
                        break;
                    default:
                        break;
                }
            }

            // IMainPage_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
            }

            public bool SetDataRoot(global::System.Object newDataRoot)
            {
                if (newDataRoot != null)
                {
                    this.dataRoot = (global::MovieLibraryApp.Views.MainPage)newDataRoot;
                    return true;
                }
                return false;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::MovieLibraryApp.Views.MainPage obj, int phase)
            {
                if (obj != null)
                {
                }
            }
        }
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2:
                {
                    this.ViewModel = (global::MovieLibraryApp.ViewModels.MainPageViewModel)(target);
                }
                break;
            case 3:
                {
                    this.BannerImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 4:
                {
                    this.BannerTitle = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5:
                {
                    this.SearchInput = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 44 "..\..\..\Views\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.SearchInput).KeyUp += this.SearchInput_OnKeyUp;
                    #line default
                }
                break;
            case 6:
                {
                    this.SearchIcon = (global::Windows.UI.Xaml.Controls.SymbolIcon)(target);
                    #line 53 "..\..\..\Views\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.SymbolIcon)this.SearchIcon).Tapped += this.SearchIcon_OnTapped;
                    #line default
                }
                break;
            case 7:
                {
                    this.EmptyList = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8:
                {
                    this.LoadingIndicator = (global::Windows.UI.Xaml.Controls.ProgressRing)(target);
                }
                break;
            case 9:
                {
                    this.MainGrid = (global::Windows.UI.Xaml.Controls.GridView)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1:
                {
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)target;
                    MainPage_obj1_Bindings bindings = new MainPage_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                }
                break;
            }
            return returnValue;
        }
    }
}
