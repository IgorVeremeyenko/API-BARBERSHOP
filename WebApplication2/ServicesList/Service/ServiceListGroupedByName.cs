namespace WebApplication2.ServicesList.Service {
    public class ServiceListGroupedByName {

        public List<Models.Service> GetNames(List<Models.Service> services) {

            return services.GroupBy(x => x.Name).Select(x => x.First()).ToList();
        }

    }
}
