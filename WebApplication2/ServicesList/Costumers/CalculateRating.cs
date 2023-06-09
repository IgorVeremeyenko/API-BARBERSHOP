using WebApplication2.Models;
using WebApplication2.Models.Costumer;

namespace WebApplication2.Services.Costumers
{

    public class CalculateRating
    {

        public List<CostumersWithRating> costumersWithRatings;

        public CalculateRating()
        {
        }
        public List<CostumersWithRating> Rating(List<Costumer> costumers)
        {

            int successfulVisits = 0;
            int totalVisits = 0;
            int rating = 0;
            double successRate = 0.0;
            List<Appointment> apps = new List<Appointment>();
            List<Service> services = new List<Service>();

            costumersWithRatings = new List<CostumersWithRating>();

            foreach (Costumer costumer in costumers)
            {
                CostumersWithRating app = new CostumersWithRating();
                successfulVisits = costumer.Statistics.Where(p => p.Complete > 0 && p.CostumerId == costumer.Id).Count();
                totalVisits = costumer.Statistics.Where(p => p.CostumerId == costumer.Id).Count();
                apps = costumer.Appointments.Where(a => a.CostumerId == costumer.Id).ToList();
                if (totalVisits > 0)
                {

                    successRate = (double)successfulVisits / totalVisits * 100;

                    if (successRate >= 90)
                    {
                        rating = 5;
                    }
                    else if (successRate >= 80)
                    {
                        rating = 4;
                    }
                    else if (successRate >= 70)
                    {
                        rating = 3;
                    }
                    else if (successRate >= 60)
                    {
                        rating = 2;
                    }
                    else
                    {
                        rating = 1;
                    }
                }
                else
                {
                    rating = 1;
                }


                app.Id = costumer.Id;
                app.Name = costumer.Name;
                app.Phone = costumer.Phone;
                app.RatingValue = rating;
                app.Total = totalVisits;
                app.UserId = costumer.UserId;
                app.SuccessCount = successfulVisits;
                if (apps.Count > 0)
                {
                    app.apps = apps;
                }

                costumersWithRatings.Add(app);

            }
            return costumersWithRatings;
        }

    }
}
