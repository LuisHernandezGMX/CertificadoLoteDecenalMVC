﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EF_UAT
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class UATEntities : DbContext
    {
        public UATEntities()
            : base("name=UATEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<di_certificado_decenal> di_certificado_decenal { get; set; }
        public virtual DbSet<di_certificado_lote_decenal> di_certificado_lote_decenal { get; set; }
    
        public virtual int spiu_tvarias_ult_nro(Nullable<decimal> sn_muestra, string nom_tabla, ObjectParameter ult_nro)
        {
            var sn_muestraParameter = sn_muestra.HasValue ?
                new ObjectParameter("sn_muestra", sn_muestra) :
                new ObjectParameter("sn_muestra", typeof(decimal));
    
            var nom_tablaParameter = nom_tabla != null ?
                new ObjectParameter("nom_tabla", nom_tabla) :
                new ObjectParameter("nom_tabla", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spiu_tvarias_ult_nro", sn_muestraParameter, nom_tablaParameter, ult_nro);
        }
    }
}
