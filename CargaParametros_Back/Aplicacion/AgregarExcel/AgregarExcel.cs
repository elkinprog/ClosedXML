using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Dominio.Request;
using Persistencia.Repository;
using SpreadsheetLight;
using System.Data;
using DateTime = System.DateTime;

namespace Aplicacion.AgregarExcel
{
    public interface IAgregarExcel
    {
        ParametroRequest parametro(string ruta);
        ParametroRequest parametrodetalle(string ruta, DataTable parametros, ref List<string> errores);
        ParametroRequest  procesarArchivo(string ruta);
    }

    public class AgregarExcel   : IAgregarExcel 
    {
        public ParametroRepository _parametroRepository { get; }

        public AgregarExcel(ParametroRepository parametroRepository)
        {
            this._parametroRepository = parametroRepository;
        }

        public ParametroRequest procesarArchivo(string ruta)
        {
            var response = parametro(ruta);
            var list = response.Parametros;
            var errores = response.Errores;

            if (response.Errores.Count == 0 && list.Rows.Count > 0)
            {
                var responseDetalle = parametrodetalle(ruta, list, ref errores);

                if (responseDetalle.Errores.Count == 0)
                {
                    response.ParametroDetalles = responseDetalle.ParametroDetalles;
                    response.Errores = errores;
                    _parametroRepository.insertMassiveData(response);
                    response.Registros = list.Rows.Count + response.ParametroDetalles.Rows.Count;
                }
            }
            return response;
        }


        public ParametroRequest parametro(string ruta) // NUEVO fUNCIONAL
        {


            using (XLWorkbook workBook = new XLWorkbook(ruta))
            {


                var worksheet = workBook.Worksheet(1);


                List<string> errores = new List<string>();

                var dt = new DataTable();

                dt.Columns.Add("Id", typeof(long));
                dt.Columns.Add("VcNombre", typeof(string));
                dt.Columns.Add("VcCodigoInterno", typeof(string));
                dt.Columns.Add("BEstado", typeof(bool));
                dt.Columns.Add("DtFechaCreacion", typeof(DateTime));
                dt.Columns.Add("DtFechaActualizacion", typeof(DateTime));
                dt.Columns.Add("DtFechaAnulacion", typeof(DateTime));

                try
                {

                    int fila = 2;

                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {

                        long Id = Convert.ToInt64(worksheet.Cell(fila, 1).Value);
                        string VcNombre = Convert.ToString(worksheet.Cell(fila, 2).Value);
                        string VcCodigoInterno = Convert.ToString(worksheet.Cell(fila, 3).Value);
                        string BEstado = Convert.ToString(worksheet.Cell(fila, 4).Value);


                        if (Id <= 0)
                        {
                            errores.Add("El id del parámetro, no puede estar vacio o tener texto en la celda (A" + row.ToString() + ")");
                        }

                        if (String.IsNullOrEmpty(VcNombre))
                        {
                            errores.Add("El nombre del parámetro (" + VcNombre + "), no puede ser vacio en la celda (B" + row.ToString() + ")");
                        }

                        if (VcNombre.Length > 100)
                        {
                            errores.Add("El nombre parámetro (" + VcNombre + "), no puede ser mayor a 100 caracteres, en la celda (B" + row.ToString() + ")");
                        }

                        if (String.IsNullOrEmpty(VcCodigoInterno))
                        {
                            errores.Add("El nombre del código interno (" + VcCodigoInterno + "), este no puede ser vacio en la celda (C" + row.ToString() + ")");
                        }

                        if (VcCodigoInterno.Length > 50)
                        {
                            errores.Add("El código interno (" + VcCodigoInterno + "), no puede ser mayor a 50 caracteres en la celda (C" + row.ToString() + ")");
                        }

                        if (String.IsNullOrEmpty(BEstado))
                        {
                            errores.Add("No se encontró el estado, este no puede ser vacio en la celda (D" + row.ToString() + ")");
                        }

                        if (!(BEstado.Equals("0") || BEstado.Equals("1")))
                        {
                            errores.Add("El estado debe tener valores 0 o 1, en la celda (D" + row.ToString() + ")");
                        }


                        var validacion = _ = _parametroRepository.ValidarExisteParametro(VcCodigoInterno);


                        if (errores.Count == 0 && !validacion.Result)
                        {

                            dt.Rows.Add(new object[]
                            {
                                Id,
                                VcNombre,
                                VcCodigoInterno,
                                BEstado == "1",
                                DateTime.Now,
                                DateTime.Now,
                                null
                            });
                        }


                        fila++;

                    }

                    return new ParametroRequest
                    {
                        Parametros = dt,
                        Errores = errores,
                    };

                }
                catch (Exception ex)
                {

                    throw;
                }
            }


        }



        public ParametroRequest parametrodetalle(string ruta, DataTable parametros, ref List<string> errores)  // NUEVO
        {


            using (XLWorkbook workBook = new XLWorkbook(ruta))
            {


                var worksheett = workBook.Worksheet(2);

                var data = new DataTable();

                data.Columns.Add("Id", typeof(long));
                data.Columns.Add("ParametroId", typeof(long));
                data.Columns.Add("VcNombre", typeof(string));
                data.Columns.Add("TxDescripcion", typeof(string));
                data.Columns.Add("IdPadre", typeof(long));
                data.Columns.Add("VcCodigoInterno", typeof(string));
                data.Columns.Add("DCodigoIterno", typeof(decimal));
                data.Columns.Add("BEstado", typeof(Boolean));
                data.Columns.Add("RangoDesde", typeof(int));
                data.Columns.Add("RangoHasta", typeof(int));


                try
                {
                    int fila = 2;

                    foreach (var rows in worksheett.RowsUsed().Skip(1))
                    {

                        long Id;
                        string ide = Convert.ToString(worksheett.Cell(fila, 1).Value);
                        long.TryParse(ide, out Id);


                        long ParametroId;
                        string paramId = Convert.ToString(worksheett.Cell(fila, 2).Value);
                        long.TryParse(paramId, out ParametroId);


                        string  VcNombre        = Convert.ToString(worksheett.Cell(fila, 3).Value);
                        string  TxDescripcion   = Convert.ToString(worksheett.Cell(fila, 4).Value);
                        string  IdPadre         = Convert.ToString(worksheett.Cell(fila, 5).Value);
                        string  VcCodigoInterno = Convert.ToString(worksheett.Cell(fila, 6).Value);
                        string  DCodigoIterno   = Convert.ToString(worksheett.Cell(fila, 7).Value);
                        string  BEstado         = Convert.ToString(worksheett.Cell(fila, 8).Value);



                        string Desde = Convert.ToString(worksheett.Cell(fila, 9).Value);
                        int RangoDesde;
                        int.TryParse(Desde, out RangoDesde);


                        string Hasta = Convert.ToString(worksheett.Cell(fila, 10).Value);
                        int RangoHasta;
                        int.TryParse(Hasta, out RangoHasta);


                        if (Id <= 0 )
                        {
                            errores.Add("El id del parámetrodetalle, no puede estar vacio o contener texto en la celda (A" + rows.ToString() + ")");
                        }

                        if (ParametroId <= 0)
                        {
                            errores.Add("El parámetroid del parámetrodetalle, no puede estar vacio o tener texto en la celda (B" + rows.ToString() + ")");
                        }

                        var parametro = (from x in parametros.Rows.OfType<DataRow>()
                                         where x.Field<long>("Id") == ParametroId
                                         select x).ToList();

                        if (parametro.Count == 0)
                        {
                            errores.Add("El id(" + Id + "), de parámetro no coincide con la lista de parámetrodetalle (" + ParametroId + "),");
                        }

                        if (String.IsNullOrEmpty(VcNombre))
                        {
                            errores.Add("El nombre del parámetrodetalle (" + VcNombre + "), no puede ser vacio en la celda (C" + rows.ToString() + ")");
                        }


                        if (!String.IsNullOrEmpty(IdPadre))
                        {
                            var parametrodetalle = (from x in data.Rows.OfType<DataRow>()
                                                    where x.Field<long>("Id") == long.Parse(IdPadre)
                                                    select x).ToList();
                            if (parametrodetalle.Count == 0)
                            {
                                errores.Add("El IdPadre de parámetrodetalle no esta en la lista de parametros enviados en la celda (E" + rows.ToString() + ")");
                            }
                        }

                        if (!String.IsNullOrEmpty(DCodigoIterno))
                        {
                            if (!decimal.TryParse(DCodigoIterno, out _))
                            {
                                errores.Add("El codigoInterno  de parámetrodetalle debe ser un decimal (G" + rows.ToString() + ")");
                            }
                        }

                        if (String.IsNullOrEmpty(BEstado))
                        {
                            errores.Add("El estado de parámetrodetalle no puede ser vacio en la celda (H" + rows.ToString() + ")");
                        }



                        if (RangoHasta < RangoDesde)
                        {
                            errores.Add("El RangoHasta  de parámetro detalle no puede ser menor al  RangoDesde  de parámetro detalle en la celda (J" + rows.ToString() + ")");
                        }


                        if (errores.Count == 0)
                        {

                            _ = data.Rows.Add(new object[]
                            {
                                Id,
                                ParametroId,
                                VcNombre,
                                TxDescripcion,
                                long.TryParse(IdPadre,out long numero) ?  numero : null,
                                VcCodigoInterno,
                                decimal.TryParse(DCodigoIterno, out decimal valor) ? valor : null,
                                BEstado == "1",
                                RangoDesde,
                                RangoHasta,
                            });
                        }

                        fila++;

                    }
                    

                    return new ParametroRequest
                    {
                        ParametroDetalles = data,
                        Errores = errores,
                    };

                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }

    }
}
        







