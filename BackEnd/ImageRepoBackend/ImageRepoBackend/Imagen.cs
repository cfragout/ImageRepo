//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImageRepoBackend
{
    using System;
    using System.Collections.Generic;
    
    public partial class Imagen
    {
        public int id { get; set; }
        public string name { get; set; }
        public Nullable<bool> userUploaded { get; set; }
        public string originalURL { get; set; }
        public string path { get; set; }
    }
}