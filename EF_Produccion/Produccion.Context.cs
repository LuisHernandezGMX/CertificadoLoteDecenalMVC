﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EF_Produccion
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ProduccionEntities : DbContext
    {
        public ProduccionEntities()
            : base("name=ProduccionEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<di_certificado_decenal> di_certificado_decenal { get; set; }
        public virtual DbSet<di_certificado_lote_decenal> di_certificado_lote_decenal { get; set; }
    
        public virtual int spiu_tvarias_ult_nro(string nom_tabla, ObjectParameter ult_nro)
        {
            var nom_tablaParameter = nom_tabla != null ?
                new ObjectParameter("nom_tabla", nom_tabla) :
                new ObjectParameter("nom_tabla", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spiu_tvarias_ult_nro", nom_tablaParameter, ult_nro);
        }
    }
}
