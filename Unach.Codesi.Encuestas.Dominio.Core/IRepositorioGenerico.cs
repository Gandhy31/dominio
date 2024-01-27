using System.Linq.Expressions;

namespace Unach.Codesi.Encuestas.Dominio.Core
{
    public interface IRepositorioGenerico<TEntidad> where TEntidad : class
    {
        void Actualizar(List<TEntidad> entidadesActualizar);
        void Actualizar(TEntidad entidadActualizar);
        TEntidad Buscar(int id);
        Task<TEntidad> BuscarAsync(int id);
        List<TEntidad> BuscarPor(Expression<Func<TEntidad, bool>> filtro = null, Func<IQueryable<TEntidad>, IOrderedQueryable<TEntidad>> ordenarPor = null, params Expression<Func<TEntidad, object>>[] entidadesRelacionadasAIncluir);
        int Contar(Expression<Func<TEntidad, bool>> filtro = null);
        void Eliminar(int id);
        void Eliminar(TEntidad entidadAeliminar);
        void Insertar(TEntidad entidad);
        List<TEntidad> ObtenerTodos();
        List<T2> ObtenerTodosEnOtraVista<T2>(Expression<Func<TEntidad, T2>> vista, Expression<Func<TEntidad, bool>> filtro = null, Func<IQueryable<TEntidad>, IOrderedQueryable<TEntidad>> ordenarPor = null) where T2 : class;
        List<TEntidad> ObtenerTodosEnVistaParcial(Expression<Func<TEntidad, TEntidad>> vista, Expression<Func<TEntidad, bool>> filtro = null);
        List<TEntidad> ObtenerTop(Expression<Func<TEntidad, bool>> filtro = null, Func<IQueryable<TEntidad>, IOrderedQueryable<TEntidad>> ordenarPor = null, int top = 0);
    }
}