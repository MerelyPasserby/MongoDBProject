using MongoDB.Driver;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace Lab_3
{    
    public partial class App : Form
    {
        string mongodbConnectionString = "mongodb://localhost:27017";
        string dbName = "agency";
        MongoDBService dBService;
        public App()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            dBService = new MongoDBService(mongodbConnectionString, dbName);
        }     

        private void clientInsertButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(idClientInput.Text, out int id)) id = -1;
            string name = nameInput.Text;
            string surname = surnameInput.Text;
            string patronymic = patronymicInput.Text;
            string address = addressInput.Text;
            string phone = phoneInput.Text;

            Client client = new Client(id, name, surname, patronymic, address, phone);
            try
            {
                Validator.ValidateClient(client);
                dBService.Insert("clients", client);
                clientStatusStripLabel.Text = "Client added";
            }
            catch (MongoWriteException)
            {
                clientStatusStripLabel.Text = "Failed to insert client: id already exists";
            }
            catch (Exception ex)
            {
                clientStatusStripLabel.Text = ex.Message;
            }
        }
        private void routeInsertButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(idRouteInput.Text, out int id)) id = -1;
            string country = countryInput.Text;
            string climate = climateInput.Text;
            if (!int.TryParse(durationInput.Text, out int duration)) duration = -1;
            string hotel = hotelInput.Text;
            if (!int.TryParse(priceInput.Text, out int price)) price = -1;

            Route route = new Route(id, country, climate, duration, hotel, price);
            try
            {
                Validator.ValidateRoute(route);
                dBService.Insert("routes", route);
                routeStatusStripLabel.Text = "Route added";
            }
            catch (MongoWriteException)
            {
                routeStatusStripLabel.Text = "Failed to insert route: id already exists";
            }
            catch (Exception ex)
            {
                routeStatusStripLabel.Text = ex.Message;
            }
        }
        private void travelInsertButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(idTravelInput.Text, out int id)) id = -1;
            if (!int.TryParse(routeIdInput.Text, out int routeId)) routeId = -1;
            if (!int.TryParse(clientIdInput.Text, out int clientId)) clientId = -1;
            DateTime date = dateInput.Value;
            if (!int.TryParse(amountInput.Text, out int amount)) amount = -1;
            if (!int.TryParse(discountInput.Text, out int discount)) discount = -1;
            if (!int.TryParse(costInput.Text, out int cost)) cost = -1;

            Travel travel = new Travel(id, routeId, clientId, date, amount, discount, cost);
            try
            {
                Validator.ValidateTravel(travel);
                dBService.Insert("travels", travel);
                travelStatusStripLabel.Text = "Travel added";
            }
            catch (MongoWriteException)
            {
                travelStatusStripLabel.Text = "Failed to insert travel: id already exists";
            }
            catch (Exception ex)
            {
                travelStatusStripLabel.Text = ex.Message;
            }
        }
        private void reviewInsertButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(idReviewInput.Text, out int id)) id = -1;
            if (!int.TryParse(reviewClientIdInput.Text, out int clientId)) clientId = -1;
            if (!int.TryParse(reviewRouteIdInput.Text, out int routeId)) routeId = -1;
            var date = reviewDateInput.Value;
            if (!int.TryParse(reviewRatingInput.Text, out int rating)) rating = -1;
            string reviewText = reviewTextInput.Text;

            Review review = new Review(id, clientId, routeId, rating, reviewText, date);

            try
            {
                Validator.ValidateReview(review);
                dBService.Insert("reviews", review);
                reviewStatusLabel.Text = "Review added";
            }
            catch (MongoWriteException)
            {
                reviewStatusLabel.Text = "Failed to insert review: id already exists";
            }
            catch (Exception ex)
            {
                reviewStatusLabel.Text = ex.Message;
            }
        }
        
        private void clientDeleteButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(deleteClientIdInput.Text, out int id)) id = -1;

            try
            {
                Validator.ValidateId(id);
                var filter = Builders<Client>.Filter.Eq(el => el.Id, id);
                if (dBService.Delete("clients", filter))
                {
                    deleteClientStatusStripLabel.Text = "Client deleted";
                }
                else
                {
                    deleteClientStatusStripLabel.Text = "Nothing deleted";
                }

            }
            catch (Exception ex)
            {
                deleteClientStatusStripLabel.Text = ex.Message;
            }
        }
        private void routeDeleteButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(deleteRouteIdInput.Text, out int id)) id = -1;

            try
            {
                Validator.ValidateId(id);
                var filter = Builders<Route>.Filter.Eq(el => el.Id, id);
                if (dBService.Delete("routes", filter))
                {
                    deleteRouteStatusStripLabel.Text = "Route deleted";
                }
                else
                {
                    deleteRouteStatusStripLabel.Text = "Nothing deleted";
                }

            }
            catch (Exception ex)
            {
                deleteRouteStatusStripLabel.Text = ex.Message;
            }
        }
        private void travelDeleteButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(deleteTravelIdInput.Text, out int id)) id = -1;

            try
            {
                Validator.ValidateId(id);
                var filter = Builders<Travel>.Filter.Eq(el => el.Id, id);
                if (dBService.Delete("travels", filter))
                {
                    deleteTravelStatusStripLabel.Text = "Travel deleted";
                }
                else
                {
                    deleteTravelStatusStripLabel.Text = "Nothing deleted";
                }

            }
            catch (Exception ex)
            {
                deleteTravelStatusStripLabel.Text = ex.Message;
            }
        }
        private void reviewDeleteButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(deleteReviewIdInput.Text, out int id)) id = -1;

            try
            {
                Validator.ValidateId(id);
                var filter = Builders<Review>.Filter.Eq(el => el.Id, id);
                if (dBService.Delete("reviews", filter))
                {
                    deleteReviewStatusLabel.Text = "Review deleted";
                }
                else
                {
                    deleteReviewStatusLabel.Text = "Nothing deleted";
                }

            }
            catch (Exception ex)
            {
                deleteReviewStatusLabel.Text = ex.Message;
            }
        }

        private void clientUpdateButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(idClientUpdateInput.Text, out int id)) id = -1;
            string name = nameUpdateInput.Text;
            string surname = surnameUpdateInput.Text;
            string patronymic = patronymicUpdateInput.Text;
            string address = addressUpdateInput.Text;
            string phone = phoneUpdateInput.Text;

            Client client = new Client(id, name, surname, patronymic, address, phone);
            try
            {
                Validator.ValidateClient(client);
                var filter = Builders<Client>.Filter.Eq(el => el.Id, id);
                if (dBService.Update("clients", filter, client))
                {
                    updateClientStripStatusLabel.Text = "Client updated";
                }
                else
                {
                    updateClientStripStatusLabel.Text = "Nothing updated";
                }

            }
            catch (Exception ex)
            {
                updateClientStripStatusLabel.Text = ex.Message;
            }
        }
        private void routeUpdateButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(idRouteUpdateInput.Text, out int id)) id = -1;

            string country = countryUpdateInput.Text;
            string climate = climateUpdateInput.Text;
            if (!int.TryParse(durationUpdateInput.Text, out int duration)) duration = -1;
            string hotel = hotelUpdateInput.Text;

            if (!double.TryParse(priceUpdateInput.Text, out double tmpPrice)) tmpPrice = -1;
            int price = (int)tmpPrice;

            Route route = new Route(id, country, climate, duration, hotel, price);
            try
            {
                Validator.ValidateRoute(route);
                var filter = Builders<Route>.Filter.Eq(el => el.Id, id);
                if (dBService.Update("routes", filter, route))
                {
                    updateRouteStripStatusLabel.Text = "Route updated";
                }
                else
                {
                    updateRouteStripStatusLabel.Text = "Nothing updated";
                }

            }
            catch (Exception ex)
            {
                updateRouteStripStatusLabel.Text = ex.Message;
            }
        }
        private void travelUpdateButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(idTravelUpdateInput.Text, out int id)) id = -1;

            if (!int.TryParse(routeIdUpdateInput.Text, out int routeId)) routeId = -1;
            if (!int.TryParse(clientIdUpdateInput.Text, out int clientId)) clientId = -1;
            DateTime date = dateUpdateInput.Value;
            if (!int.TryParse(amountUpdateInput.Text, out int amount)) amount = -1;
            if (!int.TryParse(discountUpdateInput.Text, out int discount)) discount = -1;

            if (!double.TryParse(costUpdateInput.Text, out double tmpCost)) tmpCost = -1;
            int cost = (int)tmpCost;

            Travel travel = new Travel(id, routeId, clientId, date, amount, discount, cost);
            try
            {
                Validator.ValidateTravel(travel);
                var filter = Builders<Travel>.Filter.Eq(el => el.Id, id);
                if (dBService.Update("travels", filter, travel))
                {
                    updateTravelStripStatusLabel.Text = "Travel updated";
                }
                else
                {
                    updateTravelStripStatusLabel.Text = "Nothing updated";
                }

            }
            catch (Exception ex)
            {
                updateTravelStripStatusLabel.Text = ex.Message;
            }
        }
        private void reviewUpdateButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(reviewIdUpdateInput.Text, out int id)) id = -1;
            if (!int.TryParse(reviewRouteIdUpdateInput.Text, out int routeId)) routeId = -1;
            if (!int.TryParse(reviewClientIdUpdateInput.Text, out int clientId)) clientId = -1;
            if (!int.TryParse(reviewRatingUpdateInput.Text, out int rating)) rating = -1;
            DateTime date = reviewDateUpdateInput.Value;
            string text = reviewTextUpdateInput.Text;

            Review review = new Review(id, clientId, routeId, rating, text, date);

            try
            {
                Validator.ValidateReview(review);
                var filter = Builders<Review>.Filter.Eq(el => el.Id, id);
                if (dBService.Update("reviews", filter, review))
                {
                    updateReviewStatusLabel.Text = "Review updated";
                }
                else
                {
                    updateReviewStatusLabel.Text = "Nothing updated";
                }


            }
            catch (Exception ex)
            {
                updateReviewStatusLabel.Text = ex.Message;
            }
        }
        private void clientSearchButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(idClientUpdateInput.Text, out int id)) id = -1;

            try
            {
                Validator.ValidateId(id);
                var filter = Builders<Client>.Filter.Eq(el => el.Id, id);
                var client = dBService.Find("clients", filter).FirstOrDefault();

                if (client != null)
                {
                    nameUpdateInput.Text = client.Name;
                    surnameUpdateInput.Text = client.Surname;
                    patronymicUpdateInput.Text = client.Patronymic;
                    addressUpdateInput.Text = client.Address;
                    phoneUpdateInput.Text = client.Phone;
                    updateClientStripStatusLabel.Text = "Got the client";
                }
                else
                {
                    updateClientStripStatusLabel.Text = "Got nothing";
                }
            }
            catch (Exception ex)
            {
                updateClientStripStatusLabel.Text = ex.Message;
            }
        }
        private void routeSearchButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(idRouteUpdateInput.Text, out int id)) id = -1;

            try
            {
                Validator.ValidateId(id);
                var filter = Builders<Route>.Filter.Eq(el => el.Id, id);
                var route = dBService.Find("routes", filter).FirstOrDefault();

                if (route != null)
                {
                    countryUpdateInput.Text = route.Country;
                    climateUpdateInput.Text = route.Climate;
                    durationUpdateInput.Text = route.Duration.ToString();
                    hotelUpdateInput.Text = route.Hotel;
                    priceUpdateInput.Text = route.Money.ToString();
                    updateRouteStripStatusLabel.Text = "Got the route";
                }
                else
                {
                    updateRouteStripStatusLabel.Text = "Got nothing";
                }

            }
            catch (Exception ex)
            {
                updateRouteStripStatusLabel.Text = ex.Message;
            }
        }
        private void travelSearchButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(idTravelUpdateInput.Text, out int id)) id = -1;

            try
            {
                Validator.ValidateId(id);
                var filter = Builders<Travel>.Filter.Eq(el => el.Id, id);
                var travel = dBService.Find("travels", filter).FirstOrDefault();

                if (travel != null)
                {
                    routeIdUpdateInput.Text = travel.RouteId.ToString();
                    clientIdUpdateInput.Text = travel.ClientId.ToString();
                    dateUpdateInput.Value = travel.Date;
                    amountUpdateInput.Text = travel.Amount.ToString();
                    discountUpdateInput.Text = travel.Discount.ToString();
                    costUpdateInput.Text = travel.Cost.ToString();
                    updateTravelStripStatusLabel.Text = "Got the travel";
                }
                else
                {
                    updateTravelStripStatusLabel.Text = "Got nothing";
                }
            }
            catch (Exception ex)
            {
                updateTravelStripStatusLabel.Text = ex.Message;
            }
        }
        private void reviewSearchButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(reviewIdUpdateInput.Text, out int id)) id = -1;

            try
            {
                Validator.ValidateId(id);
                var filter = Builders<Review>.Filter.Eq(el => el.Id, id);
                var review = dBService.Find("reviews", filter).FirstOrDefault();

                if (review != null)
                {
                    reviewClientIdUpdateInput.Text = review.ClientId.ToString();
                    reviewRouteIdUpdateInput.Text = review.RouteId.ToString();
                    reviewRatingUpdateInput.Text = review.Rating.ToString();
                    reviewDateUpdateInput.Value = review.Date;
                    reviewTextUpdateInput.Text = review.ReviewText;
                    updateReviewStatusLabel.Text = "Got the review";
                }
                else
                {
                    updateReviewStatusLabel.Text = "Got nothing";
                }
            }
            catch (Exception ex)
            {
                updateReviewStatusLabel.Text = ex.Message;
            }
        }

        private void refreshClientButton_Click(object sender, EventArgs e)
        {
            idClientInput.Text = "";
            nameInput.Text = "";
            surnameInput.Text = "";
            patronymicInput.Text = "";
            addressInput.Text = "";
            phoneInput.Text = "";
            clientStatusStripLabel.Text = "Form refreshed";
        }
        private void refreshRouteButton_Click(object sender, EventArgs e)
        {
            idRouteInput.Text = "";
            countryInput.Text = "";
            climateInput.Text = "";
            durationInput.Text = "";
            hotelInput.Text = "";
            priceInput.Text = "";
            routeStatusStripLabel.Text = "Form refreshed";
        }
        private void refreshTravelButton_Click(object sender, EventArgs e)
        {
            idTravelInput.Text = "";
            routeIdInput.Text = "";
            clientIdInput.Text = "";
            dateInput.Value = DateTime.Now;
            amountInput.Text = "";
            discountInput.Text = "";
            costInput.Text = "";
            travelStatusStripLabel.Text = "Form refreshed";
        }
        private void reviewRefresh_Click(object sender, EventArgs e)
        {
            idReviewInput.Text = "";
            reviewClientIdInput.Text = "";
            reviewRouteIdInput.Text = "";
            reviewRatingInput.Text = "";
            reviewDateInput.Value = DateTime.Now;
            reviewTextInput.Text = "";
            reviewStatusLabel.Text = "Form refreshed";
        }
        private void clientUpdateRefresh_Click(object sender, EventArgs e)
        {
            idClientUpdateInput.Text = "";
            nameUpdateInput.Text = "";
            surnameUpdateInput.Text = "";
            patronymicUpdateInput.Text = "";
            addressUpdateInput.Text = "";
            phoneUpdateInput.Text = "";
            updateClientStripStatusLabel.Text = "Form refreshed";
        }
        private void routeUpdateRefresh_Click(object sender, EventArgs e)
        {
            idRouteUpdateInput.Text = "";
            countryUpdateInput.Text = "";
            climateUpdateInput.Text = "";
            durationUpdateInput.Text = "";
            hotelUpdateInput.Text = "";
            priceUpdateInput.Text = "";
            updateRouteStripStatusLabel.Text = "Form refreshed";
        }
        private void travelUpdateRefresh_Click(object sender, EventArgs e)
        {
            idTravelUpdateInput.Text = "";
            routeIdUpdateInput.Text = "";
            clientIdUpdateInput.Text = "";
            dateUpdateInput.Value = DateTime.Now;
            amountUpdateInput.Text = "";
            discountUpdateInput.Text = "";
            costUpdateInput.Text = "";
            updateTravelStripStatusLabel.Text = "Form refreshed";
        }
        private void reviewUpdateRefresh_Click(object sender, EventArgs e)
        {
            reviewIdUpdateInput.Text = "";
            reviewClientIdUpdateInput.Text = "";
            reviewRouteIdUpdateInput.Text = "";
            reviewRatingUpdateInput.Text = "";
            reviewDateUpdateInput.Value = DateTime.Now;
            reviewTextUpdateInput.Text = "";
            updateReviewStatusLabel.Text = "Form refreshed";

        }

        private void selectClientsButton_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView.DataSource = dBService.GetAll<Client>("clients");
            }
            catch (Exception ex)
            {
                selectStatusStripLabel.Text = ex.Message;
            }
        }
        private void selectRoutesButton_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView.DataSource = dBService.GetAll<Route>("routes");
            }
            catch (Exception ex)
            {
                selectStatusStripLabel.Text = ex.Message;
            }
        }
        private void selectTravelsButton_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView.DataSource = dBService.GetAll<Travel>("travels");
            }
            catch (Exception ex)
            {
                selectStatusStripLabel.Text = ex.Message;
            }
        }
        private void selectReviewsButton_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView.DataSource = dBService.GetAll<Review>("reviews");
            }
            catch (Exception ex)
            {
                selectStatusStripLabel.Text = ex.Message;
            }

        }

        private void selectFIOButton_Click(object sender, EventArgs e)
        {
            try
            {
                var clients = dBService.GetAll<Client>("clients");

                var res = clients.OrderBy(c => c.Surname)
                                 .ThenBy(c => c.Name)
                                 .Select(c => new
                                 {
                                     Surname = c.Surname,
                                     Name = c.Name,
                                     Patronimyc = c.Patronymic
                                 })
                                 .ToList();

                dataGridView.DataSource = res;
            }
            catch (Exception ex)
            {
                selectStatusStripLabel.Text = ex.Message;
            }
        }
        private void selectTurkeyButton_Click(object sender, EventArgs e)
        {
            try
            {
                var routes = dBService.GetAll<Route>("routes");

                var res = routes.Where(r => r.Country == "Òóðö³ÿ" && r.Money <= 40000)
                                .ToList();

                dataGridView.DataSource = res;
            }
            catch (Exception ex)
            {
                selectStatusStripLabel.Text = ex.Message;
            }
        }
        private void selectRouteMoneyButton_Click(object sender, EventArgs e)
        {
            try
            {
                var routes = dBService.GetAll<Route>("routes");
                var travels = dBService.GetAll<Travel>("travels");

                var res = from travel in travels
                          join route in routes
                          on travel.RouteId equals route.Id
                          group travel by new { route.Country, route.Hotel } into routeGroup
                          select new
                          {
                              routeGroup.Key.Country,
                              routeGroup.Key.Hotel,
                              Amount = routeGroup.Count().ToString(),
                              Money = routeGroup.Sum(t => t.Cost).ToString(),
                          };

                dataGridView.DataSource = res.ToList();
            }
            catch (Exception ex)
            {
                selectStatusStripLabel.Text = ex.Message;
            }

        }
        private void selectMinMaxButton_Click(object sender, EventArgs e)
        {
            try
            {
                var clients = dBService.GetAll<Client>("clients");
                var travels = dBService.GetAll<Travel>("travels");

                var res = from travel in travels
                          join client in clients
                          on travel.ClientId equals client.Id
                          group travel by new { client.Surname, client.Name, client.Patronymic } into clientGroup
                          select new
                          {
                              clientGroup.Key.Surname,
                              clientGroup.Key.Name,
                              clientGroup.Key.Patronymic,
                              Min = clientGroup.Min(t => t.Cost),
                              Max = clientGroup.Max(t => t.Cost),
                              Avg = clientGroup.Average(t => t.Cost)
                          };

                dataGridView.DataSource = res.ToList();
            }
            catch (Exception ex)
            {
                selectStatusStripLabel.Text = ex.Message;
            }
        }
        private void selectReviewFullButton_Click(object sender, EventArgs e)
        {
            try
            {
                var clients = dBService.GetAll<Client>("clients");
                var routes = dBService.GetAll<Route>("routes");
                var reviews = dBService.GetAll<Review>("reviews");

                var res = from review in reviews
                          join client in clients
                          on review.ClientId equals client.Id
                          join route in routes
                          on review.RouteId equals route.Id
                          select new
                          {
                              client.Surname,
                              client.Name,
                              client.Patronymic,
                              route.Country,
                              route.Hotel,
                              review.Rating,
                              review.ReviewText
                          };

                dataGridView.DataSource = res.ToList();
            }
            catch (Exception ex)
            {
                selectStatusStripLabel.Text = ex.Message;
            }
        }
        private void selectRouteReviewButton_Click(object sender, EventArgs e)
        {
            try
            {
                var routes = dBService.GetAll<Route>("routes");
                var reviews = dBService.GetAll<Review>("reviews");

                var res = from review in reviews
                          join route in routes
                          on review.RouteId equals route.Id
                          group review by new { route.Country, route.Hotel } into routeGroup
                          select new
                          {
                              routeGroup.Key.Country,
                              routeGroup.Key.Hotel,
                              Count = routeGroup.Count(),
                              AvgRating = routeGroup.Average(r => r.Rating)
                          };

                dataGridView.DataSource = res.ToList();
            }
            catch (Exception ex)
            {
                selectStatusStripLabel.Text = ex.Message;
            }
        }
        private void selectReviewsByDataButton_Click(object sender, EventArgs e)
        {
            try
            {
                var clients = dBService.GetAll<Client>("clients");
                var reviews = dBService.GetAll<Review>("reviews");

                var res = from review in reviews
                          join client in clients
                          on review.ClientId equals client.Id
                          where review.Date >= new DateTime(2024, 9, 1)
                          group review by new { client.Surname, client.Name, client.Patronymic } into clientGroup
                          select new
                          {
                              clientGroup.Key.Surname,
                              clientGroup.Key.Name,
                              clientGroup.Key.Patronymic,
                              Amount = clientGroup.Count()
                          };
                dataGridView.DataSource = res.ToList();
            }
            catch (Exception ex)
            {
                selectStatusStripLabel.Text = ex.Message;
            }
        }
    }
}