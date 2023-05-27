namespace WebApplication2.ServicesList.Service {
    public class ServiceListGroupedByCategory {

        public List<Models.Service> GetCategories(List<Models.Service> services) {

            return services.GroupBy(x => x.Category).Select(x => x.First()).ToList();
        }
    }
}
