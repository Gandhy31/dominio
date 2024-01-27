#nullable disable

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unach.Codesi.Encuestas.Dominio.Core
{
    public class DominioGenerico<TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _contexto.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        TDbContext _contexto = null;
        private Dictionary<string, object> _repositorios;

        public DominioGenerico(TDbContext contexto)
        {
             _contexto = contexto;
            _repositorios = new Dictionary<string, object>();
        }
        public void GuardarTransacciones()
        {
            _contexto.SaveChanges();             
        }

        public IRepositorioGenerico<T> GetRepositorio<T>() where T : class
        {
            string name = typeof(T).Name;
            if (!_repositorios.ContainsKey(name))
            {
                _repositorios.Add(name, new RepositorioGenerico<T,TDbContext>(_contexto));
            }
            return _repositorios[name] as IRepositorioGenerico<T>;
        }


    }

    /// <summary>        /// Ejecutar SP con timeout extendido a 120 s        /// </summary>        /// <typeparam name="T"></typeparam>        /// <param name="name"></param>        /// <param name="nameValueParams"></param>        /// <returns></returns>       

}
