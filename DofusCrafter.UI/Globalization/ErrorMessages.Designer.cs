﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DofusCrafter.UI.Globalization {
    using System;
    
    
    /// <summary>
    ///   Une classe de ressource fortement typée destinée, entre autres, à la consultation des chaînes localisées.
    /// </summary>
    // Cette classe a été générée automatiquement par la classe StronglyTypedResourceBuilder
    // à l'aide d'un outil, tel que ResGen ou Visual Studio.
    // Pour ajouter ou supprimer un membre, modifiez votre fichier .ResX, puis réexécutez ResGen
    // avec l'option /str ou régénérez votre projet VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ErrorMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorMessages() {
        }
        
        /// <summary>
        ///   Retourne l'instance ResourceManager mise en cache utilisée par cette classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DofusCrafter.UI.Globalization.ErrorMessages", typeof(ErrorMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Remplace la propriété CurrentUICulture du thread actuel pour toutes
        ///   les recherches de ressources à l'aide de cette classe de ressource fortement typée.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à There is no content here yet.
        /// </summary>
        public static string NoContent {
            get {
                return ResourceManager.GetString("NoContent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à .
        /// </summary>
        public static string RegisterIngredientWarning {
            get {
                return ResourceManager.GetString("RegisterIngredientWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à .
        /// </summary>
        public static string SoldConfectionInformation {
            get {
                return ResourceManager.GetString("SoldConfectionInformation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à .
        /// </summary>
        public static string SoldItemDateTimeInformation {
            get {
                return ResourceManager.GetString("SoldItemDateTimeInformation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The error message in english but I want it in french.
        /// </summary>
        public static string SoldItemNameIsRequired {
            get {
                return ResourceManager.GetString("SoldItemNameIsRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à .
        /// </summary>
        public static string SoldItemWarning {
            get {
                return ResourceManager.GetString("SoldItemWarning", resourceCulture);
            }
        }
    }
}
