#nullable disable

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Unach.Codesi.Encuestas.Dominio.Core
{
    //public interface IRepositorioGenerico<TEntidad> where TEntidad : class
    //{
    //    public TEntidad Buscar(int id);
    //}
    public class RepositorioGenerico<TEntidad, TDbContext> : IRepositorioGenerico<TEntidad>
        //: IRepositorioGenerico<TEntidad>
        where TEntidad : class
        where TDbContext : DbContext
    {
        TDbContext _contexto = null;
        private DbSet<TEntidad> _entidades;

        public RepositorioGenerico(TDbContext context)
        {
            _contexto = context;
            _entidades = _contexto.Set<TEntidad>();
        }

        /// <summary>
        /// Buscar por Id de Entidad
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntidad Buscar(int id)
        {
            return _entidades.Find(id);
        }

        /// <summary>
        /// Buscar por Id de Entidad (Asíncrono)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntidad> BuscarAsync(int id)
        {
            return await _entidades.FindAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entidadesActualizar"></param>
        public void Actualizar(List<TEntidad> entidadesActualizar)
        {
            _contexto.UpdateRange(entidadesActualizar);
        }

        /// <summary>
        /// Contar en base a filtro
        /// </summary>
        /// <param name="filtro">Filtro como expresión lambda (x=>x.Propiedad == 1)</param>
        /// <returns>Cantidad de elementos que cumplen el filtro</returns>
        public int Contar(Expression<Func<TEntidad, bool>> filtro = null)
        {
            IQueryable<TEntidad> query = _entidades.AsNoTracking();
            if (filtro != null)
                query = query.Where(filtro);
            return query.Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filtro">Expresión lambda que resentada Ej. x=>x.Id == 2</param>
        /// <param name="ordenarPor">y=>(y.OrderBy(z=>z.Propiedad))</param>
        /// <param name="entidadesRelacionadasAIncluir">x=>(x as Entidad).OtraEntidadRelacionada (Padre, Hijo)</param>
        /// <returns></returns>
        public List<TEntidad> BuscarPor(Expression<Func<TEntidad, bool>> filtro = null,
                                    Func<IQueryable<TEntidad>, IOrderedQueryable<TEntidad>> ordenarPor = null,
                                    params Expression<Func<TEntidad, object>>[] entidadesRelacionadasAIncluir)//string includeProperties = "",
        {
            List<TEntidad> resultado = null;
            IQueryable<TEntidad> query = _entidades.AsNoTracking();
            if (filtro != null)
                query = query.Where(filtro);

            if (entidadesRelacionadasAIncluir != null)
                query = entidadesRelacionadasAIncluir.Aggregate(query, (current, include)
                    => current.Include(include));

            if (ordenarPor != null)
                resultado = ordenarPor(query).ToList();
            else
                resultado = query.ToList();

            return resultado;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filtro"></param>
        /// <param name="ordenarPor"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<TEntidad> ObtenerTop(Expression<Func<TEntidad, bool>> filtro = null,
                           Func<IQueryable<TEntidad>, IOrderedQueryable<TEntidad>> ordenarPor = null,
            int top = 0)
        {
            List<TEntidad> resultado = null;
            IQueryable<TEntidad> query = _entidades.AsNoTracking();


            if (filtro != null)
                query = query.Where(filtro);

            if (ordenarPor != null)
                resultado = ordenarPor(query).Take(top).ToList();
            else
                resultado = query.Take(top).ToList();

            return resultado;
        }


        public List<TEntidad> ObtenerTodos()
        {
            return _entidades.AsNoTracking().AsEnumerable().ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="vista"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public List<TEntidad> ObtenerTodosEnVistaParcial(Expression<Func<TEntidad, TEntidad>> vista,
            Expression<Func<TEntidad, bool>> filtro = null)
        {
            IQueryable<TEntidad> query = _entidades.AsNoTracking();
            if (filtro != null)
                query = query.Where(filtro);
            return query.Select(vista).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="vista"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public List<T2> ObtenerTodosEnOtraVista<T2>(Expression<Func<TEntidad, T2>> vista,
            Expression<Func<TEntidad, bool>> filtro = null,
            Func<IQueryable<TEntidad>, IOrderedQueryable<TEntidad>> ordenarPor = null) where T2 : class
        {
            IQueryable<TEntidad> query = _entidades.AsNoTracking();
            if (filtro != null)
                query = query.Where(filtro);

            if (ordenarPor != null)
                query = ordenarPor(query);

            return query.Select(vista).ToList<T2>();
        }

        public void Insertar(TEntidad entidad)
        {
            _entidades.Add(entidad);
        }

        public void Actualizar(TEntidad entidadActualizar)
        {
            _entidades.Attach(entidadActualizar);
            _contexto.Entry(entidadActualizar).State = EntityState.Modified;
        }

        public void Eliminar(TEntidad entidadAeliminar)
        {
            if (_contexto.Entry(entidadAeliminar).State == EntityState.Detached)
            {
                _entidades.Attach(entidadAeliminar);
            }
            _entidades.Remove(entidadAeliminar);
        }

        public void Eliminar(int id)
        {
            var entidadAeliminar = Buscar(id);
            if (_contexto.Entry(entidadAeliminar).State == EntityState.Detached)
            {
                _entidades.Attach(entidadAeliminar);
            }
            _entidades.Remove(entidadAeliminar);
        }
    }
}