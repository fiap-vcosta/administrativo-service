using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ClienteAggregateRoot> Clientes { get; set; }
    public DbSet<VeiculoAggregateRoot> Veiculos { get; set; }
    public DbSet<ServicoAggregateRoot> Servicos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var clientes = modelBuilder.Entity<ClienteAggregateRoot>();
        clientes.HasKey(c => c.Id);
        clientes.HasIndex(c => c.TipoDocumento);
        clientes.Property(c => c.TipoDocumento).HasConversion<string>().IsRequired();
        clientes.HasIndex(c => c.Documento).IsUnique();
        clientes.Property(c => c.Documento)
            .HasConversion(
                documento => documento.Valor,
                valor => Documento.Criar(valor))
            .IsRequired();
        clientes.Property(c => c.Nome).IsRequired();
        clientes.Property(c => c.Email).IsRequired();


        var veiculos = modelBuilder.Entity<VeiculoAggregateRoot>();
        veiculos.HasKey(v => v.Id);
        veiculos.HasIndex(v => v.Placa).IsUnique();
        veiculos.Property(v => v.Placa).IsRequired();
        veiculos.Property(v => v.Modelo).IsRequired();
        veiculos.Property(v => v.Marca).IsRequired();
        veiculos
            .HasOne<ClienteAggregateRoot>()
            .WithMany()
            .HasForeignKey(v => v.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);

        var servicos = modelBuilder.Entity<ServicoAggregateRoot>();
        servicos.HasKey(s => s.Id);
        servicos.HasIndex(s => s.Codigo).IsUnique();
        servicos.Property(s => s.Codigo).IsRequired();
        servicos.Property(s => s.Nome).IsRequired();
        servicos.Property(s => s.PrecoPadrao).HasPrecision(10, 2).IsRequired();
    }
}
