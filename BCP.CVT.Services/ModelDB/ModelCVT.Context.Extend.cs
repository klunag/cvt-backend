using System;
using System.Data.Entity;
using System.Data.SqlClient;
using BCP.CVT.Cross;
using System.Data.Entity.Core.EntityClient;


namespace BCP.CVT.Services.ModelDB
{
    public partial class GestionCMDB_ProdEntities : DbContext
    {
        private GestionCMDB_ProdEntities(string connectionString)
       : base(connectionString)
        {
        }

        public static GestionCMDB_ProdEntities ConnectToSqlServer()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = Constantes.ServidorSQL,
                InitialCatalog = Constantes.BaseDatosSQL,
                PersistSecurityInfo = true,
                IntegratedSecurity = false,
                MultipleActiveResultSets = true,
                UserID = Constantes.UsuarioSQL,
                Password = Constantes.PasswordSQL,
            };

            // assumes a connectionString name in .config of MyDbEntities
            var entityConnectionStringBuilder = new EntityConnectionStringBuilder
            {
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = sqlBuilder.ConnectionString,
                Metadata = "res://*/ModelDB.ModelCVT.csdl|res://*/ModelDB.ModelCVT.ssdl|res://*/ModelDB.ModelCVT.msl",
            };

            return new GestionCMDB_ProdEntities(entityConnectionStringBuilder.ConnectionString);
        }
    }
}
