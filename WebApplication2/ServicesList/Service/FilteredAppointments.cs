
using WebApplication2.Models;

namespace WebApplication2.ServicesList {
    public class FilteredAppointments {

        public List<Categorized> GetList(List<Models.Service> services) {

            var categories = services.GroupBy(x => x.Category).Select(group => group.First()).ToList();
            List<Categorized> categorized = new List<Categorized>();
            var cat = new Categorized();
            foreach (var item in categories)
            {
                cat = new Categorized();
                cat.children = new List<Models.Service>();

                cat.Category = item.Category;
                cat.children = services.Where(x => x.Category == item.Category).GroupBy(x => x.Name).Select(g => g.First()).ToList();
                categorized.Add(cat);
            }

            return categorized;
        }

    }

    public class Categorized {
        public int Id { get; set; }
        public string Category { get; set; }

        public List<Models.Service> children { get; set; }
    }

    public class Children {
        public string Cname { get; set; }
        public int Cprice { get; set; }
    }
}
