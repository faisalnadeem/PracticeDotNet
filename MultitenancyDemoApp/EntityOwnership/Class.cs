using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[Route("api/yourcontroller")]
public class YourApiController : Controller
{
    private readonly YourEntityXYZRepository _repo;

    public YourApiController(YourDbContext yourDbContext)
    {
        _repo = new YourEntityXYZRepository(yourDbContext);
    }

    [HttpGet("{id}")]
    [AuthorizeOwnerIntId(typeof(YourEntityXYZRepository), Policy = "YourCustomPolicy")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = _repo.GetById(id);
        return Ok(entity);
    }
}

// The "generic" authorization attribute for type int id
// Similar authorization attributes for every type of id must be created additionally, for example Guid
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AuthorizeOwnerIntIdAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private object _entityRepositoryObject;
    private IAsyncOwnerIntId _entityRepository;
    private readonly Type _TCrudRepository;

    public AuthorizeOwnerIntIdAttribute(Type TCrudRepository)
    {
        _TCrudRepository = TCrudRepository;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var yourDbContext = context.HttpContext.RequestServices.GetService<YourDbContext>();
        _entityRepositoryObject = Activator.CreateInstance(_TCrudRepository, yourDbContext);
        _entityRepository = _entityRepositoryObject as IAsyncOwnerIntId;

        var user = context.HttpContext.User;

        if (!user.Identity.IsAuthenticated)
        {
            // it isn't needed to set unauthorized result 
            // as the base class already requires the user to be authenticated
            // this also makes redirect to a login page work properly
            // context.Result = new UnauthorizedResult();
            return;
        }

        // get entityId from uri
        var idString = context.RouteData.Values["id"].ToString();
        if (!int.TryParse(idString, out var entityId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // get subjectId from user claims
        var ownerIdString = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        if (!Guid.TryParse(ownerIdString, out var ownerGuid))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!_entityRepository.IsEntityOwner(entityId, ownerGuid))
        {
            context.Result = new UnauthorizedResult();
        }
    }
}

// Your concrete repository
public class YourEntityXYZRepository : AsyncCrud<YourEntityXYZ, int>,
    IAsyncOwnerIntId // Note that type concrete IAsyncOwnerIntId is only implemented in concrete repository
{
    public YourEntityXYZRepository(YourDbContext yourDbContext) : base(yourDbContext)
    {

    }
}

// Your generic Crud repository
public abstract class AsyncCrud<TEntity, TId> : IAsyncCrud<TEntity, TId>
    where TEntity : class, IEntityUniqueIdentifier<TId>, IEntityOwner
    where TId : struct
{
    protected YourDbContext YourDbContext;

    public AsyncCrud(YourDbContext yourDbContext)
    {
        YourDbContext = yourDbContext;
    }

    // Note that the following single concrete implementation satisfies both interface members 
    // bool IsEntityOwner(TId id, Guid ownerGuid); from IAsyncCrud<TEntity, TId> and
    // bool IsEntityOwner(int id, Guid ownerGuid); from IAsyncOwnerIntId
    public bool IsEntityOwner(TId id, Guid ownerGuid)
    {
        var entity = YourDbContext.Set<TEntity>().Find(id);
        if (entity != null && entity.OwnerGuid == ownerGuid)
        {
            return true;
        }

        return false;
    }

    // Further implementations (redacted)
    public Task<bool> SaveContext() { throw new NotImplementedException(); }
    public Task<TEntity> Update(TEntity entity) { throw new NotImplementedException(); }
    public Task<TEntity> Create(TEntity entity, Guid ownerGuid) { throw new NotImplementedException(); }
    public Task<bool> Delete(TId id) { throw new NotImplementedException(); }
    public Task<bool> DoesEntityExist(TId id) { throw new NotImplementedException(); }
    public virtual Task<TEntity> GetById(TId id) { throw new NotImplementedException(); }
}

// The interface for the Crud operations
public interface IAsyncCrud<TEntity, TId>
    where TEntity : class, IEntityUniqueIdentifier<TId>
    where TId : struct
{
    bool IsEntityOwner(TId id, Guid ownerGuid);
    Task<bool> DoesEntityExist(TId id);
    Task<TEntity> GetById(TId id);
    Task<TEntity> Create(TEntity entity, Guid ownerGuid);
    Task<TEntity> Update(TEntity entity);
    Task<bool> Delete(TId id);
    Task<bool> SaveContext();
}

// The interface for the concrete type method for int id
// Similar interfaces for every type of id must be created additionally, for example Guid
public interface IAsyncOwnerIntId
{
    bool IsEntityOwner(int id, Guid ownerGuid);
}

// Typical db context
public class YourDbContext : DbContext
{
    public YourDbContext(DbContextOptions<YourDbContext> options) : base(options)
    {

    }

    public DbSet<YourEntityXYZ> YourEntityXYZ { get; set; }
}


public class YourEntityXYZ : IEntityUniqueIdentifier<int>, IEntityOwner
{
    public int Id { get; set; }
    public Guid? OwnerGuid { get; set; }
    // ... Additonal custom properties
}

public interface IEntityUniqueIdentifier<TId>
    where TId : struct
{
    TId Id { get; set; }
}

public interface IEntityOwner
{
    Guid? OwnerGuid { get; set; }
}