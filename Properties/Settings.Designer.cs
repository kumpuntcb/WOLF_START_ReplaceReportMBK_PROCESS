﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WOLF_START_ReplaceReportMBK_PROCESS.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=DESKTOP-SHLJS2M\\SQLEXPRESS2019;Initial Catalog=WolfApproveCore.MBKISO" +
            ";User ID=sa;Password=pass@word1;Connect Timeout=30;Encrypt=False;TrustServerCert" +
            "ificate=False")]
        public string WolfApproveCore_MBKISOConnectionString {
            get {
                return ((string)(this["WolfApproveCore_MBKISOConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=DESKTOP-SHLJS2M\\SQLEXPRESS2019;Initial Catalog=WolfApproveCore.MBKISO" +
            ";Integrated Security=True")]
        public string WolfApproveCore_MBKISOConnectionString1 {
            get {
                return ((string)(this["WolfApproveCore_MBKISOConnectionString1"]));
            }
        }
    }
}
