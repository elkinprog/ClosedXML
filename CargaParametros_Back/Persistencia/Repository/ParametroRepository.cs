using Persistencia.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Dominio.Request;
using Dominio.Models;
using Dominio.Mapper;

namespace Persistencia.Repository
{
    public class ParametroRepository : GenericRepository<Parametro>
    {
        public IGenericRepository<ParametroDetalle> _parametroDetalleRepository { get; }
        public  ApplicationDbContext _context;

        public ParametroRepository(ApplicationDbContext context, IGenericRepository<ParametroDetalle> parametroDetalleRepository) : base(context)
        {
            this._parametroDetalleRepository = parametroDetalleRepository;
            this._context = context;
        }

        public async Task<Boolean> ValidarExisteParametro(string codigoInterno)
        {
            return _context.Parametro.Any(p => p.VcCodigoInterno == codigoInterno);
        }

        public void insertMassiveData(ParametroRequest parametroRequest)
        {
            //insert to db           
            using (var connection = _context.Database.GetDbConnection())
            {
                connection.Open();
                SqlCommand command = (SqlCommand)connection.CreateCommand();

                using (SqlTransaction transaction = (SqlTransaction)connection.BeginTransaction())
                {
                    command.Transaction = transaction;
                    /*
                     * Cuando se le indica: SqlBulkCopyOptions.KeepIdentity, realiza internamente el  SET IDENTITY_INSERT Parametro ON
                     * Solamente se puede tener una tablacon SET IDENTITY_INSERT en  ON a la vez
                     *
                     * El comando para ejecutar un comando se realiza por medio de SQLCOMMAND
                     * command.CommandText =
                     *  "SET IDENTITY_INSERT ParametroDetalle OFF";
                     *
                     *  command.ExecuteNonQuery();
                    */
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.KeepIdentity, transaction))
                    {
                        try
                        {
                            bulkCopy.DestinationTableName = "Parametro";
                            bulkCopy.WriteToServer(parametroRequest.Parametros);
                      
                            bulkCopy.DestinationTableName = "ParametroDetalle";
                            bulkCopy.WriteToServer(parametroRequest.ParametroDetalles);

                            transaction.Commit();
                        }
                        catch (Exception ex )
                        {
                            transaction.Rollback();
                            connection.Close();
                            throw;
                        }

                    }
                }
            }

        }
        public IEnumerable<ParametroDetalleMapper> getParametroPorCodigoInterno(string codigoIterno)
        {            
            var parametro = _context.Parametro
                    .Where(p => p.BEstado == true && p.VcCodigoInterno == codigoIterno)
                    .Select(d=> new ParametroMapper
                    {
                        Id = d.Id,
                        VcNombre = d.VcNombre,
                        VcCodigoInterno = d.VcCodigoInterno,
                        ParametroDetalles = d.ParametroDetalles.
                            Select(d=>new ParametroDetalleMapper { 
                                Id=d.Id,
                                ParametroId = d.ParametroId, 
                                VcNombre = d.VcNombre, 
                                TxDescripcion = d.TxDescripcion, 
                                IdPadre = d.IdPadre,
                                VcCodigoInterno = d.VcCodigoInterno, 
                                DCodigoIterno = d.DCodigoIterno, 
                                BEstado = d.BEstado, 
                                RangoDesde = d.RangoDesde, 
                                RangoHasta = d.RangoHasta }).ToList()
                    })
                    .FirstOrDefault();
            
            return parametro.ParametroDetalles;

        }

        public IEnumerable<ParametroDetalleMapper> getParametroPorId(long Id)
        {
            var parametro = _context.Parametro
                    .Where(p => p.Id == Id)
                    .Select(d => new ParametroMapper
                    {
                        Id = d.Id,
                        VcNombre = d.VcNombre,
                        VcCodigoInterno = d.VcCodigoInterno,
                        ParametroDetalles = d.ParametroDetalles.
                            Select(d => new ParametroDetalleMapper
                            {
                                Id = d.Id,
                                ParametroId = d.ParametroId,
                                VcNombre = d.VcNombre,
                                TxDescripcion = d.TxDescripcion,
                                IdPadre = d.IdPadre,
                                VcCodigoInterno = d.VcCodigoInterno,
                                DCodigoIterno = d.DCodigoIterno,
                                BEstado = d.BEstado,
                                RangoDesde = d.RangoDesde,
                                RangoHasta = d.RangoHasta
                            }).ToList()
                    })
                    .FirstOrDefault();

            return parametro.ParametroDetalles;

        }


        public IEnumerable<ParametroDetalleMapper> getParametroDetallePorCodigoInterno(string codigoIterno, string codigoInternoDetalle)
        {
            var parametro = _context.Parametro
                    .Where(p => p.BEstado == true && p.VcCodigoInterno == codigoIterno)
                    .FirstOrDefault();

            IEnumerable<ParametroDetalleMapper> parametroDetalle = Array.Empty<ParametroDetalleMapper>();

            if (parametro != null)
            {
                 parametroDetalle = _context.ParametroDetalle
                    .Where(p => p.BEstado == true && p.VcCodigoInterno == codigoInternoDetalle && p.ParametroId == parametro.Id)
                    .Select(d => new ParametroDetalleMapper
                    {
                        Id = d.Id,
                        ParametroId = d.ParametroId,
                        VcNombre = d.VcNombre,
                        TxDescripcion = d.TxDescripcion,
                        IdPadre = d.IdPadre,
                        VcCodigoInterno = d.VcCodigoInterno,
                        DCodigoIterno = d.DCodigoIterno,
                        BEstado = d.BEstado,
                        RangoDesde = d.RangoDesde,
                        RangoHasta = d.RangoHasta
                    })
                    .ToList();
            }
            
                    
            return parametroDetalle;

        }

        public IEnumerable<ParametroDetalleMapper> getParametroDetallePorCodigoInternoPadre(string codigoIterno, long IdDetalle)
        {
            var parametro = _context.Parametro
                    .Where(p => p.BEstado == true && p.VcCodigoInterno == codigoIterno)
                    .FirstOrDefault();

            IEnumerable<ParametroDetalleMapper> parametroDetalle = Array.Empty<ParametroDetalleMapper>();

            if (parametro != null)
            {
                parametroDetalle = _context.ParametroDetalle
                   .Where(p => p.BEstado == true && p.IdPadre == IdDetalle && p.ParametroId == parametro.Id)
                   .Select(d => new ParametroDetalleMapper
                   {
                       Id = d.Id,
                       ParametroId = d.ParametroId,
                       VcNombre = d.VcNombre,
                       TxDescripcion = d.TxDescripcion,
                       IdPadre = d.IdPadre,
                       VcCodigoInterno = d.VcCodigoInterno,
                       DCodigoIterno = d.DCodigoIterno,
                       BEstado = d.BEstado,
                       RangoDesde = d.RangoDesde,
                       RangoHasta = d.RangoHasta
                   })
                   .ToList();
            }


            return parametroDetalle;
        }


        public IEnumerable<ParametroDetalleMapper> getParametroDetallePorPadre(long IdDetalle)
        {


            IEnumerable<ParametroDetalleMapper> parametroDetalle = Array.Empty<ParametroDetalleMapper>();


            parametroDetalle = _context.ParametroDetalle
                .Where(p => p.BEstado == true && p.IdPadre == IdDetalle)
                .Select(d => new ParametroDetalleMapper
                {
                    Id = d.Id,
                    ParametroId = d.ParametroId,
                    VcNombre = d.VcNombre,
                    TxDescripcion = d.TxDescripcion,
                    IdPadre = d.IdPadre,
                    VcCodigoInterno = d.VcCodigoInterno,
                    DCodigoIterno = d.DCodigoIterno,
                    BEstado = d.BEstado,
                    RangoDesde = d.RangoDesde,
                    RangoHasta = d.RangoHasta
                })
                .ToList();



            return parametroDetalle;
        }
        

    }

}
