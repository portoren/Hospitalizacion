using System.Configuration;

using Microsoft.Practices.EnterpriseLibrary.Data;


namespace DataAccess
{

    public abstract class DABase
    {

        #region Variables

        protected Database dbConn;

        #endregion

        #region Constructor

        public DABase()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            dbConn = factory.Create(ConfigurationManager.AppSettings["conexionDB"]);
        }

        #endregion

        #region Propiedades

        public Database db
        {
            get { return dbConn; }
            set { dbConn = value; }
        }

        #endregion

    }

}
