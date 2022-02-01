using Woldrich.CosmosModel;

namespace Woldrich.CosmosData;

public class CustomerDao 
{
    public async Task<Customer> CrupdateCustomer(Customer customer) {
        using (var context = new CosmosDataContext())
        {
            // TODO: assert HashKey and RangeKey are not null

            context.Customers.Add(customer);

            await context.SaveChangesAsync();

            return customer;
        }
    }

    public async Task<Customer?> GetCustomerByIdAsync(CosmosKey id) {
        using (var context = new CosmosDataContext()) 
        {
            return await context.Customers.FindAsync(id.HashKey, id.RangeKey);
        }
    }
}