using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using EF_Produccion;
using CertificadoLoteDecenalMVC.Models.Business.FileRecords;

namespace CertificadoLoteDecenalMVC.Models.DAL.Home
{
    public static class HomeDao
    {
        /// <summary>
        /// Representa una póliza compuesta leída del archivo de Excel.
        /// Esta póliza tiene el formato "[cod_suc]-[cod_ramo]-[nro_pol]-[nro_endoso]-[XX]"
        /// </summary>
        class PolizaCompuesta
        {
            public decimal CodSucursal { get; private set; }
            public decimal CodRamo { get; private set; }
            public decimal NroPoliza { get; private set; }
            public int NroEndoso { get; private set; }

            public PolizaCompuesta(string compuesta)
            {
                var elementos = compuesta.Split('-');

                CodSucursal = decimal.Parse(elementos[0]);
                CodRamo = decimal.Parse(elementos[1]);
                NroPoliza = decimal.Parse(elementos[2]);
                NroEndoso = int.Parse(elementos[3]);
            }
        }

        /// <summary>
        /// Procesa los registros y obtiene una colección de (Número de Póliza, Número de Endoso, Número de Lote) por
        /// cada grupo de pólizas.
        /// </summary>
        /// <param name="agrupados">Los registros leídos del archivo de Excel agrupados por su número de póliza.</param>
        /// <returns>Una lista de objetos PolizaEndosoLote.</returns>
        public static IEnumerable<PolizaEndosoLote> ObtenerNumerosLote(ILookup<string, Certificado> agrupados)
        {
            var numerosLote = new List<PolizaEndosoLote>();
            var ubicacion = 2; // TODO:  Octubre 2018 - Por defecto se asigna la ubicación 2 hasta nuevo aviso.

            using (var db = new ProduccionEntities()) {
                db.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                using (var transaction = db.Database.BeginTransaction()) {
                    try {
                        foreach (var grupo in agrupados) {
                            var polizaCompuesta = new PolizaCompuesta(grupo.Key);
                            var numeroLote = new ObjectParameter("ult_nro", typeof(int));

                            // Se almacena el número de póliza y su respectivo número de endoso y lote.
                            db.spiu_tvarias_ult_nro("di_certificado_lote_decenal", numeroLote);

                            numerosLote.Add(new PolizaEndosoLote {
                                NroPoliza = (int)polizaCompuesta.NroPoliza,
                                NroEndoso = polizaCompuesta.NroEndoso,
                                Lote = (int)numeroLote.Value
                            });
                            
                            foreach (var registro in grupo) {
                                var idRegCert = new ObjectParameter("ult_nro", typeof(int));

                                // Se obtiene el número de registro de certificado
                                db.spiu_tvarias_ult_nro("di_certificado_decenal", idRegCert);

                                // Se inserta el nuevo registro en [di_certificado_lote_decenal]
                                var lote = new di_certificado_lote_decenal {
                                    id_reg_cer = (int)idRegCert.Value,
                                    nro_lote = (int)numeroLote.Value,
                                    txt_certificado = registro.NoCertificado,
                                    txt_usuario_alta = "PROCESO",
                                    fec_alta = DateTime.Now,
                                    txt_status_cer = "ACTIVADO"
                                };

                                db.di_certificado_lote_decenal.Add(lote);

                                // Y finalmente se inserta el registro leído del Excel en [di_certificado_decenal]
                                var certificado = new di_certificado_decenal {
                                    id_reg_cer = (int)idRegCert.Value,
                                    id_pv = null,
                                    cod_suc = polizaCompuesta.CodSucursal,
                                    cod_ramo = polizaCompuesta.CodRamo,
                                    nro_pol = polizaCompuesta.NroPoliza,
                                    txt_nro_pol = grupo.Key,
                                    cod_item = ubicacion,
                                    txt_certificado = registro.NoCertificado,
                                    fec_ini = registro.FecIni,
                                    fec_fin_seg = null,                             // TODO: Averiguar el valor actual de este campo
                                    fec_fin_tres = registro.FecFinTres,
                                    fec_fin_cinco = registro.FecFinCinco,
                                    fec_fin_diez = registro.FecFinDiez,
                                    imp_suma_asegurada = registro.Avaluo,
                                    imp_admon_riesgos = registro.Anticipo,
                                    imp_prima_neto_end = registro.PrimaNetaEndo,
                                    imp_prima_neta_real = registro.PrimaNetaReal,
                                    imp_prima_hab = registro.PrimaHabitabilidad,
                                    imp_prima_seg = registro.PrimaSeguridad,
                                    imp_prima_3 = registro.PrimaImpermeabilizacionCubiertas,
                                    imp_prima_4 = registro.ImpermeabilizacionFachadas,
                                    imp_millar = registro.CuotaMillar,
                                    txt_nombre = registro.NombreAcreditado,
                                    txt_RFC = registro.RFC,
                                    txt_NSS = registro.NSS,
                                    txt_desarrollo = registro.Desarrollo,
                                    nro_credito = registro.NoCredito,
                                    cod_CUV = registro.CUV,
                                    txt_condominio = registro.Condominio,
                                    txt_calle = registro.Calle,
                                    nro_exterior = registro.NoExterior,
                                    nro_interior = registro.NoInterior,
                                    txt_mz = registro.Manzana,
                                    txt_lt = registro.Lote,
                                    txt_super_mz = registro.SuperManzana,
                                    txt_edif = registro.Edificio,
                                    txt_nivel = registro.Nivel,
                                    cod_postal = registro.CP,
                                    cod_colonia = registro.Colonia,
                                    cod_municipio = registro.Municipio,
                                    cod_entidad = registro.Entidad,
                                    txt_nom_constructor = registro.Desarrollador,
                                    fec_ini_garantia = registro.InicioGarantia,
                                    fec_fin_obra = registro.FinObra,
                                    fec_adquisicion = registro.Adquisicion,
                                    txt_nom_notario = registro.NombreNotario,
                                    nro_escritura = registro.NoEscritura,
                                    fecha = registro.DeFecha,
                                    nro_notaria = registro.NoNotario
                                };

                                db.di_certificado_decenal.Add(certificado);
                            }
                        }

                        db.SaveChanges();
                        transaction.Commit();
                    } catch {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return numerosLote;
        }
    }
}