namespace WebApplication2.ServicesList.Service {
    public class FilteredByName {

        public List<Models.Service> GetList(string name, List<Models.Service> services) {
            return services.Where(x => x.Category == name).GroupBy(x => x.Name).Select(group => group.First()).ToList();
        }
    }
}
