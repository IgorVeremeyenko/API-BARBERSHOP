using WebApplication2.Models;

namespace WebApplication2.Services.Masters {

    public class MastersService {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Category { get; set; }
        public string Phone { get; set; }
        public List<MasterSchedule>? Days { get; set; }
    }
    public class GenerateMasterList {

        private List<MastersService> _masterServices = new List<MastersService>();
       
        public List<MastersService> MasterList(List<Master> masters, List<Service> services, List<MasterSchedule> masterSchedules) {
            foreach (var master in masters) {
                MastersService result = new MastersService();
                result.Days = new List<MasterSchedule>();
                result.Id = master.Id;
                result.Name = master.Name;
                result.Phone = master.Phone;
                result.Category = services.Where(p => p.MasterId == master.Id).Select(m => m.Category).ToList();
                result.Days = masterSchedules.Where(p => p.MasterId == master.Id).ToList();
                _masterServices.Add(result);
            }
            return _masterServices;
        }
    }
}
