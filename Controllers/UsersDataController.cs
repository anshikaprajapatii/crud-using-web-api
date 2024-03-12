using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    public class Response
    {
        public int Status;
        public string Message;
    }

    public class UsersDataController : ApiController
    {
        public IEnumerable<User> Get()
        {
            using (ApiDbContext dbContext = new ApiDbContext())
            {
                return dbContext.Users.ToList();
            }
        }

        public string GetById(int id)
        {
            using (ApiDbContext dbContext = new ApiDbContext())
            {
                var userData = dbContext.Users.Where(x => x.UserId == id).FirstOrDefault();

                if (userData == null)
                {
                    Response response = new Response();
                    response.Status = 0;//true
                    response.Message = "User with ID : " + id + " not found.Please try again";
                    string jsonResponse = JsonConvert.SerializeObject(response);
                    return jsonResponse;
                }

                else
                {
                    string jsonResponse = JsonConvert.SerializeObject(userData);
                    return jsonResponse;
                }
            }
        }

        public string Post(User user)
        {
            using (ApiDbContext dbContext = new ApiDbContext())
            {
                if (user != null)
                {
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();

                    Response response = new Response();
                    response.Status = 0;
                    response.Message = "User saved successfully";
                    string jsonResponse = JsonConvert.SerializeObject(response);
                    return jsonResponse;
                }

                else
                {
                    Response response = new Response();
                    response.Status = 1;
                    response.Message = "Error occured";
                    string jsonRespose = JsonConvert.SerializeObject(response);
                    return jsonRespose;
                }
            }
        }

        public string Put(int id, User user)
        {
            if (user == null)
            {
                //if user is null
                Response response = new Response();
                response.Status = 1;
                response.Message = "Invalid user data";
                string jsonResponse = JsonConvert.SerializeObject(response);
                return jsonResponse;
            }

            using (ApiDbContext dbContext = new ApiDbContext())
            {
                var userData = dbContext.Users.FirstOrDefault(x => x.UserId == id);

                if (userData != null)
                {
                    userData.Username = user.Username;
                    userData.Email = user.Email;
                    userData.Age = user.Age;
                    userData.Gender = user.Gender;

                    dbContext.SaveChanges();
                    Response response = new Response();
                    response.Status = 0;
                    response.Message = "User details updated successfully";
                    string jsonResponse = JsonConvert.SerializeObject(response);
                    return jsonResponse;
                }

                else
                {
                    Response response = new Response();
                    response.Status = 1;
                    response.Message = "User not found";
                    string jsonResponse = JsonConvert.SerializeObject(response);
                    return jsonResponse;
                }
            }
        }

        public string Delete(int id)
        {
            using (ApiDbContext dbContext = new ApiDbContext())
            {
                var userData = dbContext.Users.Where(x => x.UserId == id).FirstOrDefault();

                if(userData == null)
                {
                    Response response = new Response();
                    response.Status = 1;
                    response.Message = "User with ID : " + id + " not found";
                    string jsonResponse = JsonConvert.SerializeObject(response);
                    return jsonResponse;
                }

                else
                {
                    dbContext.Users.Remove(userData);
                    dbContext.SaveChanges();

                    Response response = new Response();
                    response.Status = 0;
                    response.Message = "User with ID :" + id + " deleted successfully";
                    string jsonResponse = JsonConvert.SerializeObject(response);
                    return jsonResponse;
                }
            }
        }
    }
}
