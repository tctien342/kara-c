using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KaraManager
{
    public class APIManager
    {   
        // Class Config
        
        private string serverURL = "http://kara342.tk";

        // StateData
        private string token = ""; // Token use for server connecting
        public int userId;
        public string userName;
        public dynamic roomsI;
        public dynamic roomsType;
        public dynamic prodTime;
        public dynamic prodUse;

        // Class constructor
        public APIManager()
        {
            
        }


        // Logout funct => remove all user state
        public void Logout()
        {
            token = "";
            userId = 0;
            userName = "";
        }

        // Init class funct => getting all state from server
        public async Task<int> InitClass(string email, string pasword)
        {
            // Fake login
            dynamic user = new ExpandoObject();
            user.email = email;
            user.password = pasword;
            var objT = await PostAPI(serverURL + "/api/auth/login", user);
            dynamic obj = objT;

            // Setting token
            if ((int)obj.error_code != 0) return -1;
            token = obj.data.token;
            userId = (int)obj.data.user.id;
            userName = (string)obj.data.user.name;

            // Getting rooms
            var rooms = await GetAPI(serverURL + "/api/ds_room", "", token);
            dynamic roomsInfo = rooms;
            roomsI = roomsInfo.data;

            // Getting rooms types
            var roomsT = await GetAPI(serverURL + "/api/get_room_types", "", token);
            dynamic roomsTypeI = roomsT;
            roomsType = roomsTypeI.data;

            // Getting prod type
            var prodTimeR = await GetAPI(serverURL + "/api/get_time_prod", "", token);
            dynamic prodTimeI = prodTimeR;
            prodTime = prodTimeI.data;

            var prodUseR = await GetAPI(serverURL + "/api/get_all_prod", "", token);
            dynamic prodUseI = prodUseR;
            prodUse = prodUseI.data;

            // Return 0 for need for waiting funct
            return 0;
        }

        // Get all active bill from server
        public async Task<JObject> GetActiveBill()
        {
            string url = serverURL + "/api/ds_active_bill";
            return await GetAPI(url, "", token);
        }

        // Get all not active room
        public async Task<Object> GetEmptyRoom()
        {
            string url = serverURL + "/api/ds_empty_room";
            return await GetAPI(url, "", token);
        }

        // Get all time product from server
        public async Task<Object> GetTimeProd()
        {
            string url = serverURL + "/api/get_time_prod";
            return await GetAPI(url, "", token);
        }

        // Get month income
        public async Task<Object> GetMonth()
        {
            string url = serverURL + "/api/get_month";
            return await GetAPI(url, "", token);
        }

        // Make and active bill => open room
        public async Task<Object> PostMakeBill(int roomId, int timeId)
        {
            dynamic bill = new ExpandoObject();
            bill.room_id = roomId;
            bill.time_id = timeId;
            return await PostAPI(serverURL + "/api/post_make_bill", bill,token);
        }

        // Edit an even of the bill like changing number of product
        public async Task<Object> PostEditEven(int evenId, int prodId, int number)
        {
            dynamic even = new ExpandoObject();
            even.even_id = evenId;
            even.prod_id = prodId;
            even.number = number;
            return await PostAPI(serverURL + "/api/post_edit_even", even, token);
        }

        // Add an even for bill
        public async Task<Object> PostAddEven(int billId, int prodId, int number)
        {
            dynamic even = new ExpandoObject();
            even.bill_id = billId;
            even.prod_id = prodId;
            even.number = number;
            return await PostAPI(serverURL + "/api/post_add_even", even ,token);
        }

        // Getting an bill information by id
        public async Task<Object> GetBill(int billId)
        {
            string url = serverURL + "/api/get_bill";
            return await GetAPI(url, "?bill_id="+billId, token);
        }

        // Close an opening room => paid the bill
        public async Task<Object> PostPaidBill(int billId, long money)
        {
            dynamic bill = new ExpandoObject();
            bill.bill_id = billId;
            bill.value = money;
            return await PostAPI(serverURL + "/api/post_paid_bill", bill, token);
        }

        // Get API funct
        // Push server link - extra data - token to funct
        private static async Task<JObject> GetAPI(string apiLink, string extra, string token = "")
        {
            var url = apiLink + extra;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string strResult = await response.Content.ReadAsStringAsync();

                    return JObject.Parse(strResult);
                }
                else
                {
                    return null;
                }
            }
        }

        // Post API funct
        // Push server link - object information - token to funct
        private static async Task<JObject> PostAPI(string apiLink, dynamic obj, string token = "")
        {
            var url = apiLink;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url,content);

                if (response.IsSuccessStatusCode)
                {
                    string strResult = await response.Content.ReadAsStringAsync();
                    
                    return JObject.Parse(strResult);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
