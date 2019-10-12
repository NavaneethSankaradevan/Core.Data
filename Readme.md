http://learnings.MSFullStackers.com/index.php/2018/07/11/rs2-core/

# MFS.Core

* .Net Core Data Access Framework with Generic Repository pattern.


===========================================================

Examples 

## Creating Entity:

### Master entities:
public class Country : BaseEntity<int>
{
}

public class Person : BaseEntity<long>
{
}

### Transaction entities:
public class TaskItem : TranBaseEntity<long>
{
}

============================================================

## Business Logic :

### CRUD repositories :
public interface IProductRepository : IBaseRepository<Product, long>
{
}

public class ProductRepository : BaseRepository<Product, long>, IProductRepository
{
    public ProductRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
	protected override IQueryable<Product> GetEntitySet(bool includeDeleted = false)
	{		
        if(!includeDelete)
        {
            return Entity.Include(e => e.Category).Where(p => !p.InActive);
        }
        
		return Entity.Include(e => e.Category)
	}
}

### Read only repositories:
public interface ICountryRepository : IReadRepository<Country, int>
{
}

public class CountryRepository : BaseRepository<Country, int>, ICountryRepository
{
    public CountryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}

============================================================


Issues/Feedback or Clarification : MSFullStackers@gmail.com