using Aplicacion.AgregarExcel;
using Persistencia.Repository;

namespace WebApi.BackGround
{
    public class ProcessData: IHostedService
    {


        public Task StartAsync(CancellationToken cancellationToken)
        {
            var paramrepository = new ParametroRepository();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<ProcessData>();
        }

    }


}

