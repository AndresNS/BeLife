﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Aplicacion.Datos
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BeLifeEntities : DbContext
    {
        public BeLifeEntities()
            : base("name=BeLifeEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Contrato> Contratoes { get; set; }
        public virtual DbSet<EstadoCivil> EstadoCivils { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<Sexo> Sexoes { get; set; }
    }
}
