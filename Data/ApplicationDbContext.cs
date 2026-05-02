using GradSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GradSystem.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Registro>              Registros             => Set<Registro>();
    public DbSet<ColorJersey>           ColoresJersey         => Set<ColorJersey>();
    public DbSet<UniversidadCatalogo>   Universidades         => Set<UniversidadCatalogo>();
    public DbSet<TallaCatalogo>         Tallas                => Set<TallaCatalogo>();
    public DbSet<DedicatoriaCatalogo>   Dedicatorias          => Set<DedicatoriaCatalogo>();
    public DbSet<TituloCatalogo>        Titulos               => Set<TituloCatalogo>();
    public DbSet<ConfiguracionSistema>  ConfiguracionSistema  => Set<ConfiguracionSistema>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Registro>(e =>
        {
            e.HasIndex(r => r.Folio).IsUnique();
            e.Property(r => r.FechaEnvio).HasDefaultValueSql("NOW()");
        });

        builder.Entity<ColorJersey>().HasData(
            new ColorJersey { Id = 1, Nombre = "Negro",  Activo = true, Orden = 1 },
            new ColorJersey { Id = 2, Nombre = "Blanco", Activo = true, Orden = 2 },
            new ColorJersey { Id = 3, Nombre = "Azul",   Activo = true, Orden = 3 },
            new ColorJersey { Id = 4, Nombre = "Rojo",   Activo = true, Orden = 4 },
            new ColorJersey { Id = 5, Nombre = "Verde",  Activo = true, Orden = 5 },
            new ColorJersey { Id = 6, Nombre = "Gris",   Activo = true, Orden = 6 }
        );

        builder.Entity<UniversidadCatalogo>().HasData(
            new UniversidadCatalogo { Id = 1, Nombre = "UANL Universidad Autonoma de Nuevo Leon", Siglas = "UANL", Activo = true, Orden = 1 },
            new UniversidadCatalogo { Id = 2, Nombre = "UIN Universidad Interamericana del Norte",  Siglas = "UIN",  Activo = true, Orden = 2 }
        );

        builder.Entity<TallaCatalogo>().HasData(
            new TallaCatalogo { Id = 1, Nombre = "XS",  Activo = true, Orden = 1 },
            new TallaCatalogo { Id = 2, Nombre = "S",   Activo = true, Orden = 2 },
            new TallaCatalogo { Id = 3, Nombre = "M",   Activo = true, Orden = 3 },
            new TallaCatalogo { Id = 4, Nombre = "L",   Activo = true, Orden = 4 },
            new TallaCatalogo { Id = 5, Nombre = "XL",  Activo = true, Orden = 5 },
            new TallaCatalogo { Id = 6, Nombre = "XXL", Activo = true, Orden = 6 }
        );

        builder.Entity<TituloCatalogo>().HasData(
            new TituloCatalogo { Id = 1, Nombre = "Lic",  Activo = true, Orden = 1 },
            new TituloCatalogo { Id = 2, Nombre = "Ing",  Activo = true, Orden = 2 },
            new TituloCatalogo { Id = 3, Nombre = "Dr",   Activo = true, Orden = 3 },
            new TituloCatalogo { Id = 4, Nombre = "Dra",  Activo = true, Orden = 4 }
        );

        builder.Entity<DedicatoriaCatalogo>().HasData(
            new DedicatoriaCatalogo { Id = 1, Nombre = "A mis padres",   Activo = true, Orden = 1 },
            new DedicatoriaCatalogo { Id = 2, Nombre = "A mi madre",     Activo = true, Orden = 2 },
            new DedicatoriaCatalogo { Id = 3, Nombre = "A mi esposa",    Activo = true, Orden = 3 },
            new DedicatoriaCatalogo { Id = 4, Nombre = "A mi esposo",    Activo = true, Orden = 4 },
            new DedicatoriaCatalogo { Id = 5, Nombre = "A mis hijos",    Activo = true, Orden = 5 },
            new DedicatoriaCatalogo { Id = 6, Nombre = "A mi familia",   Activo = true, Orden = 6 }
        );

        builder.Entity<ConfiguracionSistema>().HasData(
            new ConfiguracionSistema { Id = 1, RegistroHabilitado = true }
        );
    }
}
