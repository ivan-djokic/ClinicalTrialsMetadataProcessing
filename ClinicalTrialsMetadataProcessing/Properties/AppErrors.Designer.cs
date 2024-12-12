﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClinicalTrialsMetadataProcessing.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class AppErrors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AppErrors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ClinicalTrialsMetadataProcessing.Properties.AppErrors", typeof(AppErrors).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Clinical trial metadata validation failed: {0}.
        /// </summary>
        internal static string ClinicalTrialMetadataValidationFailed {
            get {
                return ResourceManager.GetString("ClinicalTrialMetadataValidationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Status is Completed, but EndDate in the future.
        /// </summary>
        internal static string CompletedStatusError {
            get {
                return ResourceManager.GetString("CompletedStatusError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Db migration failed: {0}.
        /// </summary>
        internal static string DbMigrationFailed {
            get {
                return ResourceManager.GetString("DbMigrationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File is empty.
        /// </summary>
        internal static string EmptyFile {
            get {
                return ResourceManager.GetString("EmptyFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EndDate cannot be less than StartDate.
        /// </summary>
        internal static string EndDateLessThanStart {
            get {
                return ResourceManager.GetString("EndDateLessThanStart", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No file uploaded.
        /// </summary>
        internal static string File {
            get {
                return ResourceManager.GetString("File", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only *.json files are allowed.
        /// </summary>
        internal static string FileExtensionNotAllowed {
            get {
                return ResourceManager.GetString("FileExtensionNotAllowed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File size exceeds the limit of {0} bytes.
        /// </summary>
        internal static string FileSizeExceededLimit {
            get {
                return ResourceManager.GetString("FileSizeExceededLimit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Service internal error: {0}.
        /// </summary>
        internal static string InternalError {
            get {
                return ResourceManager.GetString("InternalError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Json deserialization failed: {0}.
        /// </summary>
        internal static string JsonDeserializationFailed {
            get {
                return ResourceManager.GetString("JsonDeserializationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Json validation failed: {0}.
        /// </summary>
        internal static string JsonValidationFailed {
            get {
                return ResourceManager.GetString("JsonValidationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Status is NotStarted, but StartDate in the future.
        /// </summary>
        internal static string NotStartedStatusError {
            get {
                return ResourceManager.GetString("NotStartedStatusError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Status is Ongoing, but StartDate in the future or EndDate has passed.
        /// </summary>
        internal static string OngoingStatusError {
            get {
                return ResourceManager.GetString("OngoingStatusError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Updating already added items not allowed. TrialId: {0}.
        /// </summary>
        internal static string UpdateNotAllowed {
            get {
                return ResourceManager.GetString("UpdateNotAllowed", resourceCulture);
            }
        }
    }
}