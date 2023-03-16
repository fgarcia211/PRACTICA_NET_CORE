using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreZapatillas.Data;
using MvcNetCoreZapatillas.Models;
using System.Data;

#region SQL SERVER
//VUESTRO PROCEDIMIENTO DE PAGINACION DE IMAGENES DE ZAPATILLAS
/*CREATE OR ALTER PROCEDURE SP_GETZAPATILLAID
(@IDPRODUCTO INT)
AS
	SELECT * FROM ZAPASPRACTICA WHERE IDPRODUCTO = @IDPRODUCTO
GO

CREATE OR ALTER PROCEDURE SP_IMAGENES_PRODUCTO
(@POSICION INT, @IDPRODUCTO INT)
AS
   SELECT * FROM
        (SELECT CAST(
            ROW_NUMBER() OVER (ORDER BY IDIMAGEN) AS INT) AS POSICION, 
            IDIMAGEN, IDPRODUCTO, IMAGEN
        FROM IMAGENESZAPASPRACTICA
        WHERE IDPRODUCTO = @IDPRODUCTO) AS QUERY
    WHERE QUERY.POSICION = @POSICION
GO*/
#endregion

namespace MvcNetCoreZapatillas.Repositories
{
    public class RepositoryZapatillas
    {
        private ZapatillasContext context;

        public RepositoryZapatillas(ZapatillasContext context)
        {
            this.context = context;
        }

        public List<Zapatilla> GetAllZapatillas()
        {
            return this.context.Zapatillas.ToList();
        }

        public Zapatilla GetOneZapatilla(int idproducto)
        {
            string sql = "SP_GETZAPATILLAID @IDPRODUCTO";
            SqlParameter pamId = new SqlParameter("@IDPRODUCTO", idproducto);
            var consulta = this.context.Zapatillas.FromSqlRaw(sql,pamId);
            Zapatilla zapa= consulta.AsEnumerable().FirstOrDefault();
            return zapa;
        }

        public ImagenZapatilla GetImagenZapatilla(int idproducto, int posicion)
        {
            string sql = "SP_IMAGENES_PRODUCTO @POSICION, @IDPRODUCTO";

            SqlParameter pamPos = new SqlParameter("@POSICION", posicion);
            SqlParameter pamIdPro = new SqlParameter("@IDPRODUCTO", idproducto);

            var consulta = this.context.ImagenesZapatillas.FromSqlRaw(sql, pamPos, pamIdPro);
            ImagenZapatilla imagenZapatillas = consulta.AsEnumerable().FirstOrDefault();

            return imagenZapatillas;

        }

        public int CountImagenesZapatilla(int idproducto)
        {
            return this.context.ImagenesZapatillas.Where(z => z.IdProducto == idproducto).Count();
        }

    }
}
