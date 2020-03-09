http://learnings.MSFullStackers.com/index.php/2018/07/11/rs2-core/

# Core.Data

* .Net Core Data Access Framework with Generic Repository pattern.


===========================================================

Examples 

## Creating Entity:

### Master entities:
public class Country : BaseEntity<int>
{
}

public class Product : BaseEntity<long>
{
}

### Transaction entities:
public class TaskItem : BaseEntityTrackable<long>
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

	protected override IQueryable<Product> GetEntitySet(bool incluedeDelete = true)
	{			
		return Entity.Include(e => e.Category).Where(p => !incluedeDelete || !p.InActive);
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