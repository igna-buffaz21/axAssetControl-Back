using axAssetControl.AccesoDatos;
using axAssetControl.Entidades;
using axAssetControl.Negocio;
using axAssetControl.Negocio.Seguridad;

namespace axAssetControl.Extensiones
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AgregarServiciosPersonalizados(this IServiceCollection services)
        {
            services.AddScoped<UsuarioAD>();
            services.AddScoped<UsuarioN>();
            services.AddScoped<EmpresaAD>();
            services.AddScoped<EmpresaN>();
            services.AddScoped<LocacionAD>();
            services.AddScoped<LocacionN>();
            services.AddScoped<SectorAD>();
            services.AddScoped<SectorN>();
            services.AddScoped<SubSectorAD>();
            services.AddScoped<SubSectorN>();
            services.AddScoped<ActivoN>();
            services.AddScoped<ActivosAD>();
            services.AddScoped<TipoActivoAD>();
            services.AddScoped<TipoActivoN>();
            services.AddScoped<RegistroControlAD>();
            services.AddScoped<RegistroControlN>();
            services.AddScoped<DetalleControlAD>();
            services.AddScoped<DetalleControlN>();
            services.AddScoped<AuthAD>();
            services.AddScoped<JwtN>();
            services.AddScoped<AuthN>();

            return services;
        }
    }
}
