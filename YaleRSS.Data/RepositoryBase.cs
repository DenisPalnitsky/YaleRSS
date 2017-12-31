namespace YaleRss.Data
{
    public abstract class RepositoryBase
    {
        protected IDbContext Context { get; private set; }

        public RepositoryBase(IDbContext context)
        {
            Context = context;
        }
    }
}
